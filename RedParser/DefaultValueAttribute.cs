using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace RedParser
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    sealed class DefaultValueAttribute : Attribute
    {
        readonly object value;
        readonly string valueFunctionName;

        public DefaultValueAttribute(object value)
            : this(value, null)
        {
        }

        public DefaultValueAttribute(object value, string valueFunctionName)
        {
            if (valueFunctionName != null && value != null)
                throw new ArgumentException("Either value OR valueFunctionName must be given", "valueFunctionName");
            this.value = value;
            this.valueFunctionName = valueFunctionName;
        }

        public object Value(Type type)
        {
            if (valueFunctionName == null)
                return value;
            else
            {
                return type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).First(method => method.IsStatic && method.Name.Equals(valueFunctionName))
                    .Invoke(null, new object[] { });
            }
        }
    }
}
