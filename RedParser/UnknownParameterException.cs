using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser
{
    public class UnknownParameterException: HumanXmlDeserializerException
    {
        public UnknownParameterException(string parameterName,
            int startLineNumber, int startLinePosition, int endLineNumber, int endLinePosition)
            : base(string.Format("Unknown parameter '{0}{1}'",
                char.ToUpper(parameterName[0]), parameterName.Substring(1)),
            startLineNumber, startLinePosition, endLineNumber, endLinePosition)
        {
        }
    }
}
