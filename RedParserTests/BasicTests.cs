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
            Parse<Empty>("BasicTests_Empty.xml");
        }

        [TestMethod]
        public void JustOneStringParameter()
        {
            var x = Parse<JustOneStringParameter>("BasicTests_JustOneStringParameter.xml");
            Assert.AreEqual("xyz", x.Name);
        }

        [TestMethod]
        public void JustOneIntegerParameter()
        {
            var x = Parse<JustOneIntegerParameter>("BasicTests_JustOneIntegerParameter.xml");
            Assert.AreEqual(42, x.Quantity);
        }

        [TestMethod]
        public void JustOneFloatParameter()
        {
            var x = Parse<JustOneFloatParameter>("BasicTests_JustOneFloatParameter.xml");
            Assert.AreEqual(33.3f, x.Quantity);
        }

        [TestMethod]
        public void JustOneDoubleParameter()
        {
            var x = Parse<JustOneDoubleParameter>("BasicTests_JustOneDoubleParameter.xml");
            Assert.AreEqual(22.2, x.Quantity);
        }

        [TestMethod]
        public void JustOneDecimalParameter()
        {
            var x = Parse<JustOneDecimalParameter>("BasicTests_JustOneDecimalParameter.xml");
            Assert.AreEqual(11.1M, x.Quantity);
        }

        [TestMethod]
        public void JustOneEnumParameter()
        {
            var x = Parse<JustOneEnumParameter>("BasicTests_JustOneEnumParameter.xml");
            Assert.AreEqual(ConsoleColor.DarkMagenta, x.Color);
        }

        [TestMethod]
        public void JustOneEnumerableParameter()
        {
            var x = Parse<JustOneEnumerableParameter>("BasicTests_JustOneEnumerableParameter.xml");
            AssertAreEnumerationsEqual(
                new int[] { 3, 5, 8, 13 },
                x.Values);
        }

        [TestMethod]
        public void EnumerationOfBase()
        {
            var x = Parse<EnumerationOfBase>("BasicTests_EnumerationOfBase.xml");
            AssertAreEnumerationsEqual(
                new SimpleBaseClass[] {
                    new SimpleBaseClass(33),
                    new SimpleDerivedClass(44, "hello")
                },
                x.Contents);
        }

        [TestMethod]
        public void AllParameters()
        {
            var x = Parse<AllParameters>("BasicTests_AllParameters.xml");
            Assert.AreEqual(1, x.Int);
            Assert.AreEqual(2.22f, x.Float);
            Assert.AreEqual(3.33, x.Double);
            Assert.AreEqual(4.44m, x.Decimal);
            Assert.AreEqual("hello", x.String);
            Assert.AreEqual(ConsoleColor.Blue, x.Enum);
            Assert.AreEqual(new SimpleBaseClass(55), x.BaseClass);
            AssertAreEnumerationsEqual(
                new SimpleBaseClass[] {
                    new SimpleBaseClass(66),
                    new SimpleDerivedClass(77, "hello again")
                },
                x.Enumeration);
        }

        [TestMethod]
        public void TestGenerics()
        {
            var x = Parse<GenericType<int>>("BasicTests_Generics.xml");
            Assert.AreEqual(42, x.Variable1);
            Assert.AreEqual(99, x.Variable2);
        }
    }
}
