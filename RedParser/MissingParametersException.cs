using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RedParser
{
    public class MissingParametersException: HumanXmlDeserializerException
    {
        public readonly IList<string> MissingParameters;
        public MissingParametersException(ICollection<string> missingParameters,
            int startlineNumber, int startLinePosition, int endlineNumber, int endLinePosition)
            : base(string.Format("Missing parameter(s): {0}",
                string.Join(", ", (from missingParameter in missingParameters select char.ToUpper(missingParameter[0]) + missingParameter.Substring(1)))),
            startlineNumber, startLinePosition, endlineNumber, endLinePosition)
        {
            MissingParameters = new List<string>(missingParameters).AsReadOnly();
        }
    }
}
