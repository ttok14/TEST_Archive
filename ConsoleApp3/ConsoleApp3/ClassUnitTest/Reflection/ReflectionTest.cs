using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ConsoleApp3
{
    /// <summary> 부모 클래스를 나타내기 위한 클래스 </summary>
    abstract class MyTestClassRoot
    {

    }

    /// <summary> 자식 클래스 </summary>
    class MyTestClassSub : MyTestClassRoot
    {
        public static int PublicStaticIntValue = 100;
        private static int PrivateStaticIntValue = 500;

        public int myIntValue;
        public string myStringValue;

        public void MyPrintMethodNoParam()
        {
            Console.WriteLine($"{nameof(MyPrintMethodNoParam)} is called !");
        }

        public void MyPrintMethodWithParams(int intParam, string stringParam)
        {
            Console.WriteLine($"{nameof(MyPrintMethodWithParams)} is called ! , int param is : {intParam} , stringParam is : {stringParam} ");
        }

        static public void ThisIsStaticMethod(int intParam)
        {
            Console.WriteLine($"{nameof(ThisIsStaticMethod)} is called ! intParam : {intParam}");
        }
    }

    class ReflectionTest
    {
        public void RunTest()
        {
            #region ====:: 클래스 정보 탈탈 털기 ::====

            PrintInfoByReflection(typeof(MyTestClassSub));

            #endregion

            Console.WriteLine();
            Console.WriteLine("========== 메서드 호출하기 ===============================");
            Console.WriteLine();

            #region ====:: 메서드 호출하기 ::====

            CallMethodByName(new MyTestClassSub(), typeof(MyTestClassSub), $"{nameof(MyTestClassSub.MyPrintMethodWithParams)}");

            Console.WriteLine();

            CallMethodByName(null, typeof(MyTestClassSub), $"{nameof(MyTestClassSub.ThisIsStaticMethod)}");

            #endregion

            Console.WriteLine();
        }

        /// <summary>
        /// <paramref name="type"/> 타입에 대한 온갖 정보 출력해보기
        /// </summary>
        void PrintInfoByReflection(Type type)
        {
            Console.WriteLine($"-- 다음 타입의 갖가지 정보를 Reflection 을 통해 출력하기 : {type.ToString()} --");

            Console.WriteLine($"---- 타입 자체에 관한 정보 ------");
            Console.WriteLine();

            Console.WriteLine($"이름 : {type.Name}");
            Console.WriteLine($"Is Class : {type.IsClass}");
            Console.WriteLine($"Is Array : {type.IsArray}");
            Console.WriteLine($"Base Type : {type.BaseType}");
            Console.WriteLine($"Is Enum : {type.IsEnum}");
            Console.WriteLine($"Assembly Full Name : {type.Assembly.FullName}");
            Console.WriteLine($"GUID : {type.GUID}");
            Console.WriteLine($"NameSpace : {type.Namespace}");
            Console.WriteLine($"Is Value Type : {type.IsValueType}");
            Console.WriteLine($"Is Primitive : {type.IsPrimitive}");

            Console.WriteLine();
            Console.WriteLine("--------------------------------------");

            Console.WriteLine($"---- 해당 타입이 가지고 있는 메서드 정보 ----");
            Console.WriteLine();

            var allMethods = type.GetMethods();

            if (allMethods == null)
            {
                Console.WriteLine("메서드 없음");
            }
            else
            {
                for (int i = 0; i < allMethods.Length; i++)
                {
                    var m = allMethods[i];

                    Console.WriteLine($"------ 메서드 이름 : {m.Name} -------");
                    Console.WriteLine($"해당 메서드를 선언하고 있는 클래스 : {m.DeclaringType}");
                    Console.WriteLine($"메서드 반환 타입 : {m.ReturnType.Name}");

                    var p = m.GetParameters();

                    Console.WriteLine();

                    if (p == null)
                    {
                        Console.WriteLine(" 파라미터 없음 ");
                    }
                    else
                    {
                        Console.WriteLine($" -- 파라미터 : {p.Length} 개 존재 -- \n ");

                        for (int j = 0; j < p.Length; j++)
                        {
                            var pinfo = p[j];

                            Console.WriteLine($"파라미터 이름 : {pinfo.Name}");
                            Console.WriteLine($"Default Value : {pinfo.DefaultValue}");
                            Console.WriteLine($"파라미터 타입 : {pinfo.ParameterType}");
                            Console.WriteLine($"out 키워드 여부 : {pinfo.IsOut}");
                            Console.WriteLine($"파라미터 순서 (Position) : {pinfo.Position}");

                            Console.WriteLine();
                        }
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine("--------------------------------");
        }

        /// <summary> 메서드 호출 함수 </summary>
        ///     => <param name="targetInstance"> 타겟 인스턴스 , static method 를 호출할때는 필요없음 (NULL) </param>
        public void CallMethodByName(object targetInstance, Type type, string name)
        {
            var method = type.GetMethod(name);

            if (method == null)
            {
                Console.WriteLine($"다음 타입 ({type}) 에 {name} 라는 메서드가 없어서 호출 실패");
                return;
            }

            var paramList = method.GetParameters();

            // 파라미터가 존재한다면 개수 설정 
            int paramCnt = paramList != null ? paramList.Length : 0;

            object[] parameters = new object[paramCnt];

            for (int i = 0; i < parameters.Length; i++)
            {
                if (paramList[i].ParameterType.IsValueType == false)
                {
                    /// String 이면 특정 문자열로 파라미터 초기화 
                    if (paramList[i].ParameterType == typeof(System.String))
                    {
                        parameters[i] = "Jayce param hoo! (이 파라미터는 string 일때 고정으로 초기화되게 했음!)";
                    }
                    else
                    {
                        parameters[i] = null;
                    }
                }
                else
                {
                    /// Int32 이면 특정 숫자로 파라미터 초기화
                    if (paramList[i].ParameterType == typeof(System.Int32))
                    {
                        parameters[i] = 5959;
                    }
                }
            }

            // 메서드가 static 이면 null 
            if (method.IsStatic)
            {
                targetInstance = null;
            }

            method.Invoke(targetInstance, parameters);
        }
    }
}
