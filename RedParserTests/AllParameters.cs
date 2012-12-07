using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParserTests
{
    class AllParameters
    {
        public readonly int Int;
        public readonly float Float;
        public readonly double Double;
        public readonly decimal Decimal;
        public readonly string String;
        public readonly ConsoleColor Enum;
        public readonly SimpleBaseClass BaseClass;
        public readonly IEnumerable<SimpleBaseClass> Enumeration;

        public AllParameters(
            [RedParser.XmlName("Int")]
            int int_,
            [RedParser.XmlName("Float")]
            float float_,
            [RedParser.XmlName("Double")]
            double double_,
            [RedParser.XmlName("Decimal")]
            decimal decimal_,
            [RedParser.XmlName("String")]
            string string_,
            [RedParser.XmlName("Enum")]
            ConsoleColor enum_,
            SimpleBaseClass baseClass,
            IEnumerable<SimpleBaseClass> enumeration)
        {
            Int = int_;
            Float = float_;
            Double = double_;
            Decimal = decimal_;
            String = string_;
            Enum = enum_;
            BaseClass = baseClass;
            Enumeration = enumeration;
        }
    }
}
