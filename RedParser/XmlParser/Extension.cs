using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser.XmlParser
{
    struct Extension
    {
        public readonly int StartLineNumber;
        public readonly int StartLinePosition;
        public readonly int EndLineNumber;
        public readonly int EndLinePosition;

        public Extension(int startLine, int startPosition, int endLine, int endPosition)
        {
            StartLineNumber = startLine;
            StartLinePosition = startPosition;
            EndLineNumber = endLine;
            EndLinePosition = endPosition;
        }
    }
}
