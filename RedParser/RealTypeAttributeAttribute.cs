using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class RealTypeAttributeAttribute : Attribute
    {
        readonly string attributeName;

        // This is a positional argument
        public RealTypeAttributeAttribute(string attributeName)
        {
            this.attributeName = attributeName;
            Compulsory = false;
        }

        public string AttributeName
        {
            get { return attributeName; }
        }

        // This is a named argument
        public bool Compulsory { get; set; }
    }
}
