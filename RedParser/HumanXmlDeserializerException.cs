using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RedParser
{
    [Serializable]
    public class HumanXmlDeserializerException : Exception
    {
        public int StartLineNumber { get; private set; }
        public int StartLinePosition { get; private set; }
        public int EndLineNumber { get; private set; }
        public int EndLinePosition { get; private set; }

        public HumanXmlDeserializerException(string message,
            int startLineNumber, int startLinePosition, int endLineNumber, int endLinePosition)
            : base(message) {
            loadData(startLineNumber, startLinePosition, endLineNumber, endLinePosition);
        }

        public HumanXmlDeserializerException(string message, Exception inner,
            int startLineNumber, int startLinePosition, int endLineNumber, int endLinePosition)
            : base(message, inner) {
            loadData(startLineNumber, startLinePosition, endLineNumber, endLinePosition);
        }

        private void loadData(int startLineNumber, int startLinePosition, int endLineNumber, int endLinePosition)
        {
            // -1 to transform base-1 to base-0
            StartLineNumber = startLineNumber - 1;
            StartLinePosition = startLinePosition - 1;
            EndLineNumber = endLineNumber - 1;
            EndLinePosition = endLinePosition - 1;
            // correct because XmlReader doesn't give us precise informations
            StartLinePosition = Math.Max(0, StartLinePosition - 1);
            EndLinePosition = Math.Max(0, EndLinePosition - 2);
            // check
            if (StartLineNumber < 0)
                throw new ArgumentOutOfRangeException("startLineNumber", "startLineNumber cannot be 1 or less");
            if (EndLineNumber < 0)
                throw new ArgumentOutOfRangeException("endLineNumber", "endLineNumber cannot be 1 or less");
            if (StartLinePosition < 0)
                throw new ArgumentOutOfRangeException("startLinePosition", "startLinePosition cannot be 1 or less");
            if (EndLinePosition < 0)
                throw new ArgumentOutOfRangeException("endLinePosition", "endLinePosition cannot be 1 or less");
            if (StartLineNumber > EndLineNumber)
                throw new ArgumentException("endLineNumber", "Must be greater or equal to startLineNumber");
            if (StartLineNumber == EndLineNumber && StartLinePosition > EndLinePosition)
                throw new ArgumentException("endLinePosition", "When start and end are on the same line, must be greater or equal to startLinePosition");
        }

        protected HumanXmlDeserializerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
