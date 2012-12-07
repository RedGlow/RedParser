using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParserTests
{
    class SimpleBaseClass
    {
        public readonly int X;

        public SimpleBaseClass(int x)
        {
            X = x;
        }

        public override bool Equals(object obj)
        {
            return (obj is SimpleBaseClass) &&
                ((SimpleBaseClass)obj).X == X;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode();
        }
    }
}
