using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser
{
    class TooComplexAttributeException: HumanXmlDeserializerException
    {
        public TooComplexAttributeException(string attributeName, int startlineNumber, int startLinePosition, int endLineNumber, int endLinePosition)
            : base(string.Format("Too complicated parameter for an attribute: {0}", attributeName),
            startlineNumber, startLinePosition, endLineNumber, endLinePosition)
        {
        }
    }
}
