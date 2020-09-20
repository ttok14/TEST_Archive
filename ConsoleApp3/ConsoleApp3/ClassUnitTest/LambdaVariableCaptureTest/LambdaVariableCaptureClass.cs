using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.ClassUnitTest.LambdaVariableCaptureTest
{
    public class LambdaVariableCaptureClass
    {
        public class TestClass01
        {
            public Action onClick;
            public void SetOnClilck(Action callback)
            {
                onClick = callback;
            }
        }

        public List<TestClass01> obj = new List<TestClass01>();
    
        public void PrintInt(int n)
        {
            Console.WriteLine(n);
        }

        public LambdaVariableCaptureClass()
        {
            int objCnt = 10;

            for (int i = 0; i < objCnt; i++)
            {
                obj.Add(new TestClass01());
            }

            /*
             * 카운트만큼 돌면서 i 값에 0 ~ 9 들어가도록 의도하여 
             * 작성 . 
             * 하지만 실제로 해당 함수를 호출하여 실행해보면 
             * 전부 값이 10 으로 나오는걸 알 수있음 . 
             * 무슨 현상이냐 ? 
             * 
             * 쉽게 말하면 , 
             * {
             *    Print(i); 
                }
                여기서 하나의 익명 클래스가 생기고 거기에 필드값으로 i 값을 담을수 있는
                int 변수가 하나 생김 . 

                그리고 해당 클래스의 변수 이름도 똑같이 i 로 정해짐 . 

                그리고 그 i 값에 스코프 외부에 있는 즉 for 문안에서 쓰인 i 값이 차례대로 
                0 , 1, 2, 3.. 이렇게 쓰이게됨 . 

                변수는 하난데 값이 계속 쓰이니까 당연히 각 오브젝트마다 각각 다른 i 값이 세팅되있지 않겠지?

                즉 이 부분에서 변수가 하나의 값으로 전부 나오는거는 설명이됨 . 자 근데 왜 10 이냐 ? 
                0~9 인데 , 마지막이면 9 이지 않느냐 ? 

                결론은 아님 . 

                for 문의 원리 즉 , i ++ 를 하다가 i 가 10 이 되면 빠져나옴 . 즉 저 익명 클래스는 i 값이 
                바뀌는 즉시 해당 값을 자신의 변수값 i 에다가 새로 써버림 . 즉 사라지지 않고 저 클래스는 
                계속 메모리에 상주하기 때문에, 해당 i 값은 계속 10 이 되어 버린거임 . 
                스코프밖에 i 값은 10 이 되면서 for 를 탈출하여 스택에서 제거됐지만 , 그 해당 로컬 변수 
                i 값을 익명 클래스는 계속 메모리에 상주하며 자신의 i 값에 for 에서의 i 마지막 값 10 을 
                써버렸다 . 이거임 ㅇㅋ? 

            즉 여기서 알수있는거 , 코드블록 하나마다 익명 클래스 하나가 생길수있음 . 
                그리고 Capture 대상의 변수가 자신의 블록 밖이면 , 그 변수를 '잃지' 않기 위해서 
                내부에다가 별도로 똑같은 데이터형의 똑같은 이름의 변수를 하나 생성함 . 
                그리고 해당 변수를 capture 함 . 바뀌면 바뀌는 즉시 다시 capture 함 . 

                즉 만약 코드블록 안에다가 int i  를 했다면 해당 i 변수는 스택마냥 익명 함수가 종료되면 같이
                사라지게됨 . 근데 저거는 외부임 . 외부이기때문에 정상적인 루트로는 가져올수가 없으므로
                지가 클래스를 만들어서 값을 복사해버리는거임 . ㅇㅋ ? 
             */
            for (int i = 0; i < objCnt; i++)
            {
                obj[i].SetOnClilck(() =>
                {
                    PrintInt(i);
                });
            }
        }

        public void Call(int index)
        {
            obj[index].onClick();
        }
    }
}
