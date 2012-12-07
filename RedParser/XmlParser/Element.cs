using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Reflection;

namespace RedParser.XmlParser
{
    class Element: Node
    {
        private string name;
        public override string Name
        {
            get { return name; }
        }

        private Extension? extension = null;
        public readonly IEnumerator<Node> NodesEnumerator;

        private Dictionary<string, string> parsedAttributes = new Dictionary<string,string>();
        private bool allAttributesParsed = false;
        private XmlReader xmlReader;

        public Element(XmlReader xmlReader)
        {
            if (xmlReader.NodeType != XmlNodeType.Element)
                throw new InvalidOperationException();
            this.xmlReader = xmlReader;
            name = xmlReader.Name;
            NodesEnumerator = getNodes().GetEnumerator();
        }

        public string GetAttribute(string name)
        {
            if (parsedAttributes.ContainsKey(name))
                return parsedAttributes[name];
            else if (!allAttributesParsed)
                return xmlReader.GetAttribute(name);
            else
                return null;
        }

        private int startLineNumber;
        private int startLinePosition;
        private IEnumerable<Node> getNodes()
        {
            var xmlLineInfo = xmlReader as IXmlLineInfo;
            startLineNumber = xmlLineInfo.LineNumber;
            startLinePosition = xmlLineInfo.LinePosition;
            var numAttributes = xmlReader.AttributeCount;
            var isEmptyElement = xmlReader.IsEmptyElement;

            // parse all attributes
            if (numAttributes > 0)
            {
                xmlMoveToAttribute(0);
                for (var attributeIndex = 0;
                    attributeIndex < numAttributes;
                    attributeIndex++)
                {
                    parsedAttributes[xmlReader.Name] = xmlReader.Value;
                    var attributeStartLineNumber = xmlLineInfo.LineNumber;
                    var attributeStartLinePosition = xmlLineInfo.LinePosition;
                    var name = xmlReader.Name;
                    var value = xmlReader.Value;
                    if (attributeIndex < numAttributes - 1)
                        xmlMoveToAttribute(attributeIndex + 1);
                    else
                        xmlReadStartElement();
                    yield return new Attribute(name, value,
                        attributeStartLineNumber, attributeStartLinePosition,
                        xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
                }
            }
            else
                // just pass over start element
                xmlReadStartElement();

            // ok, we parsed all attributes
            allAttributesParsed = true;

            // skip reading content if empty node
            if (!isEmptyElement)
            {
                // parse sub-elements
                while (xmlReader.NodeType != XmlNodeType.EndElement)
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        var newElement = new Element(xmlReader);
                        yield return newElement;
                        newElement.ConsumeAndGetExtension();
                    }
                    else if (xmlReader.NodeType == XmlNodeType.Text)
                    {
                        var text = xmlReadContentAsString();
                        yield return new Text(text, new Extension(
                            startLineNumber, startLinePosition,
                            xmlLineInfo.LineNumber, xmlLineInfo.LinePosition));
                    }
                    else
                        xmlRead();

                // read end element
                xmlReadEndElement();
            }

            // compute extension
            extension = new Extension(startLineNumber, startLinePosition,
                xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
        }

        private object xmlCall(Delegate xmlDelegate, params object[] parameters)
        {
            try
            {
                return xmlDelegate.DynamicInvoke(parameters);
            }
            catch (TargetInvocationException te)
            {
                if (te.InnerException is XmlException)
                {
                    // compute extension
                    var e = (XmlException)te.InnerException;
                    extension = new Extension(startLineNumber, startLinePosition,
                        e.LineNumber, e.LinePosition);
                    throw new UnhandledException(e,
                        extension.Value.StartLineNumber, extension.Value.StartLinePosition,
                        extension.Value.EndLineNumber, extension.Value.EndLinePosition);
                }
                throw te.InnerException;
            }
        }

        private delegate void XmlMoveToAttributeDelegate(int i);
        private void xmlMoveToAttribute(int i)
        {
            xmlCall(new XmlMoveToAttributeDelegate(xmlReader.MoveToAttribute), i);
        }

        private delegate string XmlReadContentAsStringDelegate();
        private string xmlReadContentAsString()
        {
            return (string)xmlCall(new XmlReadContentAsStringDelegate(xmlReader.ReadContentAsString));
        }

        private delegate bool XmlReadDelegate();
        private bool xmlRead()
        {
            return (bool)xmlCall(new XmlReadDelegate(xmlReader.Read));
        }

        private delegate void MethodInvoker();

        private void xmlReadStartElement()
        {
            xmlCall(new MethodInvoker(xmlReader.ReadStartElement));
        }

        private void xmlReadEndElement()
        {
            xmlCall(new MethodInvoker(xmlReader.ReadEndElement));
        }

        public override Extension ConsumeAndGetExtension()
        {
            while (NodesEnumerator.MoveNext())
                NodesEnumerator.Current.ConsumeAndGetExtension();
            Debug.Assert(extension.HasValue);
            return extension.Value;
        }
    }
}
