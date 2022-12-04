using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp3
{
    /// <summary>
    /// <see cref="ManualResetEvent"/> , <see cref="AutoResetEvent"/>
    /// 위 클래스는 Thread 들을 특정 위치에서 대기시키거나 통과시키게 할 수 있음.
    /// 여기선 <see cref="AutoResetEvent"/> 클래스에 대해 테스트 진행
    /// </summary>
    class AutoEventClass
    {
        /// <summary> Thread 동기화 제어할 클래스 , <see cref="ManualResetEvent"/> 과는 다르게
        /// <see cref="AutoResetEvent"/> 는 Signal 상태에서 Thread 가 WaitOne() 을 만나는 순간 
        /// 그대로 통과하며 자동으로 '신호받지않음' 상태로 전환됨. 
        /// 즉 Signal 상태로 전환후 하나의 Thread 가 통과후 자동으로 신호받지않음 전환 처리된 다라는 뜻 . </summary>
        AutoResetEvent are = new AutoResetEvent(true);

        /// <summary>
        /// 테스트 내용
        ///         - <see cref="ManualResetEvent"/> 및 <see cref="AutoResetEvent"/> 를 통해 
        ///         Thread 의 실행을 중간에 멈추거나 (대기 상태로 전환, 즉 신호없음(no Signal)) 
        ///         또는 멈추지 않고 그대로 실행하는 (신호있음 (Signal)) 방법에 대해 테스트
        /// </summary>
        public void RunTest_AutoResetEvent()
        {
            // 생성할 Thread 개수 
            int threadCount = 10;

            /// threadCount 개 만큼 Thread 를 생성하고 생성된 Thread 들은 각각 <see cref="ThreadMethod(object)"/> 함수 실행
            for (int parameter = 0; parameter < threadCount; parameter++)
            {
                new Thread(ThreadMethod).Start(parameter);
            }

            // 엔터키 누를때마다 신호받음 처리로 전환시켜줄 처리를 해줄 Thread 추가
            new Thread(ThreadForSignalOn).Start();
        }

        void ThreadMethod(object parameter)
        {
            int i_param = (int)parameter;

            /// 이 함수를 호출한 Thread 는 현재의 <see cref="AutoResetEvent"/> 의 Signal 이 
            /// On(True) 이라면 그대로 통과 , Off(False) 라면 Blocking 됨 . (CPU 점유X)
            are.WaitOne();

            Console.WriteLine("Passed ! Param is : " + i_param);
        }

        /// <summary> 사용자가 Enter 누를때마다 Signal 을 On 시켜서 (신호받음 상태로 설정) 
        /// 대기중인 하나의 Thread 가 Release 되어 실행을 할 수 있게끔 처리. 
        /// <see cref="AutoResetEvent"/> 의 특징상 하나의 Thread 가 Release 되어 Block 상태에서 풀려나게 되면
        /// * 자동적으로 * 다시 Reset 되어 '신호없음' 상태가 됨. (<see cref="ManualResetEvent"/> 과는 다르게) </summary>
        void ThreadForSignalOn()
        {
            Console.WriteLine($"Press Enter To Set Signal !");

            while (true)
            {
                /// 엔터키 누르면 <see cref="EventWaitHandle.Set"/> 을 호출하여 대기중인 Thread 들중 
                /// 가장 먼저 Block 된 Thread 가 '신호받음' 으로 되어 Release 됨 . 즉 Block 상태에서 풀리고
                /// 다음 코드 실행을 진행 . 
                Console.ReadLine();
                Console.WriteLine($"Set Signal On !");
                are.Set();
            }
        }
    }
}
