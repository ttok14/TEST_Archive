using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static System.Console;

namespace ConsoleApp3.ClassUnitTest.Analysis
{
    class SyntaxAnalysis
    {
        /// <summary>
        /// (https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis)
        /// 
        /// Install 필요한 Package 
        ///             - Microsoft.CodeAnalysis.CSharp 
        ///                 => 이거 하면 알아서 Dependency 있는 애들은 자동으로 Install 됨 . 

        /// <see cref="SyntaxTree"/> 는 쉽게 말해 Source Code File 하나 전체를 분석하여 Data 로 나누어 Tree 화 시킨 개념이라 생각하면 됨
        /// 사소한 Space 나 Semicolon 이나 \t 등 해당 File 에 Written 된 모든 Code 를 SyntaxTree 화 시킬 수 있음 . 그럼 그 해당 SyntaxTree 안에는
        /// 그 Source Code 에 대한 모든 정보가 들어가 있는거임 . 
        ///         => Programming Language 에 따라 Source Code 가 달라지기 때문에 <see cref="SyntaxTree"/> 는 기본적으로 abstract 형태이며
        ///             C# 의 Source Code 로 SyntaxTree 관련 처리를 하고싶다 하면은 <see cref="CSharpSyntaxTree"/> 가 존재 
        ///             
        ///         => Syntax Tree 에서 계층이 존재하는데 , Node -> Token -> Trivia 이렇게 존재함 . 왼쪽일 수록 더 큰 개념을 포괄함 
        ///                 => e.g. Trivia 는 '사소한 것' , '하찮은 것' 이라는 의미로 , Space 같은 것들이 저기에 해당됨. 
        /// 
        /// Syntax Tree View 하기 
        ///     - View -> Other Windows -> Syntax Visualizer 
        ///             => 메뉴에 없으면 Visual Studio Installer 에서 .NET Compiler Platform SDK 인가 그거 안깔려있는지 체크 ㄱㄱ 
        ///</summary>
        public void RunTest()
        {
            // Hello world 출력하는게 전부인 C# Source Code 
            const string programText =
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
                }";

            WriteLine(
                $@"Test Program Source Code Text :

==============================================
{programText}
==============================================

");

            // 위 Source Code 가 CSharp Code 이고 이거로 Syntax Tree 를 구성하기 위해 파싱 및 Instance 생성
            SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);

            // CompilationUnit 은 전체 SyntaxTree 의 가장 상위 Representation 인 Root 를 의미 
            // Syntax Node 상속 받는점 참고 ( Node => Token => Trivia 계층 )
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            WriteLine($"The tree is a : {root.Kind()} Node");
            WriteLine($"The tree has {root.Members.Count} elements in it");
            WriteLine($"The tree has {root.Usings.Count} using statements , They are : ");

            foreach (var element in root.Usings)
            {
                WriteLine($"\t{element.Name}");
            }

            var firstMember = root.Members[0];
            WriteLine($"The first member is a : {firstMember.Kind()}");

            WriteLine();

            // 위 SourceCode 의 첫 멤버는 Namespace 이므로 해당 Syntax 타입으로 캐스팅
            var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;

            WriteLine($"There are {helloWorldDeclaration.Members.Count} members declared in this namespace.");

            WriteLine();

            var firstMemberOfNamespace = helloWorldDeclaration.Members[0];
            WriteLine($"The first member is a {firstMemberOfNamespace.Kind()}.");

            // 해당 NameSpace 의 첫 번째 멤버 즉 class Program 의 Source Code
            WriteLine($"FirstMember Of NameSpace (class)'s SourceCode \n{firstMemberOfNamespace.GetText()}");

            // 해당 Namespace 의 첫번쨰 Member 는 class Program 즉 Class Syntax 로 캐스팅
            var programDeclaration = (ClassDeclarationSyntax)firstMemberOfNamespace;
            WriteLine($"There are {programDeclaration.Members.Count} members declared in the {programDeclaration.Identifier} class");

            WriteLine();

            // 해당 Class 의 첫 번째 Method 는 Main Method 이므로 Method Syntax 로 캐스팅
            var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];
            WriteLine($"The first member is a {mainDeclaration.Kind()}");
            WriteLine($"FirstMember Of Class (method)'s SourceCode \n{mainDeclaration.GetText()}");

            WriteLine($"The return type of the {mainDeclaration.Identifier} method is {mainDeclaration.ReturnType}");
            WriteLine($"The method has {mainDeclaration.ParameterList.Parameters.Count} parameters");
            foreach (ParameterSyntax p in mainDeclaration.ParameterList.Parameters)
            {
                WriteLine($"\tthe type of the {p.Identifier} parameter is {p.Type}");
            }
            WriteLine($"The body text of the {mainDeclaration.Identifier} method follows : ");
            WriteLine(mainDeclaration.Body.ToFullString());

            var firstArg = mainDeclaration.ParameterList.Parameters[0];

            WriteLine($"First arg : {firstArg}");
        }
    }
}
