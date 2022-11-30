using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp3
{
    class MultithreadingTest
    {
        ResetEventClass ResetEventClassTest = new ResetEventClass();

        public void RunTest()
        {
            Console.WriteLine("---- Run Mulththrading Test ----");

            ResetEventClassTest.RunTest();
        }

    }
}
