using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParserTests
{
    class EnumerationOfBase
    {
        public readonly List<SimpleBaseClass> Contents;

        public EnumerationOfBase(IEnumerable<SimpleBaseClass> contents)
        {
            Contents = contents.ToList();
        }
    }
}
