using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public enum TYPE
    {
        t01 = 0,
        t02 = 3,
        t03 = 7
    }

    [Serializable]
    public class GlobalTestClass
    {
        public int n;
        public string str;
        public float f;
    }

    [Serializable]
    public class BinaryFormatterTestClass01
    {
        public int vInt;
        public float vFloat;
        public string vStr;
    }
    
    [Serializable]
    public class EncryptionTestClass
    {
        public string name;
        public int age;

        public override string ToString()
        {
            return string.Format("Name : {0} , Age : {1}", name, age);
        }
    }

    public class LinqTestClass01
    {
        public int n;
    }
}
