using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedParser
{
    public class TypeSearchScope
    {
        public readonly string Namespace;
        public readonly string AssemblyFullName;

        public TypeSearchScope(string namespace_, string assemblyFullName)
        {
            Namespace = namespace_;
            AssemblyFullName = assemblyFullName;
        }

        public TypeSearchScope(Type sampleType)
        {
            Namespace = sampleType.Namespace;
            AssemblyFullName = sampleType.Assembly.FullName;
        }
    }
}
