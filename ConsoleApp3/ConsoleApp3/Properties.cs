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
    public class BinaryFormatterTestClass01
    {
        public int vInt;
        public float vFloat;
        public string vStr;
    }

}
