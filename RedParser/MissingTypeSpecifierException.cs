using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser
{
    class MissingTypeSpecifierException: MissingParametersException
    {
        public MissingTypeSpecifierException(string typeParameter,
            int startlineNumber, int startLinePosition, int endlineNumber, int endLinePosition)
            : base(new string[] { typeParameter },
            startlineNumber, startLinePosition, endlineNumber, endLinePosition)
        {
        }
    }
}
