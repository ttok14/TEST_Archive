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
        /// <summary> <see cref="ManualResetEvent"/> 로 Thread 동기화 제어 테스트 </summary>
        ResetEventClass ResetEventClassTest = new ResetEventClass();
        /// <summary> <see cref="AutoEventClassTest"/> 로 Thread 동기화 제어 테스트 </summary>
        AutoEventClass AutoEventClassTest = new AutoEventClass();

        public void RunTest()
        {
            Console.WriteLine("---- Run Mulththrading Test ----");

            // ResetEventClassTest.RunTest_ManualResetEvent();
            AutoEventClassTest.RunTest_AutoResetEvent();
        }

    }
}
