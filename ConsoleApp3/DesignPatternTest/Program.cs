using System;

namespace DesignPatternTest
{
    class Program
    {
        static void Main(string[] args)
        {
            RunConditionAction();
        }

        /// <summary> 조건 부 행동 </summary>
        static void RunConditionAction()
        {
            new ConditionActionRunner().RunTest();
        }
    }
}
