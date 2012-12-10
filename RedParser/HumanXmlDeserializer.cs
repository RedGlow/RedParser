using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using RedParser.XmlParser;

namespace RedParser
{
    /// <summary>
    /// A serializer thought to produce a human-readable output and error, while
    /// keeping the classes structure clean.
    /// </summary>
    public class HumanXmlDeserializer
    {
        public List<TypeSearchScope> TypeSearchScopes { get; private set;  }

        public HumanXmlDeserializer()
        {
            TypeSearchScopes = new List<TypeSearchScope>();
        }

        public T Deserialize<T>(string filename)
        {
            using (var xmlReader = XmlReader.Create(filename))
                return Deserialize<T>(xmlReader);
        }

        public T Deserialize<T>(XmlReader xmlReader)
        {
            xmlReader.MoveToContent();
            return (T)deserialize(typeof(T), new ElementProvider(xmlReader).Read());
        }

        private object deserialize(Type type, Node node)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (node == null)
                throw new ArgumentNullException("element");

            try
            {
                if (isSimpleType(type))
                    // simple type (int, double, enum, ...) parsing
                    return deserializeSimpleType(type, node);
                else if (type.GetInterfaces().Concat(Enumerable.Repeat(type, 1))
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    // IEnumerable<T> parsing
                    return deserializeIEnumerable(type, node);
                else
                    // Complex type (classes) parsing
                    return deserializeComplexType(type, node);
            }
            catch (HumanXmlDeserializerException)
            {
                throw;
            }
            catch (XmlException xmlException)
            {
                throw new UnhandledException(xmlException,
                    xmlException.LineNumber, xmlException.LinePosition + 1,
                    xmlException.LineNumber, xmlException.LinePosition + 3);
            }
            catch (Exception exc)
            {
                var extension = node.ConsumeAndGetExtension();
                throw new UnhandledException(exc, extension.StartLineNumber, extension.StartLinePosition,
                    extension.EndLineNumber, extension.EndLinePosition);
            }
            finally
            {
                node.ConsumeAndGetExtension();
            }
        }

        private object deserializeSimpleType(Type type, Node node)
        {
            if (node is SimpleContent)
                return deserializeSimpleContent(type, (SimpleContent)node);
            else
            {
                Debug.Assert(node is Element);
                var element = (Element)node;
                element.NodesEnumerator.MoveNext();
                var currentNode = element.NodesEnumerator.Current;
                if (!(currentNode is Text))
                {
                    var extension = element.ConsumeAndGetExtension();
                    throw new TooSimpleElementException(element.Name,
                        extension.StartLineNumber, extension.StartLinePosition,
                        extension.EndLineNumber, extension.EndLinePosition);
                }
                if (element.NodesEnumerator.MoveNext())
                {
                    var extension = element.ConsumeAndGetExtension();
                    throw new TooSimpleElementException(element.Name,
                        extension.StartLineNumber, extension.StartLinePosition,
                        extension.EndLineNumber, extension.EndLinePosition);
                }
                return deserializeSimpleContent(type, (Text)currentNode);
            }
        }

        private object deserializeIEnumerable(Type type, Node node)
        {
            if (!(node is Element))
            {
                var extension = node.ConsumeAndGetExtension();
                throw new TooSimpleElementException(node.Name,
                    extension.StartLineNumber, extension.StartLinePosition,
                    extension.EndLineNumber, extension.EndLinePosition);
            }
            var element = (Element)node;

            var genericListType = typeof(List<>).MakeGenericType(type.GetGenericArguments()[0]);
            var list = genericListType.GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            var addMethod = genericListType.GetMethod("Add");
            while (element.NodesEnumerator.MoveNext())
            {
                var currentNode = element.NodesEnumerator.Current;
                var nodeName = currentNode.Name;
                if (nodeName == "null")
                {
                    addMethod.Invoke(list, new object[] { null });
                    continue;
                }
                Type specificType = findType(type, nodeName);
                if (specificType == null)
                {
                    var extension = currentNode.ConsumeAndGetExtension();
                    throw new TypeNotFoundException(nodeName,
                        extension.StartLineNumber, extension.StartLinePosition,
                        extension.EndLineNumber, extension.EndLinePosition);
                }
                var subObject = deserialize(specificType, currentNode);
                addMethod.Invoke(list, new object[] { subObject });
            }
            return list;
        }

        private object deserializeComplexType(Type type, Node node)
        {
            if (!(node is Element))
            {
                var extension = node.ConsumeAndGetExtension();
                throw new TooSimpleElementException(node.Name,
                    extension.StartLineNumber, extension.StartLinePosition,
                    extension.EndLineNumber, extension.EndLinePosition);
            }
            var element = (Element)node;
            // find real type if necessary
            var realTypeAttributes = (RealTypeAttributeAttribute[])
                type.GetCustomAttributes(typeof(RealTypeAttributeAttribute), false);
            string typeAttribute = null;
            if (realTypeAttributes.Length != 0)
            {
                Debug.Assert(realTypeAttributes.Length == 1);
                var realTypeAttribute = realTypeAttributes[0];
                var typeName = element.GetAttribute(realTypeAttribute.AttributeName);
                if (typeName == null && realTypeAttribute.Compulsory)
                {
                    var extension = element.ConsumeAndGetExtension();
                    throw new MissingTypeSpecifierException(
                        "typeName",
                        extension.StartLineNumber, extension.StartLinePosition,
                        extension.EndLineNumber, extension.EndLinePosition);
                }
                if (typeName != null)
                    type = findType(type, typeName);
                typeAttribute = realTypeAttribute.AttributeName;
            }
            // get constructor and parameters
            var constructors = type.GetConstructors();
            if (constructors.Length != 1)
            {
                var extension = node.ConsumeAndGetExtension();
                throw new AmbiguousConstructorException(type,
                    extension.StartLineNumber, extension.StartLinePosition,
                    extension.EndLineNumber, extension.EndLinePosition);
            }
            var constructor = constructors[0];
            var constructorParameters = constructor.GetParameters();
            var constructorParametersByName = constructorParameters.ToDictionary(cp => cp.Name);
            var parameters = new Dictionary<string, object>();
            // get name translations for XML
            var xmlNameToConstructorName =
                new Dictionary<string, string>();
            foreach (var constructorParameter in constructorParameters)
            {
                var parameterName = constructorParameter.Name;
                var xmlNameAttributes =
                    constructorParameter.GetCustomAttributes(typeof(XmlNameAttribute), false);
                if (xmlNameAttributes.Length != 0)
                    xmlNameToConstructorName[
                        ((XmlNameAttribute)xmlNameAttributes[0]).Value] =
                        parameterName;
                else
                    xmlNameToConstructorName[
                        char.ToUpper(parameterName[0]) + parameterName.Substring(1)] =
                        parameterName;
            }
            // parse sub-nodes
            var nodeExtensions = new Dictionary<string, Extension>();
            while (element.NodesEnumerator.MoveNext())
            {
                var currentNode = element.NodesEnumerator.Current;
                // skip attribute if type attribute
                if (typeAttribute != null && currentNode is XmlParser.Attribute &&
                    currentNode.Name.Equals(typeAttribute))
                    continue;
                // check name
                string name = null;
                xmlNameToConstructorName.TryGetValue(currentNode.Name, out name);
                if (name == null || !constructorParametersByName.ContainsKey(name))
                {
                    var extension = currentNode.ConsumeAndGetExtension();
                    throw new UnknownParameterException(name,
                        extension.StartLineNumber, extension.StartLinePosition,
                        extension.EndLineNumber, extension.EndLinePosition);
                }
                // parse sub-element
                parameters[name] = deserialize(
                    constructorParametersByName[name].ParameterType,
                    currentNode);
                // mark as read
                constructorParametersByName.Remove(name);
                // save position
                nodeExtensions[name] = currentNode.ConsumeAndGetExtension();
            }
            // fill parameters from defaults
            var fillerParameters = constructorParametersByName.Values
                .Where(parameter => parameter.RawDefaultValue != DBNull.Value)
                .ToList();
            foreach (var fillerParameter in fillerParameters)
            {
                parameters[fillerParameter.Name] = fillerParameter.RawDefaultValue;
                constructorParametersByName.Remove(fillerParameter.Name);
            }
            // fill parameters from DefaultValueAttribute
            foreach (var missingParameter in constructorParametersByName.Values.ToList())
            {
                var defaultValueAttributes = missingParameter.GetCustomAttributes(
                    typeof(DefaultValueAttribute), false);
                if (defaultValueAttributes.Length > 0)
                {
                    parameters[missingParameter.Name] =
                        ((DefaultValueAttribute)defaultValueAttributes[0]).Value(type);
                    constructorParametersByName.Remove(missingParameter.Name);
                }
            }
            // check missing
            if (constructorParametersByName.Count > 0)
            {
                var extension = node.ConsumeAndGetExtension();
                throw new MissingParametersException(constructorParametersByName.Keys,
                    extension.StartLineNumber, extension.StartLinePosition,
                    extension.EndLineNumber, extension.EndLinePosition);
            }
            // create object
            try
            {
                return constructor.Invoke(
                    (from parameter in constructorParameters
                     select parameters[parameter.Name]).ToArray());
            }
            catch (TargetInvocationException tie)
            {
                if (tie.InnerException is ArgumentException)
                {
                    var ae = tie.InnerException as ArgumentException;
                    if (ae.ParamName != null && nodeExtensions.ContainsKey(ae.ParamName))
                    {
                        var extension = nodeExtensions[ae.ParamName];
                        throw new UnhandledException(ae,
                            extension.StartLineNumber, extension.StartLinePosition,
                            extension.EndLineNumber, extension.EndLinePosition);
                    }
                }
                throw tie.InnerException;
            }
        }

        private bool isSimpleType(Type type)
        {
            return type == typeof(int) ||
                type == typeof(decimal) ||
                type == typeof(double) ||
                type == typeof(float) ||
                type == typeof(bool) ||
                type == typeof(string) ||
                type.IsEnum;
        }

        private object deserializeSimpleContent(Type type, SimpleContent simpleContent)
        {
            var invariant = System.Globalization.CultureInfo.InvariantCulture;
            if (type == typeof(int))
                return int.Parse(simpleContent.Value);
            else if (type == typeof(decimal))
                return decimal.Parse(simpleContent.Value, invariant);
            else if (type == typeof(double))
                return double.Parse(simpleContent.Value, invariant);
            else if (type == typeof(float))
                return float.Parse(simpleContent.Value, invariant);
            else if (type == typeof(bool))
                return bool.Parse(simpleContent.Value);
            else if (type == typeof(string))
                return simpleContent.Value;
            else if (type.IsEnum)
            {
                var enumName = simpleContent.Value;
                var enumValue = parseEnum(type, enumName);
                if (enumValue.HasValue)
                    return enumValue.Value;
                else
                {
                    var extension = simpleContent.ConsumeAndGetExtension();
                    throw new UnknownEnumValueException(type, enumName,
                        extension.StartLineNumber, extension.StartLinePosition,
                        extension.EndLineNumber, extension.EndLinePosition);
                }
            }
            else
            {
                var extension = simpleContent.ConsumeAndGetExtension();
                throw new TooComplexAttributeException(simpleContent.Name,
                    extension.StartLineNumber, extension.StartLinePosition,
                    extension.EndLineNumber, extension.EndLinePosition);
            }
        }

        private Type findType(Type baseType, string baseTypeName)
        {
            Type specificType = null;
            var subType = baseType.IsGenericType ? baseType.GetGenericArguments()[0] : null;
            var subTypeGenerics = subType != null && subType.IsGenericType ? subType.GetGenericArguments() : null;
            var subTypeGenericsString = subType != null && subType.IsGenericType ?
                string.Join(",", from subGeneric in subTypeGenerics select "[" + subGeneric.FullName + "]") :
                null;
            foreach (var typeSearchScope in TypeSearchScopes)
            {
                var typeName = string.Format("{0}.{1}, {2}",
                    typeSearchScope.Namespace,
                    baseTypeName,
                    typeSearchScope.AssemblyFullName);
                specificType = Type.GetType(typeName);
                if (specificType != null)
                    break;
                if (subTypeGenericsString != null)
                {
                    typeName = typeSearchScope + "." + baseTypeName + "`" +
                        subTypeGenerics.Length.ToString() + "[" +
                        subTypeGenericsString + "]";
                    specificType = Type.GetType(typeName);
                    if (specificType != null)
                        break;
                }
            }
            return specificType;
        }

        private static int? parseEnum(Type enumType, string enumName)
        {
            Debug.Assert(enumType.IsEnum);
            var index = Array.IndexOf(Enum.GetNames(enumType), enumName);
            if (index == -1)
                return null;
            else
                return (int)Enum.GetValues(enumType).GetValue(index);
        }

        private static void skipSubtree(XmlReader xmlReader)
        {
            bool isEmptyElement = xmlReader.IsEmptyElement;
            using (var subReader = xmlReader.ReadSubtree())
                while (subReader.Read())
                    ;
            if(!isEmptyElement)
                xmlReader.ReadEndElement();
        }
    }
}
