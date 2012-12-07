using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace RedParser.XmlParser
{
    /// <summary>
    /// An XML parser capable to keeping track of start-end of elements and remove
    /// irregular handling of empty elements.
    /// </summary>
    class ElementProvider
    {
        private XmlReader xmlReader;

        public ElementProvider(XmlReader xmlReader)
        {
            this.xmlReader = xmlReader;
        }

        public Element Read()
        {
            return new Element(xmlReader);
        }
    }
}
