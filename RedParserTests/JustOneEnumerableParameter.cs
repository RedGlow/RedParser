using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParserTests
{
    class JustOneEnumerableParameter
    {
        public readonly List<int> Values;
        
        public JustOneEnumerableParameter(IEnumerable<int> values)
        {
            Values = values.ToList();
        }
    }
}
