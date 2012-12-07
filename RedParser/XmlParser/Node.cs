using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser.XmlParser
{
    abstract class Node
    {
        public abstract string Name { get; }
        public abstract Extension ConsumeAndGetExtension();
    }
}
