using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RedParser
{
    public class AmbiguousConstructorException: HumanXmlDeserializerException
    {
        public readonly Type Type;
        public AmbiguousConstructorException(Type type, int startlineNumber, int startLinePosition, int endlineNumber, int endLinePosition)
            : base(string.Format("Ambiguous constructor for type {0}", type.FullName), startlineNumber, startLinePosition, endlineNumber, endLinePosition)
        {
            Type = type;
        }
    }
}
