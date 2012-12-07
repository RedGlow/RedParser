using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser.XmlParser
{
    class Attribute: SimpleContent
    {
        private string value;
        public override string Value { get { return value; } }

        private Extension extension;
        public override Extension ConsumeAndGetExtension()
        {
            return extension;
        }

        private string name;
        public override string Name
        {
            get { return name; }
        }

        public Attribute(string name, string value,
            int startLine, int startPosition, int endLine, int endPosition)
        {
            this.name = name;
            this.value = value;
            extension = new Extension(startLine, startPosition, endLine, endPosition);
        }
    }
}
