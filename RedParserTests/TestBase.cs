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
            IEnumerable<T> expected,
            IEnumerable<T> actual)
        {
            var expectedEnumerator = expected.GetEnumerator();
            var actualEnumerator = actual.GetEnumerator();
            for (; ; )
            {
                var expectedNext = expectedEnumerator.MoveNext();
                var actualNext = actualEnumerator.MoveNext();
                if (expectedNext && !actualNext)
                    Assert.Fail("Second enumeration shorter than first.");
                else if (!expectedNext && actualNext)
                    Assert.Fail("First enumeration shorter than second.");
                else if (!expectedNext && !actualNext)
                    break;
                else
                    Assert.AreEqual(expectedEnumerator.Current, actualEnumerator.Current);
            }
        }
    }
}
