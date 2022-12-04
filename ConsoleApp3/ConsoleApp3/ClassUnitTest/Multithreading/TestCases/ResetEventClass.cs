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
    /// 여기서는 <see cref="ManualResetEvent"/> 클래스 테스트 진행
    /// </summary>
    class ResetEventClass
    {
        // 생성자에서 Signal (신호) 값을 True/False 로 넣어주는데
        // 여기서 신호가 True 라면은 '신호 받음' 상태가 됨.
        // 여기서 '신호 받음' 상태는 횡단보도를 건너려는 보행자를 생각하면 쉬운데 ,
        // 이 ManualResetEvent 클래스가 횡단보도의 신호 역할을 하고 ,
        // 해당 신호등을 보고 건너려는 보행자는 Thread 가 됨 .
        // 그래서 즉 , 이 ManualResetEvent 클래스가 뭘 하느냐 , 
        // 특정 시점에 현재 실행중인 Thread 를 ManualResetEvent 내부에 설정된 신호에 따라
        // 멈추게 하거나 , 아니면 그대로 진행하게 할 수 있음. 즉 Thread 블록을 걸수가 있음. 
        // 그 기준은 신호가 True 라면 '신호 받음' 으로 판정 및 보행자 신호에서 초록색을 의미
        // False 라면 '신호 없음' 으로 판정 및 보행자 신호에서 붉은색 의미.
        // 결론은 , 현재 '신호 받음' 으로 설정돼 있다면 ManualResetEvent 에서 제공하는 
        // Block 관련 함수를 만나도 해당 Thread 는 무시하고 통과
        // 하지만 '신호 없음' 으로 설정돼있다면 Block 됨. ( e.g. WaitOne() )
        ManualResetEvent mre = new ManualResetEvent(true);

        /// <summary> Thread 들 끼리 변수를 공유함 </summary>
        int shared_variable;

        /// <summary> 테스트 정수 변수값이 (<see cref="ThreadMethod(object)"/>() 참고) 
        /// 이 변수와 값이 같아질때 ManualResetEvent 를 '신호 없음' 으로 전환함 
        /// '신호 없음' 으로 전환되면 이 시점부터 WaitOne() 와도 같은 함수를 호출하게 되면
        /// 해당 Thread 는 블록에 걸림
        /// </summary>
        int waitAt = 5;

        /// <summary>
        /// 테스트 내용
        ///         - <see cref="ManualResetEvent"/> 및 <see cref="AutoResetEvent"/> 를 통해 
        ///         Thread 의 실행을 중간에 멈추거나 (대기 상태로 전환, 즉 신호없음(no Signal)) 
        ///         또는 멈추지 않고 그대로 실행하는 (신호있음 (Signal)) 방법에 대해 테스트
        /// </summary>
        public void RunTest_ManualResetEvent()
        {
            // 생성할 Thread 개수 
            int threadCount = 10;

            /// threadCount 개 만큼 Thread 를 생성하고 생성된 Thread 들은 각각 <see cref="ThreadMethod(object)"/> 함수 실행
            for (int parameter = 0; parameter < threadCount; parameter++)
            {
                new Thread(ThreadMethod).Start(parameter);
            }

            Thread.Sleep(3000);

            // 여기서 다시 '신호 있음' 즉 신호등으로 치면 초록불에 해당하는
            // 상태로 전환 . 
            // 즉, WaitOne() 함수를 호출했을때 '신호 없음' 상태였던 Thread 들은
            // Block 에 걸린 상태 일텐데 , 해당 Thread 들이 이 부분에서 '신호 받음' 으로
            // 전환되어 Block 상태에서 벗어나 실행을 이어나감 .
            mre.Set();
        }

        void ThreadMethod(object parameter)
        {
            int i_param = (int)parameter;

            // WaitOne() 함수가 호출되면 현재 코드를 실행하는 현재 Thread 는 
            // 해당 mre 즉 ManualResetEvent 객체 내부에 설정된 Signal(신호) 값에 따라
            // Block 이 걸리거나 무시를 하고 통과를 하기도 함.
            // '신호받음' 즉 True 라면 통과, False 라면 여기서 대기 상태로 돌입함 (CPU 점유 X)
            // 참고로 반환 타입이 bool 이며 , 신호받지 않는 상태면 반환하지 않고 그대로 블록.
            mre.WaitOne();

            Console.WriteLine(i_param + " , " + shared_variable);

            if (i_param == waitAt)
            {
                Console.WriteLine($"i_param Reached {waitAt} So Resetting the signal ! (means Threads will block at 'mre.WaitOne()' until the execution of 'mre.Set()')");

                // Reset() 함수는 신호를 리셋 , 즉 
                // 신호등으로 치면 초록 -> 빨간 불로 변함 . 
                // 즉 , 신호 없음 상태가 되기 때문에 이제부터 mre.WaitOne() 함수를 만나는 
                // Thread 들은 Block 상태가 될것임. 
                mre.Reset();
            }
        }
    }
}
