using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParserTests
{
    class SimpleDerivedClass: SimpleBaseClass
    {
        public readonly string Y;

        public SimpleDerivedClass(int x, string y)
            : base(x)
        {
            Y = y;
        }

        public override bool Equals(object obj)
        {
            var s = obj as SimpleDerivedClass;
            return s != null &&
                s.X == X &&
                s.Y == Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
