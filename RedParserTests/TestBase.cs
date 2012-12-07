using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedParser;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedParserTests
{
    public class TestBase
    {
        protected T Parse<T>(string filename)
        {
            var d = new HumanXmlDeserializer();
            d.TypeSearchScopes.Add(new TypeSearchScope(typeof(int)));
            d.TypeSearchScopes.Add(new TypeSearchScope(typeof(IEnumerable<int>)));
            d.TypeSearchScopes.Add(new TypeSearchScope(typeof(SimpleBaseClass)));
            var ret = d.Deserialize<T>(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    filename));
            Assert.IsNotNull(ret);
            return ret;
        }

        protected void AssertAreEnumerationsEqual<T>(
            IEnumerable<T> first,
            IEnumerable<T> second)
        {
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            for (; ; )
            {
                var firstNext = firstEnumerator.MoveNext();
                var secondNext = secondEnumerator.MoveNext();
                if (firstNext && !secondNext)
                    Assert.Fail("Second enumeration shorter than first.");
                else if (!firstNext && secondNext)
                    Assert.Fail("First enumeration shorter than second.");
                else if (!firstNext && !secondNext)
                    break;
                else
                    Assert.Equals(firstEnumerator.Current, secondEnumerator.Current);
            }
        }
    }
}
