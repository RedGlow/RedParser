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
        protected T parse<T>(string filename)
        {
            var d = new HumanXmlDeserializer();
            d.DefaultPrefixes.Add("System");
            d.DefaultPrefixes.Add("System.Collections.Generic");
            var ret = d.Deserialize<T>(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    filename));
            Assert.IsNotNull(ret);
            return ret;
        }
    }
}
