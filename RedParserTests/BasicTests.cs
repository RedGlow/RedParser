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
        public void Empty()
        {
            parse<Empty>("BasicTests_Empty.xml");
        }

        [TestMethod]
        public void JustOneStringParameter()
        {
            var x = parse<JustOneStringParameter>("BasicTests_JustOneStringParameter.xml");
            Assert.AreEqual("xyz", x.Name);
        }

        [TestMethod]
        public void JustOneIntegerParameter()
        {
            var x = parse<JustOneIntegerParameter>("BasicTests_JustOneIntegerParameter.xml");
            Assert.AreEqual(42, x.Quantity);
        }

        [TestMethod]
        public void JustOneFloatParameter()
        {
            var x = parse<JustOneFloatParameter>("BasicTests_JustOneFloatParameter.xml");
            Assert.AreEqual(33.3f, x.Quantity);
        }

        [TestMethod]
        public void JustOneDoubleParameter()
        {
            var x = parse<JustOneDoubleParameter>("BasicTests_JustOneDoubleParameter.xml");
            Assert.AreEqual(22.2, x.Quantity);
        }

        [TestMethod]
        public void JustOneDecimalParameter()
        {
            var x = parse<JustOneDecimalParameter>("BasicTests_JustOneDecimalParameter.xml");
            Assert.AreEqual(11.1M, x.Quantity);
        }

        [TestMethod]
        public void JustOneEnumParameter()
        {
            var x = parse<JustOneEnumParameter>("BasicTests_JustOneEnumParameter.xml");
            Assert.AreEqual(ConsoleColor.DarkMagenta, x.Color);
        }

        [TestMethod]
        public void JustOneEnumerableParameter()
        {
            var x = parse<JustOneEnumerableParameter>("BasicTests_JustOneEnumerableParameter.xml");
            Assert.AreEqual(4, x.Values.Count);
            Assert.AreEqual(3, x.Values[0]);
            Assert.AreEqual(5, x.Values[1]);
            Assert.AreEqual(8, x.Values[2]);
            Assert.AreEqual(13, x.Values[3]);
        }
    }
}
