using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RedParser
{
    public class UnhandledException: HumanXmlDeserializerException
    {
        public UnhandledException(Exception inner, int startLineNumber, int startLinePosition, int endLineNumber, int endLinePosition)
            : base(string.Format("Unexpected error: {0}", inner.Message), inner, startLineNumber, startLinePosition, endLineNumber, endLinePosition)
        {
        }
    }
}
