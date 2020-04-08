using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp3;

namespace ConsoleApp3.ClassUnitTest
{
    // Attribute 는 지워도 문제 X (c#문법)

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TestFieldPropertyAttribute : Attribute
    {
        public string name;
        public string _varType;
        public bool _primaryKey { get; private set; }
    }

    public class TestAttributeMapper<T>
    {
        
    }
}
