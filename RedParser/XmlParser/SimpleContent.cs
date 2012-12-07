using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser.XmlParser
{
    abstract class SimpleContent: Node
    {
        public abstract string Value { get; }
    }
}
