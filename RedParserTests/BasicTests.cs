using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedParser;
using System.IO;

namespace RedParserTests
{
    [TestClass]
    public class BasicTests: TestBase
    {
        [TestMethod]
        public void ReadMinimalXml()
        {
            parse<Empty>("BasicTests_ReadMinimalXml.xml");
        }
    }
}
