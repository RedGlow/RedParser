using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser.XmlParser
{
    class Text: SimpleContent
    {
        public override string Name
        {
            get { throw new InvalidOperationException(); }
        }

        private string value;
        public override string Value { get { return value; } }

        private Extension extension;
        public override Extension ConsumeAndGetExtension()
        {
            return extension;
        }

        public Text(string value, Extension extension)
        {
            this.value = value;
            this.extension = extension;
        }
    }
}
