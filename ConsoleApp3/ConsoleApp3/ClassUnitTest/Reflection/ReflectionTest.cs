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
        public void InspectClassInfo()
        {
            #region ====:: 클래스 정보 탈탈 털기 ::====

            PrintInfoByReflection(typeof(MyTestClassSub));

            #endregion

            Console.WriteLine();
        }

        public void MethodCallTest()
        {
            #region ====:: 메서드 호출하기 ::====

            Console.WriteLine("========== 메서드 호출하기 ===============================");
            Console.WriteLine();

            CallMethodByName(new MyTestClassSub(), typeof(MyTestClassSub), $"{nameof(MyTestClassSub.MyPrintMethodWithParams)}");

            Console.WriteLine();

            CallMethodByName(null, typeof(MyTestClassSub), $"{nameof(MyTestClassSub.ThisIsStaticMethod)}");

            #endregion
        }

        /// <summary> 
        /// Assembly 란 ? [https://stackoverflow.com/questions/1362242/what-exactly-is-an-assembly-in-c-sharp-or-net] , [https://docs.microsoft.com/en-us/dotnet/standard/assembly/]
        ///     - Assembly 란 C# 보다는 .Net 의 개념임. (C# 은 .Net 을 기반으로 하는 프로그래밍 언어이며 .Net 의 Specification 들을 구현하기에 C# 의 기반이 .Net 이라함)
        ///     - Assembly 의 형태는 .exe 또는 .dll 
        ///     - Source Code 의 Compile 된 Output 형태
        ///     - Assembly 는 배포(Deployment) 의 가장 최소 단위임
        ///     - Assembly 는 전형적으로 MSIL (Microsoft Intermediate language) 로 이루어져 있음 
        ///         - 해당 MSIL 은 .Net Runtime 에 의해 실행 가능 . 
        ///     - Assembly 은 Type , Class 을 포함 
        ///     - Assembly A 에서 B 를 Load (주로 Binding 이라 부름) 하는 시점은 , B 의 Type 을 실제로 사용하는 시점임. 
        ///             - 해당 시점에 CLR (Common Language Runtime) 이 Output Folder 에 .config 을 찾아서 거기에 기술된 Assembly 리스트를
        ///                 기준으로 Version 정보를 가져와서 해당 Path 에서 Load 시도 
        ///     - Assembly 가 Binding 되는 시점은 Runtime 이다. Runtime 에 StartUp Project 가 Load 되고 다른 Assembly 를 참조하는 시점 , 
        ///                 즉 다른 Assembly 에 포함된 Type 을 사용하는 시점이 실제로 해당 Assembly 를 Binding 시도하는 시점임 .
        ///     - Global Assembly Cache (GAC) 는 여러 Application 들이 Share 할 수 있는 Assembly 들이 위치한 곳임 . 
        /// </summary>
        public void AssemblyTest()
        {
            #region ====:: Assembly 클래스 테스트 ::====

            Console.WriteLine("========== Assembly 클래스 테스트 ===================");
            Console.WriteLine();

            Console.WriteLine("=== 현재 실행중인 Assembly 관련 테스트 ===");
            // 현 Assembly 가져옴 
            var curAssembly = Assembly.GetExecutingAssembly();
            // 이름 정보 가져옴 
            var curAssemblyNameInfo = curAssembly.GetName();

            Console.WriteLine($"This Assembly Name : {curAssemblyNameInfo.Name}");
            Console.WriteLine($"Is Loaded From Global Assembly Cache (GAC) : {curAssembly.GlobalAssemblyCache}");

            Console.WriteLine("=====\n");

            Console.WriteLine($"== Assembly 의 DefinedTypes ==");

            // 해당 Assembly 에서 Define 된 Type 들을 전부 출력
            //      => i.e 람다로 인한 anonymous class 같은 경우도 Compiler 가 Compile 시 Class 로 만들기 때문에 (대체로 DisplayClass 라 불림)
            //              출력해보면 전부 다 출력되는걸 알 수 있음 . (e.g. ConsoleApp3.Program+<>c__DisplayClass37_0)
            foreach (var definedType in curAssembly.DefinedTypes)
            {
                Console.WriteLine($"Following Type : [ {definedType.FullName} ] has been defined");
            }

            Console.WriteLine("=====\n");

            // 
            Console.WriteLine($"EntryPoint : {curAssembly.EntryPoint.Name}");

            Console.WriteLine("=====\n");

            Console.WriteLine($"== Exported Types ==");
            foreach (var type in curAssembly.ExportedTypes)
            {
                Console.WriteLine(type.Name);
            }

            Console.WriteLine("=====\n");

            Console.WriteLine($"This assembly ImageRuntimeVersion : { curAssembly.ImageRuntimeVersion}");
            Console.WriteLine($"This assembly Location : {curAssembly.Location}");

            Console.WriteLine("=====\n");

            Console.WriteLine($"IsDynamic Assembly : {curAssembly.IsDynamic}");

            Console.WriteLine("=====\n");

            var files = curAssembly.GetFiles();

            foreach (var file in files)
            {
                Console.WriteLine($"Assembly File : {file.Name}");
            }

            var assemName = curAssembly.FullName;
            Console.WriteLine("aa:" + assemName);

            Console.WriteLine("=========");
            var assembliesReferencedByThis = curAssembly.GetReferencedAssemblies();

            // Current Assembly 가 참조하는 다른 Assembly 들 출력
            foreach (var assembly in assembliesReferencedByThis)
            {
                Console.WriteLine($"Assembly Referenced By {curAssembly.FullName} : {assembly.Name}");
            }

            Console.WriteLine("=========\n");

            var typesThisAssemblyContains = curAssembly.GetTypes();

            // 현재 Assembly 가 가지고 있는 모든 Type 들 출력
            foreach (var type in typesThisAssemblyContains)
            {
                Console.WriteLine($"Type that this Assembly contains : {type.Name}");
            }

            Console.WriteLine("========\n");

            Console.WriteLine($"===== Manually Load and Check GAC =======");

            foreach (var assemblyName in curAssembly.GetReferencedAssemblies())
            {
                try
                {
                    // Assembly 를 Manual 하게 Load 하기 
                    //      => CLR 이 Assembly 찾는 Directory 기준은 현재 Project 의 Output Folder (e.g. xx/bin/Debug/ ). 
                    //                    해당 Directory 는 Directory.GetCurrentDirectory() 로 가져올수있음 . 
                    //      => e.g. 파라미터로 "ConsoleApp3" 만 적는경우 해당 Project 의 Bin/Debug(또는 Release) Directory 상에서 Search 함 
                    var loadedAssembly = Assembly.Load(assemblyName.Name);
                    Console.WriteLine($"Assembly Loaded Manually : {loadedAssembly.GetName()} , Is GAC : {loadedAssembly.GlobalAssemblyCache}");
                }
                catch (Exception exp)
                {
                    Console.WriteLine($"Assembly Load Manually Failed : {assemblyName.Name}");
                }
            }

            Console.WriteLine("============\n");

            #endregion
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
