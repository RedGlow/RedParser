using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RedParser
{
    public class TypeNotFoundException: HumanXmlDeserializerException
    {
        public readonly string NodeName;
        public TypeNotFoundException(string nodeName, int startlineNumber, int startLinePosition, int endlineNumber, int endLinePosition)
            :base(string.Format("Unknown type {0}", nodeName), startlineNumber, startLinePosition, endlineNumber, endLinePosition)
        {
            NodeName = nodeName;
        }
    }
}
