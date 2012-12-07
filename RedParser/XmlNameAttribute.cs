using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class XmlNameAttribute : Attribute
    {
        public readonly string Value;

        // This is a positional argument
        public XmlNameAttribute(string value)
        {
            Value = value;
        }
    }
}
