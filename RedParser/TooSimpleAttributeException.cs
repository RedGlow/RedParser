using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser
{
    class TooSimpleElementException: HumanXmlDeserializerException
    {
        public TooSimpleElementException(string attributeName, int startlineNumber, int startLinePosition, int endLineNumber, int endLinePosition)
            : base(string.Format("Too simple parameter for a content: {0}", attributeName),
            startlineNumber, startLinePosition, endLineNumber, endLinePosition)
        {
        }
    }
}
