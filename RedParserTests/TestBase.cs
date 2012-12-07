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
            var ret = d.Deserialize<T>(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    filename));
            Assert.IsNotNull(ret);
            return ret;
        }
    }
}
