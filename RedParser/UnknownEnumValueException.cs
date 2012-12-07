using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser
{
    class UnknownEnumValueException: HumanXmlDeserializerException
    {
        public UnknownEnumValueException(Type enumType, string enumName,
            int startLineNumber, int startLinePosition, int endLineNumber, int endLinePosition)
            : base(
            string.Format("Unknown value '{0}'. Only valid values are: {1}.",
                enumName, string.Join(", ", Enum.GetNames(enumType).Select(n => "'" + n + "'"))),
            startLineNumber, startLinePosition, endLineNumber, endLinePosition)
        {
        }
    }
}
