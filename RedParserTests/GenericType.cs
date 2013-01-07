using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParserTests
{
    class GenericType<T>
    {
        public readonly T Variable1;

        public readonly T Variable2;

        public GenericType(T variable1, T variable2)
        {
            Variable1 = variable1;
            Variable2 = variable2;
        }
    }
}
