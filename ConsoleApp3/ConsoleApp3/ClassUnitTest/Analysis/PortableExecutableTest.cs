using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;

using static System.Console;

namespace ConsoleApp3.ClassUnitTest.Analysis
{
    /// <summary>
    /// <see cref=""/>
    /// </summary>
    class PortableExecutableTest
    {
        readonly string OutputName = $"myTest(madeFrom{nameof(PortableExecutableTest)}).dll";

        /// <summary>
        /// Source Code 를 Runtime 에 Compile 을 하고 그걸 Dll 로 저장 테스트
        /// </summary>
        public void CompileAndSaveDLL_Test()
        {
            // 현재 .cs SourceCode File 이 위치한 Directory 가져옴 (어느 Machine 에서도 실행되게)
            var path = Global.GetCallerFilePath();
            var curCsLocation = Path.GetDirectoryName(path);

            // - 혹시 이미 만들어진거 있으면 미리 삭제 - 
            if (File.Exists(Path.Combine(curCsLocation, OutputName)))
            {
                File.Delete(Path.Combine(curCsLocation, OutputName));
            }

            /// <see cref="SyntaxTree"/> 에 대해서는 <see cref="SyntaxAnalysis.SyntaxAnalysis"/> 참고 
            ///         => 간단하게 설명하자면 , 하나의 Source Code File 을 Tree 구조의 Data 로 구조화한 것이 SyntaxTree 임.
            ///             즉 하나의 Source Code File 은 하나의 SyntaxTree 로 Programmactially 하게 표현 가능 
            var trees = new List<SyntaxTree>()
            {
                CSharpSyntaxTree.ParseText(
                    @"using System;
                        using System.Collections;
                        using System.Linq;
                        using System.Text;

                        namespace HelloWorld
                        {
                            class Program
                            {
                                static void Main(string[] args)
                                {
                                    Console.WriteLine(""Hello, World!"");
                                }
                            }
                        }")
        };

            WriteLine();

            // 위 소스코드에서 필요한 Assembly 들 로부터 Location 가져와서 metadata reference 추가
            // (Assembly 참조 추가라고 생각하면 됨)
            var references = new List<MetadataReference>()
            {
                // mscorlib.dll
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                // System.Core.dll
                MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location)
            };

            // 현재 Project 가 참조하는 Assembly 들 가져와서 Add 하는거는 테스트중에 자꾸 Emit 에서 Fail 나서 주석 처리
            //foreach (var item in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            //{
            //    var assembly = Assembly.Load(item);
            //    references.Add(MetadataReference.CreateFromFile(assembly.Location));
            //}
            //  Assembly.GetEntryAssembly().GetReferencedAssemblies()
            ///     .ToList()
            //     .ForEach(a => references.Add(MetadataReference.CreateFromFile(Assembly.Load(a).Location)));

            // SourceCode 자체의 정보인 Syntax Tree 와 
            // Assembly 참조 정보가 있는 Metadata reference 를 넘겨서 dll 형태로 Compile
            var compilation = CSharpCompilation.Create(OutputName
                , trees
                , references
                , new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            // Disk 에 Write (파일 생성)
            var result = compilation.Emit(Path.Combine(curCsLocation, OutputName));

            WriteLine($"IsSuccess : {result.Success}");

            if (result.Success)
            {
                WriteLine("Success");

                Process.Start("explorer.exe", curCsLocation);
            }
            else
            {
                WriteLine($"Failed");

                foreach (var diagnose in result.Diagnostics)
                {
                    WriteLine(diagnose.ToString());
                }
            }
        }
    }
}
