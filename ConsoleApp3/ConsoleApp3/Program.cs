﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Ionic.Zip;
using Ionic.Crc;
using System.Runtime.Serialization.Formatters.Binary;
using ConsoleApp3.ClassUnitTest;
using System.Text.RegularExpressions;
using System.Security.Cryptography; // 암호화 
using System.Diagnostics;
using ConsoleApp3.ClassUnitTest.LambdaVariableCaptureTest;
using System.Reflection;
using System.Threading;
using System.Collections;
//using ConsoleApp3.ClassUnitTest.ConvarianceTest;

/// <summary>
/// ====== 테스트 목록 =======
/// <see cref="ConsoleApp3.Program.BasicMathOperationSpeedTest"/> - 연산 속도 테스트
/// <see cref="ConsoleApp3.Program.IonicSaveZip"/> - ionic 을 이용해서 zip 조작하기
/// <see cref="ConsoleApp3.Program.EncryptionDecryptionTest"/> - 부호화/복호화 테스트 
/// <see cref="ConsoleApp3.Program.LinqUsage"/> - Linq 클래스 활용 테스트 
/// <see cref="ConsoleApp3.Program.ExcelTest"/> - 엑셀 조작 테스트 
/// <see cref="ConsoleApp3.Program.ReflectionTest"/> - Reflection 테스트 
/// <see cref="ConsoleApp3.Program.RegexTest"/> - 정규식 (Regular Expression) 테스트 
/// <see cref="ConsoleApp3.Program.BitOperationTest"/> - 비트 연산 테스트
/// <see cref="ConsoleApp3.Program.CharacterTest"/> - 문자 테스트 , 아스키코드/유니코드 etc
/// <see cref="ConsoleApp3.Program.SortTest"/> - 정렬(Sorting) 테스트 및 활용법들 
/// <see cref="ConsoleApp3.Program.ConvarianceTest"/> - 공변성/반공변성 (generic parameter 앞에 in,out) 테스트  
/// <see cref="ConsoleApp3.Program.DateTime_TimeSpanTest"/> - DateTime 이랑 TimeSpan 활용 테스트 
/// <see cref="ConsoleApp3.Program.LambdaVariableCaptureTest"/> - 람다 Closure 테스트
/// <see cref="ConsoleApp3.Program.BatchFileUsageTest"/> - 배치 파일 조작 테스트 
/// <see cref="ConsoleApp3.Program.ReadOnlyStructTest"/> - 구조체를 Readonly 로 활용하기 테스트 (immutable 구조체만들기)
/// <see cref="ConsoleApp3.Program.UnicodeCharacterAdvanceTest"/> - 유니코드 문자 테스트 , 심화편 . ( 한글 체킹 여부 등 )
/// <see cref="ConsoleApp3.Program.WeakReferenceTest"/> - WeakReference 활용 테스트 ----------------------------------------------------- TODO 
/// <see cref="ConsoleApp3.Program.StringTextUsageTest"/> - String 활용 (Split 등 ..)
/// <see cref="ConsoleApp3.Program.EnvironmentClassUsageTest"/> EnvironmentClassUsageTest - Environment 클래스 활용 테스트 ----------------------------------------------------- TODO 
/// <see cref="ConsoleApp3.Program.AsyncTest"/> - 비동기 async,await,task 관련 활용 테스트 
/// <see cref="ConsoleApp3.Program.TupleTest"/> - Tuple 활용법 테스트 
/// <see cref="ConsoleApp3.Program.IterationUsageTest"/> - Iteration 테스트 (ICollection, IList , 등등 ..)
/// </summary>
namespace ConsoleApp3
{
    class Program
    {
        #region HELPER_METHOD
        static void Print(object str, int linePaddingCount = 0)
        {
            PrintL(str, linePaddingCount);
        }

        static void PrintL(object str, int linePaddingCount = 0)
        {
            string paddingStr = "";

            for (int i = 0; i < linePaddingCount; i++)
            {
                paddingStr += "\n";
            }

            Console.WriteLine(str.ToString() + paddingStr);
        }

        static void PrintW(object str, int spacePaddingCount = 0)
        {
            string paddingStr = "";

            for (int i = 0; i < spacePaddingCount; i++)
            {
                paddingStr += " ";
            }

            Console.Write(str.ToString() + paddingStr);
        }

        static void PadLines(int lineCount = 1, bool printSeparator = false)
        {
            string str = "";

            for (int i = 0; i < lineCount; i++)
            {
                str += "\n";
            }

            if (string.IsNullOrEmpty(str) == false)
                Console.Write(str);

            if (printSeparator)
            {
                Console.WriteLine("================================");
            }
        }
        #endregion

        static void Main(string[] args)
        {
            //// 기본 테스트 환경 세팅 . ///////////
            ProjectUtility.SetupTestEnvironment();
            //////////////////////////////////////

            // 이 밑에서 테스트 진행 
            //BasicMathOperationSpeedTest();
            //IonicSaveZip();
            //EncryptionDecryptionTest();
            LinqUsage();
            // ExcelTest();
            // ReflectionTest();
            // RegexTest();
            // BitOperationTest();
            //CharacterTest();
            //SortTest();
            // ConvarianceTest();
            // DateTime_TimeSpanTest();
            //LambdaVariableCaptureTest();
            //BatchFileUsageTest();
            //ReadOnlyStructTest();
            //UnicodeCharacterAdvanceTest();
            // WeakReferenceTest();
            // EnvironmentClassUsageTest();
            // StringTextUsageTest();
            //TupleTest();
            // IterationUsageTest();

            #region Async 테스트 (Case 별)
            //AsyncTest(AsyncTestCase.AsyncVoidEventHandler);
            // AsyncTest(AsyncTestCase.LongCalculation);
            // AsyncTest(AsyncTestCase.AsyncTest_GameLogic);
            // AsyncTest(AsyncTestCase.AsyncTest_Loading);
            #endregion
        }

        #region 제이스 테스트 코드

        // 주의 ** 바이너리로 만들 class 는 [Serializable] attribute 가 추가돼있어야함. 태그 . 
        static void WriteBinaryFormatterAndSaveAsFile()
        {
            // 이런 방법으로도 초기화가능함. 
            var data = new BinaryFormatterTestClass01[] {
                new BinaryFormatterTestClass01() { vInt = 10, vFloat = 20 , vStr = "first" },
                new BinaryFormatterTestClass01() { vInt = 20, vFloat = 30, vStr  = "second" },
                new BinaryFormatterTestClass01() { vInt = 30, vFloat =40  , vStr = "third" } };

            string outputDir = ProjectUtility.TestDataStoragePath;

            // 맨앞에 역슬래쉬 넣으니까 에러남 뭐임 ? 
            // 무튼 데이터(.dat) 파일 생성하고 FileMode.Create 니까 Write 권한 획득됨. 
            //** 참고로 using 문이 FileStream 의 Dispose 를 호출하고 이 안에서 Close() 도 되기 때문에 별도로 Close 호출 안했음 **//
            using (var fs = new FileStream(outputDir + "/binaryFormatterResultFile.dat", FileMode.Create))
            {
                var bf = new BinaryFormatter();
                // 직렬화해서 쭉 넣어줌 
                bf.Serialize(fs, data);
                ProjectUtility.OpenDataStorageFolder();
            }

            // 다시 deserialize 해서 가져오는 코드 
            using (var fs = new FileStream(outputDir + "/binaryFormatterResultFile.dat", FileMode.Open))
            {
                var bf = new BinaryFormatter();

                var result = bf.Deserialize(fs) as BinaryFormatterTestClass01[];

                for (int i = 0; i < result.Length; i++)
                {
                    Console.WriteLine(result[i].vInt + " " + result[i].vFloat + " " + result[i].vStr);
                }
            }
        }

        // MemoryStream, BinaryFormatter 로 serialized,deserialize 테스트 
        static void SerializeDeserializeByFormatterTest()
        {
            // 원래 데이터 
            string oriData = "IamSoGosu";
            byte[] bytes;

            PrintL("Original Text : " + oriData);

            ///////////////// 바이트로 직렬화하기  ////////////////////

            // 메모리 스트림 할당함 . formatter 가 직렬화 데이터를 넣을 스트림 
            // using 은 MemoryStream 에 Dispose 가 있기때문 
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                // 직렬화함 . stream 에 씀 . 
                formatter.Serialize(memoryStream, oriData);
                // 메모리 스트림에 쓴걸 바이트로 변환 
                bytes = memoryStream.ToArray();
                PrintL("Serialized Object Byte Length : " + bytes.Length);
            }

            //////////////// 원래 데이터로 역직렬화 //////////////////

            // 메모리 스트림 다시 할당 
            // 생성자에 바이트 넘겨주면 걍 바로 스트림에 씀 
            using (var memoryStream = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();
                // deserialize 를 하고 그걸 string 으로 형변환 
                var deserializedString = formatter.Deserialize(memoryStream) as string;
                PrintL("Deserialized Result String : " + deserializedString);
            }
        }

        // zip 파일 압축하기 압축풀기 extract 하기 등등 테스트 코드 @@ 
        // ionic zip 라이브러리 필요 . nuget에서 다운가능
        static void IonicSaveZip()
        {
            Console.WriteLine("IonicSaveZip !! task zipping");

            using (ZipFile zip = new ZipFile())
            {
                string dir = ProjectUtility.TestDataStoragePath + "/ionicTestResult";

                // 존재하면 암것도 안하고 없으면 생성 . 
                Directory.CreateDirectory(dir);

                // MaTxt.txt 를 생성하고 내용 대충 써줌 
                for (int i = 0; i < 3; i++)
                {
                    using (var sw = new StreamWriter(dir + @"\MaTxt" + i.ToString() + ".txt"))
                    {
                        sw.Write("Text Written : " + i.ToString());
                    }
                }

                // 해당 디렉토리 정보 get 
                DirectoryInfo dirInfo = new DirectoryInfo(dir);

                if (dir != null)
                {
                    // 하위 파일 가져옴 . 
                    var files = dirInfo.GetFiles();

                    // 파일을 zip 객체에 add 해줌 
                    for (int i = 0; i < files.Length; i++)
                    {
                        zip.AddFile(files[i].FullName, @"DirFirst\DirSecond"); // Dir\Internal\Zip 은 해당 파일을 압축할때 
                                                                               // 최상위에 위치시키는게 아니라 디렉토리를 생성해서 그 안에 넣고싶을때 
                                                                               // 저렇게 경로로 적어주면 됨 . 
                                                                               // 즉 그냥 빈 문자열 "" 을 적게되면은 , 해당 파일들이 최상위에 위치하여 
                                                                               // 압축을 풀시 바로 해당 파일들이 나오게됨 . 

                        // 즉 압축을 풀었을때 해당 파일들이 바로 나오게하고싶다 , 하면 빈문자열을 넣으면됨 
                    }

                    // 해당 디렉토리에 MaZip.zip 으로 저장함 . 
                    zip.Save(dir + @"\MaZip.zip");

                    // 이 zip 파일을 extract 해서 특정 디렉토리에 더 생성 가능. 
                    zip.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                    zip.ExtractAll(dir + @"\Exctracted");

                    ProjectUtility.OpenDataStorageFolder();
                }
            }
        }

        static void FTP()
        {
            string outputFIlePath = @"C:\Users\user\Desktop\TempTemp\power.aab";
            string ftphost = "192.168.0.121";
            string ftpfilepath = "/WorksDrive/FishingHero/Android/FishingHero_20190714_1353_81_Release.aab";

            string ftpfullpath = "ftp://" + ftphost + ftpfilepath;

            using (WebClient request = new WebClient())
            {
                //   request.Credentials = new NetworkCredential("UserName", "P@55w0rd");
                byte[] fileData = request.DownloadData(ftpfullpath);

                using (FileStream file = File.Create(outputFIlePath))
                {
                    file.Write(fileData, 0, fileData.Length);
                }

                Console.WriteLine("download complete");
            }
            /*
            string url  = "ftp://192.168.0.121/WorksDrive/PinpongAssetBundle/ppablist__1_android.bundlelist";

            FtpWebRequest req;

            req = (FtpWebRequest)FtpWebRequest.Create(url);
            req.KeepAlive = true;
            req.ContentLength = 3000;
            req.Method = "POST";// WebRequestMethods.Ftp.DownloadFile;
            req.UseBinary = true;

            FileInfo fileInfo = new FileInfo(@"C:\Users\user\Desktop\TempTemp\Data.txt");

            using (var stream = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var reqStream= req.GetRequestStream();

                long length = reqStream.Length;

                while(length != 0)
                {
                    reqStream.CopyTo(stream);
                }

                reqStream.Close();
            } */
        }

        /*
         * 크게 
         * DateTime, TimeSpan 구조체를 사용가능함. 
         * DateTime 은 '시간(Time)' , TimeSpan 은 '기간(Duration)' 을 나타내기 위함인데 , 
         * 
         * 둘이 헷갈릴수 있는데 , 이런거임 .
         * DateTime 은 말 그대로 '시간' 을 의미함 . 
         * 예로 , 오늘 14:20 을 표현하고싶다 , 그러면 DateTime 으로 표현함 . 
         *
         * 근데 내가 여기서 만약 '지금 시간에서 30분을 뺀 시간은 어떻게 구할까?' 라 한다면 , 
         * 
         * 지금 '시간' 인 DateTime 에서 30분이라는 TimeSpan('기간') 을 뺀 DateTime('시간') 을 구하면 됨. 
         * */
        static void DateTime_TimeSpanTest()
        {
            PrintL(Convert.ToDateTime("05 /01/1996 17:23:29"));

            //     var timeSpan = new TimeSpan(24, 0, 0);
            //     Console.WriteLine(TimeSpan.TicksPerDay / TimeSpan.TicksPerSecond);
            //     return;

            Print("--------특정 시간 표현-------");
            Print("오늘 날짜 : " + DateTime.Today);
            Print("내일 : " + (DateTime.Today + TimeSpan.FromDays(1)));
            Print("지금 시각 : " + DateTime.Now);
            Print("지금으로 부터  1시간 전 " + DateTime.Now.Subtract(TimeSpan.FromHours(1)));
            Print("지금으로 부터  1시간 25분전 " + DateTime.Now.Subtract(TimeSpan.FromMinutes(85)));
            Print("-------------------------------");
            PadLines(2);

            // 특정 시간이 오늘의 새벽 5시를 지났는지 안지났는지 체크
            int hoursBeforeNow = 3;
            DateTime todayFiveAm = DateTime.Today.AddHours(5);

            DateTime specificTime = DateTime.Now.Subtract(TimeSpan.FromHours(hoursBeforeNow));
            Print("지금 시각은 : " + DateTime.Now + " , 테스트 기준 시간은 " + hoursBeforeNow + " 시간 전인 " + specificTime);
            Print("기준 시간 : " + specificTime + " 은 오늘의 새벽 5시 " + todayFiveAm + " 를 지났다 ? 안지났다 ? 결과는 ?");

            // 시간 체크 < > 연산 가능 
            Print(specificTime > todayFiveAm ? "지났습니다" : "안 지났습니다");

            PadLines();

            // Tick 에 관해서 
            // Tick 은 여러 분야에 따라서 의미가 달라지는데, 예로 Cpu 는 전기가 한 번 들어오는거를 틱이라고도 하고 
            // 여기서는 시간이기 때문에 시간의 최소단위 
            // 즉 , 1초보다도 훨씬 낮음 
            // 물론 소수점으로 1초 아래를 표현할수 있지만 부동소수점이던 고정소수점이던 
            // 부정확함이 있기에 , 오차없는 정수로 표현하는듯함 . 

            // 초당 틱 출력 
            // 틱은 특정 '시간' 이 아니라 , '기간' 을 나타내므로 TimeSpan 에 있음 . 

            // 또 이점은 Tick 을 사용하면 단위가 다른 시간들 즉 1일, 1시간, 1분 ,1초 등을
            // 하나의 통일된 Tick 이란 단위로 표현할수 있어서 편함 . 즉 
            // 1일은 24시간임, 즉 1일을 표현하려면 초단위로는 86400초 , 분단위로는 모르겠고 ㅋㅋ 
            // 시간단위로는 24시간이지 , 즉 서로 단위가 다르면 변환해줘야하는데 
            // 틱은 고정임 . 
            // 즉 1초의 틱을 알면 모든 변환이 틱으로만 가능함 .
            // 1시간 틱 / 1초 틱 = 1시간은 몇초인지 , 가 계산이 되는거 
            // 시간으로 하면 나라가 달라지거나 단위가 달라지거나 환경이 달라지면 오류가 날수있음. 하지만 고정된 
            // 틱이라는 단위로 계산하면 오류를 범할수 없음 . 

            // 1초 Tick 출력
            Console.WriteLine("1초 Ticks : " + TimeSpan.TicksPerSecond);
            // 1시간 Tick 출력
            Console.WriteLine("1시간 Ticks : " + TimeSpan.TicksPerHour);
            // 1시간 30분 Tick 출력 
            Console.WriteLine("1시간 30분 Ticks : " + new TimeSpan(hours: 1, minutes: 30, seconds: 0).Ticks);
            // 하루는 몇초인가 ?  
            Console.WriteLine("1일은 몇초인가 ? : " + TimeSpan.TicksPerDay / TimeSpan.TicksPerSecond);
            // 1시간은 몇초인가 ? 
            Console.WriteLine("1시간은 몇초인가 ? " + TimeSpan.TicksPerHour / TimeSpan.TicksPerSecond);
        }

        /*
         * DisplayClass (익명 클래스를 이렇게 부름) 의 Variable Capture (Closure Capture 라고도 함)
         * 를 테스트함 . 
         * LambdaVariableCaptureClass 클래스 내부를 보면은 자세한 설명이 있음 . 
         * 간단하게 , 익명함수로 각각 다른 값을 넘겼는데,  최종적으로 똑같은 하나의 값만 기억되어
         * 파라미터로 넘어오는 현상 . 왜 그런지 . 테스트 . 및 이해 . 
         * */
        static int captureTestData = 0;
        static void LambdaVariableCaptureTest()
        {
            /// Test 01 : 조금 뻔함 . 
            Action action = () =>
            {
                Print(captureTestData);
            };

            action();
            captureTestData++;
            action();
            action();

            LambdaVariableCaptureClass testClass = new LambdaVariableCaptureClass();

            testClass.Call(4);
            testClass.Call(2);
        }

        /// <summary>
        ///  배치파일 조작 테스트 
        /// </summary>
        static void BatchFileUsageTest()
        {
            #region SVN Revision 정보 가져오기 
            /// 여기에 svn 프로젝트 폴더의 path 를 넣으셈
            string svnWorkingCopyPath = @"WorkingCopyPath"; /// 이카루스 기준 C:\Projects\SVN\IE_Sub 였음 . 210504 오호..

            ProcessStartInfo info2 = new ProcessStartInfo("svn", $@"info {svnWorkingCopyPath}");
            info2.WindowStyle = ProcessWindowStyle.Hidden;
            info2.RedirectStandardOutput = true;
            info2.RedirectStandardError = true;
            info2.UseShellExecute = false;

            var pc2 = Process.Start(info2);

            var resultStr2 = pc2.StandardOutput.ReadToEnd();
            var error2 = pc2.StandardError.ReadToEnd();
            bool workingCopyExist = string.IsNullOrEmpty(error2);

            /// error 문이 존재 
            /// => 해당 working copy 없음 
            if (workingCopyExist == false)
            {
                Print(error2);
                Print($"타겟 path : {svnWorkingCopyPath}");
            }
            /// 해당 path 가 workin copy 라면 
            /// 정상적으로 출력 
            else
            {
                #region SVN 의 Revision 확인하기 
                ProcessStartInfo info = new ProcessStartInfo("svn", $@"info {svnWorkingCopyPath}");
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.UseShellExecute = false;

                var pc = Process.Start(info);

                var resultStr = pc.StandardOutput.ReadToEnd();
                var error = pc.StandardError.ReadToEnd();

                if (string.IsNullOrEmpty(error) == false)
                {
                    Print(error);
                }
                else
                {
                    Print(resultStr);
                }
                #endregion
            }
            #endregion

            Print("=============================================================");

            #region 일정 시간후에 자동으로 종료하기 (Shutdown)
            //System.Diagnostics.Process.Start("mspaint.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "shutdown.exe";
            startInfo.Arguments = "-s -t 9999"; /// 자동종료 예약한거 취소하려면 argument 를 -a 로 설정 ㄱㄱ (abort)

            #region ADB 연결중인 기기 모드 Disconnect 하기 
            //  @"C:\Users\Jayce\AppData\Local\Android\Sdk\platform-tools/adb.exe";

            /// 아래 코드는 ADB 에 연결중인 기기들을 끊음 
            // startInfo.FileName = @"C:\Users\Jayce\AppData\Local\Android\Sdk\platform-tools/adb.exe";
            // startInfo.Arguments = "disconnect";
            #endregion

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    /// 해당 실행파일이 끝날때까지 현재 이 프로세스는 Block 상태 . 즉 대기하게됨.
                    /// 즉 동기함수 .
                    exeProcess.WaitForExit();

                    var code = exeProcess.ExitCode;

                    if (code == 0)
                    {
                        Console.WriteLine("Successfully exit!");
                    }
                    else
                    {
                        Console.WriteLine("Error Occur ! . Code : " + code);
                    }
                }
            }
            catch
            {
            }
            #endregion
        }

        #region Immutable 데이터 타입 만들기 테스트 
        /// <summary>
        /// https://exceptionnotfound.net/csharp-in-simple-terms-8-structs-and-enums/#:~:text=Structs%20and%20enums%20are%20both,constructors%2C%20methods%2C%20and%20properties.&text=We%20can%20use%20any%20integer,and%20int%20is%20the%20default.
        /// Microsoft 에서 권장하는 구조체 (struct) 의 사용 법중 하나가, 구조체를 immutable 하게 구현하라는 거임. 
        /// 하나의 Immutable 구조체는 값을 해당 변수를 '생성' 할때만 오직 초기화 할 수 있는 구조체임. 
        /// 구조체를 immutable 하게 만들수 있는 방법은 , <see cref="readonly"/> 타입을 사용하는 거임.  
        /// </summary>
        public readonly struct ReadOnlyStruct
        {
            public ReadOnlyStruct(string name, int age)
            {
                Name = name;
                Age = age;
            }

            public string Name { get; }
            public int Age { get; }
            /// readonly 구조체인데 , set property 가 존재함 => syntax error 발생 
            /// public int Age_ErrorVersion { get; set; }

            public void Print()
            {
                Console.WriteLine($"Name : {Name}, Age : {Age}");
            }
        }

        static void ReadOnlyStructTest()
        {
            ReadOnlyStruct person = new ReadOnlyStruct("제이스", 29);
            person.Print();

            /// 당연히 syntax error 발생 
            /// person.Age = "15";
        }

        #endregion

        #region char 문자 타입의 Unicode Category 검사 및 다양한 활용 테스트 
        /// <summary>
        /// 표 참고 https://unicode.org/charts/PDF/UAC00.pdf
        /// </summary>
        static void UnicodeCharacterAdvanceTest()
        {
            string str = "ㅋ012 !@ abc DEF ㄱㄷㄴ 이것은 한글 데스네 ㅗㅜ . \n 호호 HaHa";

            Console.WriteLine($"테스트 문자열 : {str}");

            PadLines();

            foreach (var c in str)
            {
                var codeCtg = char.GetUnicodeCategory(c);

                Console.WriteLine("----------");

                Console.WriteLine($"문자 : {c} , unicode : {(int)c} , unicodeCategory : {codeCtg}");
                switch (codeCtg)
                {
                    case System.Globalization.UnicodeCategory.LowercaseLetter:
                        Console.WriteLine($"UnicodeCateogry 판정 : 소문자 | {codeCtg}");
                        break;
                    case System.Globalization.UnicodeCategory.UppercaseLetter:
                        Console.WriteLine($"UnicodeCateogry 판정 : 대문자 | {codeCtg}");
                        break;

                    case System.Globalization.UnicodeCategory.SpaceSeparator:
                        Console.WriteLine($"UnicodeCateogry 판정 : 스페이스 | {codeCtg}");
                        break;
                    case System.Globalization.UnicodeCategory.OtherLetter:
                        Console.WriteLine($"UnicodeCateogry 판정 : 기타 , 속하지 않는 문자 (한글, 한문 etc..) | {codeCtg}");
                        break;
                    default:
                        Console.WriteLine($"UnicodeCateogry 판정 : 기타 | {codeCtg}");
                        break;
                }

                /// 한국어인지 체크 
                if (IsKorean(c))
                {
                    Console.WriteLine("한국어 O");

                    /// 한국어면은 완성 글자인지 체크 
                    if (IsCompletedKoreanLetter(c))
                    {
                        Print("완성형");
                    }
                    else
                    {
                        Print("미완성형");
                    }
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// 한국어인지 체크 
        /// </summary>
        static bool IsKorean(char c)
        {
            /// 해당 character 를 정수 코드로 변환. 
            /// (유니코드)
            var value = (int)c;

            /// 한글의 처음 유니코드 정수값 (표참고)
            var korean_ucd_from = (int)'ㄱ';
            /// 한글의 끝 유니코드 정수값 (표참고)
            var korean_ucd_to = (int)'힣';

            return value >= korean_ucd_from && value <= korean_ucd_to;
        }

        /// <summary>
        /// 완성형 한글인지 체크 
        /// e.g
        ///     가 -> 완성형 
        ///     나 -> 완성형 
        ///     ㄷ -> 미완성형
        ///     ㄹ -> 미완성형 
        ///     뻑 -> 완성형
        ///     ㅏ -> 미완성형
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static bool IsCompletedKoreanLetter(char c)
        {
            /// 해당 character 를 정수 코드로 변환. 
            /// (유니코드)
            var value = (int)c;

            /// 한글의 글자 처음 유니코드 정수값 (표참고)
            var from = (int)'가';
            /// 한글의 끝 유니코드 정수값 (표참고)
            var to = (int)'힣';

            return value >= from && value <= to;
        }
        #endregion

        #region Async & Await 테스트 
        public enum AsyncTestCase
        {
            None = 0,
            AsyncVoidEventHandler,
            LongCalculation, /// 길고 긴~~ 연산하기 . 연산 하는동안 UI Thread 가 멈춰서는 안됨 . 
            AsyncTest_GameLogic, /// 보통 게임의 로직을 구현 
            AsyncTest_Loading /// 길고 긴 로딩 구현 
        }

        public enum AsyncTestGameState
        {
            None = 0,
            DownloadingAsset,
            CharacterSelect,
            InGame,
            CloseGame
        }

        private static AsyncTestGameState asyncTest_GameState;

        static void AsyncTest(AsyncTestCase testCase)
        {
            if (testCase == AsyncTestCase.AsyncVoidEventHandler)
            {
                AsyncTest_AsyncVoidEventHandler();
            }
            else if (testCase == AsyncTestCase.LongCalculation)
            {
                AsyncTest_LongCalc();
            }
            else if (testCase == AsyncTestCase.AsyncTest_GameLogic)
            {
                AsyncTest_GameLogic();
            }
            else if (testCase == AsyncTestCase.AsyncTest_Loading)
            {
                AsyncTest_LoadAsset();

                /// 메인 쓰레드 강제로 블로킹 걸자~
                /// (UI Thread 라고도 불리지?)
                while (asyncTest_loadAssetDone == false)
                {
                    /// 대기중 ..
                    /// 로딩동안 메인 로직이 도는거를 시뮬레이션 . . . 
                    /// 아래 로직에서 메인 로직이 돌겠지 ? 
                }
            }
        }

        #region Async 이면서 Void 의 Return Type 을 가지는 메서드를 Event Handler 로 쓰기 
        private static void AsyncTest_AsyncVoidEventHandler()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            float s = 0;
            float t = 0f;
            Action handler = AsyncTest_AsyncVoidReturnTypeMethod;

            int n = 0;

            /// 메인 루프 while
            while (true)
            {
                /// 2 초에 한번씩 
                if (watch.Elapsed.Seconds - s > 2)
                {
                    s += 2f;
                    Console.WriteLine("Main Logic 2 Seconds passed !");

                    n++;
                    if (n == 2)
                    {
                        Console.WriteLine("CALL!");

                        /// 비동기 이벤트 핸들러 호출~!
                        handler.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// async void 타입의 이벤트 핸들러용 메서드.
        /// </summary>
        public async static void AsyncTest_AsyncVoidReturnTypeMethod()
        {
            /// 현재 메서드는 async 즉 비동기 가능 함수이기에 
            /// await 사용 가능 . 
            /// await 는 Task,Task<T> 객체들을 반환해주는 쪽에서 
            /// 작업을 마칠때까지 비동기로 기다릴수있음 . 
            /// 즉 기다리는 동안 UI 쓰레드는 여전히 계속 실행되는 상태 .
            /// 그래서 아래 Task.Delay() 는 파라미터로 지정한 시간만큼 
            /// 대기한후에 작업을 끝내줌 . 
            await Task.Delay(TimeSpan.FromSeconds(1));
            Print("One sec passed!");

            await Task.Delay(TimeSpan.FromSeconds(2));
            Print("Two sec passed!");

            await Task.Delay(TimeSpan.FromSeconds(3));
            Print("Three sec passed!");
        }

        #endregion

        #region Async Test - 아주 ~~ 긴 ~~ 연산 
        private static bool isAsyncTest_longCalcAllDone;

        private static void AsyncTest_LongCalc()
        {
            AsyncTest_PrintTotalLongCalcResult();

            while (true)
            {
                /// Front End task ... 
                /// (UI Thread task ....)
                /// 여기서는 게임의 뭐 메인 로직 ? 돌거임 ㅇㅇ 
            }
        }

        /// <summary>
        /// 길고 긴 연산을 통해 얻을수 있는 값을 출력하는데
        /// 연산이 아주 길기때문에 async 로 비동기로 BackGround 즉
        /// Worker Thread 를 이용해서 실행함. 
        /// </summary>
        private static async void AsyncTest_PrintTotalLongCalcResult()
        {
            Print("Result01 계산전 밀리세컨드 시간 : " + DateTime.Now.Millisecond);

            var result01 = await AsyncTest_DoLongCalc(1000000);

            Print("Result01 계산후 밀리세컨드 시간 : " + DateTime.Now.Millisecond);

            PadLines();

            Print("Result02 계산전 밀리세컨드 시간 : " + DateTime.Now.Millisecond);

            var result02 = await AsyncTest_DoLongCalc(50000000);

            Print("Result02 계산후 밀리세컨드 시간 : " + DateTime.Now.Millisecond);

            Print("합산 : " + (result01 + result02).ToString("n0"));

            isAsyncTest_longCalcAllDone = true;
        }

        private static async Task<ulong> AsyncTest_DoLongCalc(ulong loopCnt)
        {
            Print("반복 숫자 : " + loopCnt);

            return await Task.Run(() =>
            {
                ulong result = 0;

                for (ulong i = 0; i < loopCnt; i++)
                {
                    result += i;
                }

                return result;
            });
        }

        #endregion

        #region Async Test - 게임 로직편

        /// <summary>
        /// C# 의 Async/Await 키워드 테스트 
        /// </summary>
        static void AsyncTest_GameLogic()
        {
            AsyncTest_SetGameState(AsyncTestGameState.DownloadingAsset);

            while (asyncTest_GameState != AsyncTestGameState.CloseGame)
            {
                /// 메인 게임 로직 . 현재 비동기로 돌고있는 
                /// <see cref="AsyncTest_SetGameState"/> 와는 별개로 돌음. 
            }

            PadLines(1);
            Print("- 게임 종료 -");
        }

        private static void AsyncTest_SetGameState(AsyncTestGameState state)
        {
            Print($"GameState : {state}");
            asyncTest_GameState = state;

            switch (state)
            {
                case AsyncTestGameState.DownloadingAsset:
                    {
                        TestAsync_StartDownload((timeTaken) =>
                        {
                            Print($"총 에셋 다운로드 시간 : {timeTaken}");
                            AsyncTest_SetGameState(AsyncTestGameState.CharacterSelect);
                        });
                    }
                    break;
                case AsyncTestGameState.CharacterSelect:
                    {
                        AsyncTest_SetGameState(AsyncTestGameState.InGame);
                    }
                    break;
                case AsyncTestGameState.InGame:
                    {
                        AsyncTest_SetGameState(AsyncTestGameState.CloseGame);
                    }
                    break;
                case AsyncTestGameState.CloseGame:
                    {

                    }
                    break;
            }
        }

        /// <summary>
        /// Async 다운로드 시뮬레이션 
        /// 
        /// ** Task 는 UI Thread 가 아닌 Worker Thread (BackGround Thread) 
        /// 에서 실행이 됨. ** 
        /// </summary>
        static async void TestAsync_StartDownload(Action<double> onDownloadDone)
        {
            double totalTimeTaken = 0d;

            #region ---------------------------- Case 01 -------------------------------------------------
            var task01 = Task.Run(() =>
            {
                /// 다운로드 총 걸린 시간 체크용 
                double timeTaken = 0;

                for (int i = 0; i < 10; i++)
                {
                    Print($"File01 다운로드 진행률 : {i} / 10");

                    /// 
                    Thread.Sleep(TimeSpan.FromSeconds(0.3));
                    timeTaken += 0.3;
                }

                return timeTaken;
            });

            var time = await task01;

            Print($"File 01 다운로드 끝 ! , 걸린 시간 : {time}");

            totalTimeTaken += time;
            #endregion

            #region ------------------------ Case 02 -----------------------------------------

            double timeTaken_down02 = 0;

            var task02 = new Task(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Print($"File 02 : 다운로드 진행률 {i} / 10");
                    Thread.Sleep(TimeSpan.FromSeconds(0.3));
                    timeTaken_down02 += 0.3;
                }
            });

            task02.RunSynchronously();

            Print($"File 02 다운로드 끝 ! , 걸린 시간 : {timeTaken_down02}");
            totalTimeTaken += timeTaken_down02;
            #endregion

            #region --------------------- Case 03 --------------------------------------------

            double timeTaken_file03 = 0d;

            var taskIdCheck01 = Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Print($"File 03 : 다운로드 진행률 {i} / 5");
                    Thread.Sleep(TimeSpan.FromSeconds(0.2));
                    timeTaken_file03 += 0.2;
                }

                Print($"File 03 다운로드 끝 ! , 걸린 시간 : {timeTaken_file03}");
                totalTimeTaken += timeTaken_file03;
            })
            /// 위 Task 가 끝나게 되면 밑에 ContinueWith 에 람다문이 실행된다. 
            .ContinueWith((task_) =>
            {
                /// 파라미터 task_ 와 taskIdCheck01 의 id 가 같음 . 
                /// 왜냐면은 현재 이 task 는 taskIdCheck01 의 Task 의 비동기 작업이
                /// 끝난 후에 실행되는 task 이기 때문. 
                Print($"이전 Task 의 ID : {task_.Id}");
                onDownloadDone(totalTimeTaken);
            });

            Print($"Task 의 ID : {taskIdCheck01.Id}");
            #endregion
        }

        #endregion

        #region Async Test - 로딩 
        /// <summary>
        /// Async 로 로딩 시뮬레이션 
        /// AsyncTest_LoadAsset 의 Return Type 에 Task 가 아닌 void 왜냐면
        /// 이 함수를 Calling 하는 측에서 기다릴 필요없으니( await 키워드를 쓸 필요가 없으니 )
        /// 그리고 void 타입은 Event Handler 로도 사용 가능 ,
        /// 즉 Action a = <see cref="AsyncTest_LoadAsset"/> 이 가능 . 
        /// 하지만 Return Type 이 Task 라면
        /// Action<Task> a; 는 불가. 
        /// </summary>
        static bool asyncTest_loadAssetDone;

        static async void AsyncTest_LoadAsset()
        {
            List<string> loadedAssetNames = new List<string>();

            /// await <see cref="AsyncTest_Load"/> 즉 
            /// 해당 함수를 호출하며 Block 시킴 . 
            /// 끝나는 시점은 해당 Task 가 끝나는 시점임 . 
            /// ++ <see cref="AsyncTest_Load"/> 함수의 Return type 이
            /// Task<double> 이기에 값을 저장 가능 . 
            /// 만약 Return Type 이 async void 였다면 
            /// await AsyncTest_LoadAsset(loadedAssetNames);
            var timeTaken = await AsyncTest_Load(loadedAssetNames);

            /// 위에 await 키워드로 함수가 끝난 후에 아래 코드 실행
            PadLines();

            Print($"-- 로딩 완료 목록 (걸린시간 {timeTaken}) --");

            PadLines();

            for (int i = 0; i < loadedAssetNames.Count; i++)
            {
                Print(loadedAssetNames[i]);
            }

            asyncTest_loadAssetDone = true;
        }

        /// <summary>
        /// return type 이 async Task 인 이유는, 이 함수를 호출하는 측에서 (Caller)
        /// await 로 <대기> 가 가능해야하기 때문.
        /// 
        /// </summary>
        static async Task<double> AsyncTest_Load(List<string> completedAssetNameList)
        {
            double timeTaken = 0;

            for (int i = 0; i < 10; i++)
            {
                /// await 키워드로 일정 시간이 지난후에 끝나는 Task 생성 및 대기 . 
                await Task.Delay(TimeSpan.FromSeconds(0.4));
                timeTaken += 0.4;

                string loadedAssetName = $"Hero{i}";
                completedAssetNameList.Add(loadedAssetName);
                Print(loadedAssetName + "이 로딩됨");
            }

            return timeTaken;
        }

        #endregion

        #endregion

        #region Linq 테스트 
        public class LinqTest_Student
        {
            public string name;
            public int score;
            public int age;

            public LinqTest_Student(string name, int score, int age)
            {
                this.name = name;
                this.score = score;
                this.age = age;
            }

            public void Print()
            {
                Program.Print($"이름 : {name} , 점수 : {score} , 나이 : {age}");
            }
        }

        /*
         * Linq 는 편하지만 할당이 많이 일어나 GC 발생률을 증가시키는 함수들이 많음.
         * 고로 런타임 Update 로직에 매 프레임에 무거운 작업을 하게되는것은 최대한 피하면서 
         * 성능을 조금 포기하고 가독성에 투자하겠다하는 경우에 적합함. 또는 에디터 코드나 . 
         * 각각 함수마다 GC 체크는 추가적으로 프로파일링으로 테스트 ㄱ ㄱ 
         * (예로 Min,Max 함수같은 경우 GC 겁나 처먹음)
         * 
         * 자주쓰이는애들 : Min,Max,Any,First,Single,
         */
        static void LinqUsage()
        {
            #region Query (orderby, where .. etc)
            PrintL("Query 키워드 테스트");

            /// 통과 기준 점수 
            int passThreshold = 60;
            int[] scores = new int[] { 100, 50, 20, 10, 80, 150, 185, 75, 38 };

            /// 이름,점수,나이 다름 
            LinqTest_Student[] students_all_diff = new LinqTest_Student[]
            {
                new LinqTest_Student("김다라", 20, 15)
                 , new LinqTest_Student("김세진", 80, 18)
                 , new LinqTest_Student("이국진", 50,14)
                 , new LinqTest_Student("창세기", 95,18)
                 , new LinqTest_Student("김호미", 40,19)
                 , new LinqTest_Student("김계란", 18,16)
                 , new LinqTest_Student("김국진", 95, 20)
                 , new LinqTest_Student("이창렬", 70, 25)
            };

            /// 이름 동일 / 점수,나이 다름 
            LinqTest_Student[] students_name_same = new LinqTest_Student[]
            {
                new LinqTest_Student("이름", 20, 15)
                 , new LinqTest_Student("이름", 80, 18)
                 , new LinqTest_Student("이름", 50,14)
                 , new LinqTest_Student("이름", 95,18)
                 , new LinqTest_Student("이름", 40,19)
                 , new LinqTest_Student("이름", 18,16)
                 , new LinqTest_Student("이름", 95, 20)
                 , new LinqTest_Student("이름", 70, 25)
            };

            /// 이름, 점수 동일 / 나이 다름
            LinqTest_Student[] students_diff_age = new LinqTest_Student[]
            {
                new LinqTest_Student("이름", 100, 15)
                 , new LinqTest_Student("이름", 100, 18)
                 , new LinqTest_Student("이름", 100,14)
                 , new LinqTest_Student("이름", 100,18)
                 , new LinqTest_Student("이름", 100,19)
                 , new LinqTest_Student("이름", 100,16)
                 , new LinqTest_Student("이름", 100, 20)
                 , new LinqTest_Student("이름", 100, 25)
            };

            {
                Print($"통과 기준 점수(score) : {passThreshold} 이상 스코어만 추리기 테스트 ");

                IEnumerable<int> passedScores =
                    from score in scores
                    where score >= passThreshold
                    select score;

                foreach (var s in passedScores)
                {
                    Print($"통과한 점수(score) : {s}");
                }
            }

            PadLines();

            {
                int rangeMin = 30;
                int rangeMax = 60;

                Print($"{rangeMin} 이상이면서 {rangeMax} 이하인 Score 들 추리기 테스트");

                IEnumerable<int> scoresInRange =
                    from score in scores
                    where score >= rangeMin && score <= rangeMax
                    select score;

                foreach (var s in scoresInRange)
                {
                    Print($"Score : {s}");
                }
            }

            PadLines();

            {
                Print("학생 이름으로 오름차순 정렬 테스트 (가나다라 ...)");

                IEnumerable<LinqTest_Student> orderByName =
                    from student in students_all_diff
                    orderby student.name ascending /// ++ ascending 는 스킵가능. 디폴트 오름차순 . 
                    select student;

                foreach (var s in orderByName)
                {
                    s.Print();
                }
            }

            PadLines();

            {
                Print("학생 이름으로 오름차순 정렬 테스트 (가나다라 ...)");

                IEnumerable<LinqTest_Student> orderByName =
                    from student in students_all_diff
                    orderby student.name ascending /// ++ ascending 는 스킵가능. 디폴트 오름차순 . 
                    select student;

                foreach (var s in orderByName)
                {
                    s.Print();
                }
            }

            PadLines();

            {
                Print("학생 이름으로 내림차순 정렬 테스트 (가나다라 ...)");

                IEnumerable<LinqTest_Student> orderByName =
                    from student in students_all_diff
                    orderby student.name descending
                    select student;

                foreach (var s in orderByName)
                {
                    s.Print();
                }
            }

            PadLines();

            {
                Print("학생 이름 오름차순 정렬 , 이름이 완전히 동일하다면 점수 내림차순 정렬");

                IEnumerable<LinqTest_Student> orderByName =
                    from student in students_name_same /// 이름이 동일해야 score 정렬 작동.
                    orderby student.name ascending, student.score descending
                    select student;

                foreach (var s in orderByName)
                {
                    s.Print();
                }
            }

            PadLines();

            {
                Print("학생 이름 오름차순 , 점수 내림차순 , 나이 내림차순 정렬 . (뭔소리냐면, 학생 이름이 같고 점수가 같다면 나이 내림차순 적용)");

                IEnumerable<LinqTest_Student> orderByName =
                    from student in students_diff_age /// 이름/점수가 동일해야 , 마지막 정렬 우선순위 '나이' 가 적용됨
                    orderby student.name ascending, student.score descending, student.age descending
                    select student;

                foreach (var s in orderByName)
                {
                    s.Print();
                }
            }

            PadLines();

            {
                Print("학생 이름 오름차순 , 점수 내림차순 , 나이 내림차순 정렬 . (뭔소리냐면, 학생 이름이 같고 점수가 같다면 나이 내림차순 적용)");

                IEnumerable<LinqTest_Student> orderByName =
                    from student in students_diff_age /// 이름/점수가 동일해야 , 마지막 정렬 우선순위 '나이' 가 적용됨
                    orderby student.name ascending, student.score descending, student.age descending
                    select student;

                foreach (var s in orderByName)
                {
                    s.Print();
                }
            }

            #endregion

            PadLines(printSeparator: true);

            #region Repeat
            {
                PrintL("TEST Repeat");

                // new LinqTestClass01() { n =10 } 를 3 번 할당하여 IEnumerable 로 뱉음. 
                // 즉 LinqTesClass01 로 순회를 돌게됨. 
                // ** 주의할건 저 n = 에다가 Random() 과도 같은 걸 넣어서 3 개의 인스턴스의
                // n 에 Random 값을 넣겠다, 하는건 안됨.  
                // 내부적으로 인스턴스 하나만 생성후 애를 = 연산자로 넣어주는듯. 즉 참조형일때는 인스턴스 하나만 가리키게됨.  
                // 무튼 대가리에 이건 안된다는걸 50%만 input 하여도 나중에 실수할 확률이 줄을것 . 
                foreach (var item in Enumerable.Repeat(
                    element: new LinqTestClass01() { n = 10 },
                    count: 3))
                    PrintL(item.n);
            }
            #endregion

            PadLines();


            #region Range
            {
                PrintL("Test Range");

                // Range 는 정수 시퀀스 생성하는데 , start 숫자부터 + count 까지 . 즉 
                // start 가 5 고 count 가 10 이면 5 로부터 + 10 . 즉 5 부터 15 까지의 정수 시퀀스 생성.
                foreach (var item in Enumerable.Range(
                    start: 5,
                    count: 10))
                {
                    PrintL(item);
                }

                // 약간의 응용 . 알파벳 출력 
                // 아스키코드값 'a' 로부터 시작해서 , 알파벳의 끝 'z' 에서 'a' 를 빼서 
                // 전체 알파벳 순회 시퀀스 생성하여 순회 . 출력 . 끝 
                foreach (char item in Enumerable.Range('a', 'z' - 'a'))
                    PrintL(item);
            }
            #endregion

            PadLines();

            #region Any
            {
                PrintL("Test Any");

                List<int> anyTestList = new List<int>();
                // Any 는 element 가 하나라도 있으면 True . 비어있으면 false 임 . 
                // 즉 Count > 0 같이 줒같은 코드 안써도되게끔 해쥼 개굳 ? 
                PrintL(anyTestList.Any());
                anyTestList.Add(1);
                PrintL(anyTestList.Any());
            }
            #endregion

            PadLines();

            #region Count
            {
                PrintL("Test Count");

                List<int> countTestList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

                PrintL(countTestList.Count((num) =>
                {
                    // 클래스나 구조체면 .n 으로 접근하면 되겠지? 줜나 당연한거지만 
                    // 무튼 이 코드는 순회하면서 5 보다 작은애의 개수를 가져오는거 
                    return num < 5;
                }));
            }
            #endregion

            PadLines();

            #region First
            {
                List<int> list = new List<int>() { 1, 2, 3, 4 };
                // 처음 요소 뱉어주는 First 함수 .
                PrintL(list.First());
                // empty 면 exception 
                list.Clear();
                try
                {
                    PrintL(list.First());
                }
                catch (Exception exp)
                {
                    PrintL("Exception ! : " + exp.Message);
                }
            }
            #endregion

            PadLines();

            #region Single
            {
                List<int> list = new List<int>() { 1, 2, 3, 4 };

                // Single 함수도 First 와 같이 가장 앞에것을 뱉어주는데
                // 요소가 무조건 하나여야만함 .  즉 하나만 있어야한다는걸 코드로 보여주는거 
                // 0 개 또는 하나가 넘어가게되면 
                // Exception 발생 
                try
                {
                    PrintL(list.Single());
                }
                catch (Exception exp)
                {
                    PrintL("Exception ( 요소가 하나보다 많음 ) : " + exp.Message);
                }

                // 하나는 잘 출력됨  
                list.Clear();
                list.Add(1);

                PrintL(list.Single());
            }
            #endregion

            PadLines();
        }

        #endregion

        // Fix 
        static void TestForeachImplement()
        {

        }

        // 숫자로 ToString 할때 여러가지 표현 방식 테스트 .
        // https://docs.microsoft.com/ko-kr/dotnet/standard/base-types/standard-numeric-format-strings 참고 
        // 참고로 string.format("{0:N0}", n); 처럼 format 함수에도 적용 가능 . 
        static void NumberToStringUsage()
        {
            float tf = 1234.56f;

            // preview 테스트 시작 
            /*
             * n 은 자리수에 맞게 , 를 넣어주는거 . 뒤에 숫자를 넣으면 소수점 아래로 채워짐 . 
             * f 은 소수점 아래로 채울 갯수 
             * 000.00 에서 왼쪽 000 의 의미는 현재 숫자를 전부 표시하되 (전부 표시하는거는 0이 1개던 3 개던 관계 x , 걍 다표시함 디폴트임) 3 자리보다 낮다면 그 부분은 0 으로 채워라임.
             *          오른쪽 00 는 소수점을 표시하되 2자리까지만 , 없는 부분은 0 으로 들어가게됨. 
             * ###.## 은 000.00 이랑 비슷하지만 현재 ToString() 되는 숫자가 해당 자리수보다 작아서 존재하지않으면 0 자체를 채워넣지 않음 . 
             *         예로 1.1f 인데 ###.### 이다 ? 그럼 1.1f 만 뜸 . 근데 000.000 이면 001.100 이 뜨겠지 ㅇㅋ ? 
             * */

            // 1,234.56 => n 이기떔에 자리수에 맞게 , 를 넣으면서 뒤에 숫자가 없으므로 기본 2 자리수까지 출력. 중요한건 그 오른쪽 숫자로부터 반올림? 될거임 . 
            PrintL(tf.ToString("n"));
            // 1,234.5600 => 4 가 붙어서 소수점 4개까지 표시 
            PrintL(tf.ToString("n4"));
            // 1,234
            PrintL(tf.ToString("n0"));
            // 1234
            PrintL(tf.ToString("f0"));
            // 1234.560
            PrintL(tf.ToString("f3"));
            // 1234.6 
            PrintL(tf.ToString("0.0"));
            // 1234.56
            PrintL(tf.ToString("0.00"));
            // 01234.560
            PrintL(tf.ToString("00000.000"));
            // 1234.6
            PrintL(tf.ToString("#.#"));
            // 1234.56 
            PrintL(tf.ToString("#.###"));
            // preview 끝 

            Console.WriteLine();

            int n = int.MaxValue;

            PrintL(n.ToString("n"));
            PrintL(n.ToString("n0"));
            PrintL(n.ToString("n1"));
            PrintL(n.ToString("n2"));
            PrintL(n.ToString("n3"));
            PrintL(n.ToString("f"));
            PrintL(n.ToString("f3"));
            PrintL(n.ToString("0.000"));

            Console.WriteLine();

            float f = 1234.56f;

            PrintL(f.ToString("n"));
            PrintL(f.ToString("n0"));
            PrintL(f.ToString("n1"));
            PrintL(f.ToString("n2"));

            PrintL(f.ToString("f0"));
            PrintL(f.ToString("f1"));
            PrintL(f.ToString("f2"));
            PrintL(f.ToString("f0"));

            PrintL(f.ToString("0.0"));
            PrintL(f.ToString("000.0000"));
            PrintL(f.ToString("0000000000"));
            PrintL(f.ToString("0,0"));

            PrintL(f.ToString("##.####"));
            PrintL(f.ToString("#.#"));
            PrintL(f.ToString("#.0"));
            PrintL(f.ToString("#.##"));
            PrintL(f.ToString("#.00"));
            PrintL(f.ToString("#.###"));
            PrintL(f.ToString("#.000"));
            PrintL(f.ToString("#.#0#"));
            PrintL(f.ToString("#.0#0"));
            PrintL(f.ToString("#.#0#0#"));
            PrintL(f.ToString("#.0#0#0"));

            Console.WriteLine();

            float f02 = 0.12f;

            PrintL(f02.ToString("#.0"));
            PrintL(f02.ToString("#.##"));
            PrintL(f02.ToString("0.#"));
            PrintL(f02.ToString("#0.#0"));
            PrintL(f02.ToString("0#0.#0"));
            PrintL(f02.ToString("00#0.#####"));

            Console.WriteLine();

            int n02 = 1234;

            PrintL(n02.ToString("##.##"));
            PrintL(n02.ToString("00.00"));
            PrintL(n02.ToString("000000.000"));
        }

        static void FileInfoTask()
        {
            string path = @"C:\Users\user\Desktop\TempTemp\jayce.key"; //  Path.GetTempFileName();
            FileInfo fileInfo = new FileInfo(path);

            Console.WriteLine("Exist : " + fileInfo.Exists);

            if (fileInfo.Exists)
            {
                Console.WriteLine("Extension : " + fileInfo.Extension);
                Console.WriteLine("Directory : " + fileInfo.DirectoryName);

                Console.WriteLine("FileName : " + fileInfo.Name);
                Console.WriteLine("FullName : " + fileInfo.FullName);
                Console.WriteLine("Is ReadyOnly : " + fileInfo.IsReadOnly);
                Console.WriteLine("CreateionTime : " + fileInfo.CreationTime);
                Console.WriteLine("CreationTimeUTC : " + fileInfo.CreationTimeUtc);
                Console.WriteLine("LastAccessTime : " + fileInfo.LastAccessTime);
                Console.WriteLine("LastAccessTimeUTC : " + fileInfo.LastAccessTimeUtc);
                Console.WriteLine("LastWriteTime : " + fileInfo.LastWriteTime);
                Console.WriteLine("LastWriteTimeUTC : " + fileInfo.LastWriteTimeUtc);
                Console.WriteLine("Length(Byte) : " + fileInfo.Length);
                Console.WriteLine("Attributes : " + fileInfo.Attributes);
            }

            //     fileInfo.Create();

            //fileInfo.Encrypt();
        }

        static void CharUnicodeTask()
        {
            string str = "ASDasd";
            string strNum = "584";

            Console.WriteLine("Read String by one");

            foreach (var c in str)
            {
                Console.WriteLine(c);
            }

            Console.WriteLine("convert character(unicode) to int");

            foreach (var c in str)
            {
                Console.WriteLine(Convert.ToInt32(c));
            }

            Console.WriteLine("convert num char to int");

            // ToInt32 에 char 를 넣어주면 해당 char 의 유니코드값 ,즉 
            // 숫자 '5' 는 유니코드표에서 53 에 해당함 . 그래서 실제 숫자로 변환하려면 
            // 그냥 c - 48 해주면 되는거임.
            // string 으로 넣어주면 그냥 그 문자 자체 "5" 를 숫자 5 로 변환하는거고 .
            foreach (var c in strNum)
            {
                Console.WriteLine(Convert.ToInt32(c.ToString()));
                Console.WriteLine(c - 48); // 
            }
        }

        // 클로저 테스트, il 코드로 까면 displayClass 로 불리우는 클래스로 대체됨. 
        // Fix
        static void DisplayClassTest()
        {

        }

        // 아스키코드 위주 
        // 명심할거는 컴퓨터의 모든것은 숫자다 . 
        // 참고로 char 은 c# 서 2 바이트임 . UTF-16

        // 표 https://valueelectronic.tistory.com/83 
        static void CharacterTest()
        {
            // 영어 대문자들 출력 
            {
                Print("대문자 출력");

                // 65~90 , A~Z 에 해당하는 아스키코드 
                // ABCD...
                for (int i = 65; i < 91; i++)
                    PrintW((char)i, 1);

                // Linq 버전 
                /* foreach (char item in Enumerable.Range(65, 91 - 65))
                                PrintW(item, 1); */
            }

            PadLines(2);

            // 영어 소문자들 출력
            {
                Print("소문자 출력");

                // 97~122 , a~z 에 해당하는 아스키코드
                // abcd...
                for (int i = 97; i < 123; i++)
                    PrintW((char)i, 1);
            }

            PadLines(2);

            // 영어 대문자 -> 소문자 변환 
            {
                Print("대문자 -> 소문자 출력");

                for (int i = 65; i < 91; i++)
                {
                    char c = (char)i;
                    PrintW(c + " -> " + (char)(c + 32), 1);
                    PrintW(" , ");
                }
            }

            PadLines(2);

            // 영어 소문자 -> 대문자 변환 
            {
                Print("소문자 -> 대문자 출력");

                for (int i = 97; i < 123; i++)
                {
                    char c = (char)i;
                    PrintW(c + " -> " + (char)(c - 32), 1);
                    PrintW(" , ");
                }
            }

            PadLines(2);

            {
                Print("숫자 char -> int 숫자로 출력");

                // 숫자 0~9 도 아스키코드에 있음 

                // char 숫자를 int 숫자로 변환 
                string str = "321456";
                List<char> chars = new List<char>();

                // char 형인 숫자 하나씩 ++ 
                foreach (var item in str)
                {
                    chars.Add(item);
                }

                foreach (var item in chars)
                {
                    // 숫자의 아스키코드 값 - '0' 의 아스키코드 값 
                    // 즉 '0' 의 아스키 코드값은 48 
                    // '1' 의 아스키코드 값은 49 
                    // 즉 '1' - '0' 은 1 임 . 즉 숫자로 변환되는 아주 간단한 원리. 
                    int number = (item - '0');

                    PrintW(number);
                }
            }

            PadLines(2);

            {
                // c# 에서 char 는 2 바이트임 . 즉 한글도 포함함 

                // '한' 이라는 글자의 유니코드 값은 D55C
                char A = '한';

                // 밑에 한문들도 같은 맥락으로 값들이 있음 
                char B = '寒';
                char C = '汏';

                Print(string.Format("한국어 : {0} 유니코드 ({1})", A, (int)A));
                Print(string.Format("한문 : {0} 유니코드 ({1})", B, (int)B));
                Print(string.Format("한문 : {0} 유니코드 ({1})", C, (int)C));
            }
        }

        // 정규식 테스트 
        // Fix 
        /// <summary>
        /// TODO : 
        ///     - 패턴별로 example 잘 정리해서 보여줘야함
        ///         - 중괄호 {} 로 잘 그룹화해서 패턴별로 exmaple 보여줄까 ? 
        ///     - 
        /// </summary>
        #region 패턴 (Pattern) 설명 
        /// [\d] : 숫자만을 개별로 찾음 . 1234ABCD 면 1,2,3,4 즉 개별로. ( d 를 대문자로해서 [\D] 로 하면 숫자가 '아닌' 것만 찾음. 이 룰은 일괄 적용 )
        /// [\d]+ : + 가 붙으면, '전부' 란 의미 . 숫자를 찾는데 연속으로 붙어있는 숫자라면 그 숫자를 하나로 해서 찾음 . 즉 그룹으로 . 
        ///     예로 1234ABC987 면 , 1234 , 987 가 매칭 대상이 됨.
        #endregion
        static void RegexTest()
        {
            string input = "Start,975-5121-5642,holala@gmail.com";
            string pattern = @"[\d]+";// "[0-9]+";

            Print($"Input : {input}, Pattern : {pattern}", 1);

            {
                Print("----- Regex.Match 테스트 -----");

                var r = Regex.Match(input, pattern);

                if (r.Success)
                {
                    Print("(Match Success)");

                    Print("First Match : " + r.Value);
                    Print("Length : " + r.Length);
                    Print("Index From Original Input : " + r.Index);
                }
                else
                {
                    Print("(Match Fail)");
                }

                Print("-----------------------------------");
            }

            PadLines(2);

            {
                Print($"----- Regex.Matches 테스트 -----");

                /// Match 를 순회하기 위해 Regex 의 Matches() 를 이용 
                var r = Regex.Matches(input, pattern);

                foreach (var item in r)
                {
                    Console.WriteLine("검출된 문자열 : " + item);
                }

                Print("-----------------------------------");
            }

        }

        // Fix
        static void ReflectionTest()
        {
            ReflectionTest test = new ReflectionTest();
        }

        // Fix
        static void ExcelTest()
        {
            ExcelTestClass excel = new ExcelTestClass();

            excel.Setup();
        }

        // 암호화 테스트 . 주로 객체를 바이트로 serialize 한 후 해당 바이트를 
        // 부호화한 다음(암호화) 다시 복호화하여 원래 데이터를 가져오는게 주 목적 
        static void EncryptionDecryptionTest()
        {
            EncryptionTestClass testClass = new EncryptionTestClass()
            {
                name = "MyNameIs Zian",
                age = 15531
            };
            EncryptionTestClass decryptedClass;

            byte[] sourceBytes = ProjectUtility.Serialize_BinaryFormatter(testClass);

            PrintL("원래 데이터 - " + testClass.ToString());
            PrintL("원래 데이터 바이트 크기 : " + sourceBytes.Length, 1);

            {
                ///////////// RijndaelManaged 암호화 ///////////

                // AES( Advanced Encryption Standard ) 오리지널 이름은 Rijndael 
                string aesKey = "a1D5g7Nkl8o6T2bgRF6qshmlpo87sfvs"; // 키값 , 열쇠라 생각하면됨 . 이 열쇠로 잠그고 여는거임. 
                byte[] keyArray = Encoding.UTF8.GetBytes(aesKey);

                // dispose 유의 
                using (var rDel = new RijndaelManaged())
                {
                    ///// 부호화 관련 세팅 
                    rDel.Key = keyArray;
                    rDel.Mode = CipherMode.ECB; // 암호화 모드? 즉 직접적인 알고리즘과도 연관있는듯함. 
                    rDel.Padding = PaddingMode.PKCS7; // 해당 대칭알고리즘에서 사용될 Padding. 디폴트 : PKCS7
                    ICryptoTransform cTransform = rDel.CreateEncryptor();
                    byte[] encryptedBytes = cTransform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length);

                    PrintL("RijndaelManagedKey 부호화(암호화) 바이트 크기 : " + encryptedBytes.Length);

                    ///// 복호화 관련 세팅 
                    cTransform = rDel.CreateDecryptor();
                    byte[] decryptedByte = cTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    var decryptedResult = ProjectUtility.Deserialize_BinaryFormatter(decryptedByte) as EncryptionTestClass;

                    PrintL("RijndaelManagedKey 복호화 결과 : " + decryptedResult.ToString());
                }
            }

            {
                // dispose 유의 
                using (MD5 md5Hash = MD5.Create())
                {
                    // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.md5?view=netframework-4.8 참고 
                    PrintL("Fix!!");
                }
            }
        }

        // 인코딩 테스트 
        static void EncodingTest()
        {

        }

        // 연산 속도 테스트 
        // 곱셈이 조금더 빠르다는걸 증명 . 
        // 근데 별로 차이안나네 ? 
        static void BasicMathOperationSpeedTest()
        {
            Stopwatch watch = new Stopwatch();

            // 1억번 돈다 
            int cnt = 100000000;

            watch.Start();

            float f = 1000;

            for (int i = 0; i < cnt; i++)
            {
                f *= 0.5f;
            }

            watch.Stop();

            PrintL("곱셈 걸린 시간(밀리세컨즈) : " + watch.ElapsedMilliseconds);

            watch.Reset();
            watch.Start();

            f = 1000;

            for (int i = 0; i < cnt; i++)
            {
                f /= 2;
            }

            watch.Stop();

            PrintL("나눗셈 걸린 시간(밀리세컨즈) : " + watch.ElapsedMilliseconds);
        }

        // 비트연산 테스트 
        static void BitOperationTest()
        {
            int n = 0xf;
            int t = 32;

            int move = 1 << 1;

            PrintL(n.ToString().ToBinary());
            PrintL(t.ToString().ToBinary());
            PrintL(n & t);

            int n2 = 0x100;
            int t2 = 0x101;

            PrintL(n2 & t2);

            int n3 = 0xf;
            int t3 = 8;

            PrintL(n3 & t3);

            /// 비트연산은 보통 연산자보다 빠르다고 알려져있음 .
            /// 테스트 ㄱ 
            /// (결과: 개뿔도 차이없음.)
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 1000000000; i++)
            {
                ulong u = 0x1 << 4;
            }
            watch.Stop();
            Console.WriteLine("Result : " + watch.ElapsedTicks);

            watch.Restart();
            for (int i = 0; i < 1000000000; i++)
            {
                ulong u = 1 * 16;
            }
            watch.Stop();
            Console.WriteLine("Result : " + watch.ElapsedTicks);
        }

        // 리스트 sort 테스트 
        static void SortTest()
        {
            SortTestClass forSortMethod = new SortTestClass();
            List<SortTestClass> scores = new List<SortTestClass>();
            Random random = new Random();

            PrintL("Original data");
            PadLines();

            for (int i = 0; i < 10; i++)
            {
                scores.Add(new SortTestClass() { score = random.Next() % 1000 });
                PrintL(scores[i].score);
            }

            PadLines();

            // IComparable 인터페이스 CompareTo 함수를 이용하여 정렬. 
            PrintL("Sort!");
            scores.Sort();
            scores.ForEach(t => PrintL(t.score));
            scores.Shuffle();

            PadLines();
            PrintL("Shuffle!");
            PadLines();

            // static 함수로 정렬 . 
            PrintL("Sort!");
            scores.Sort(SortTestClass.CompareByComparison_Static);
            scores.ForEach(t => PrintL(t.score));
            scores.Shuffle();

            PadLines();
            PrintL("Shuffle!");
            PadLines();

            // non static 일반 함수로 정렬 . 
            PrintL("Sort!");
            scores.Sort(forSortMethod.CompareByComparison_NonStatic);
            scores.ForEach(t => PrintL(t.score));
            scores.Shuffle();

            PadLines();
            PrintL("Shuffle!");
            PadLines();

            PrintL("Sort!");
            // Comparison 델리게이트 형태에만 맞춰주면 되니까 
            // 익명함수도 가능함. 
            scores.Sort((t1, t2) =>
            {
                if (t1.score < t2.score)
                {
                    return 1;
                }
                else if (t1.score > t2.score)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            });

            scores.ForEach(t => PrintL(t.score));
            scores.Shuffle();

            PadLines();
            PrintL("Shuffle!");
            PadLines();

            PrintL("Sort!");
            scores.Sort(new SortTestClass_Comparer());
            scores.ForEach(t => PrintL(t.score));
            scores.Shuffle();

            PadLines();
            PrintL("Shuffle!");
            PadLines();

            PrintL("ONLY Half Sort!");
            scores.Sort(0, (int)(scores.Count * 0.5f), new SortTestClass_Comparer());
            scores.ForEach(t => PrintL(t.score));
            scores.Shuffle();

            PadLines();
            PrintL("Shuffle!");
            PadLines();
        }

        // 공변성, 반공변성 테스트 
        static void ConvarianceTest()
        {
            //   ConvarianceTestClass01 test = new ConvarianceTestClass01();
            //   test.TestNormalCasting();
            //   test.TestConvariance();
            //   test.TestDelegate();
        }

        #endregion

        #region WeakReference 테스트 
        /// <summary>
        /// TODO 
        /// </summary>
        static void WeakReferenceTest()
        {

        }
        #endregion

        #region Environment 클래스 Usage 테스트 
        /// <summary>
        /// TODO 
        /// </summary>
        static void EnvironmentClassUsageTest()
        {
            Print($"텍스트1{System.Environment.NewLine}텍스트2{System.Environment.NewLine}텍스트3{System.Environment.NewLine}");
        }
        #endregion

        #region Text 핸들링 Usage 
        /// <summary>
        /// TODO
        /// </summary>
        static void StringTextUsageTest()
        {
            #region Split 
            string[] str = new string[]
            {
                "Show me the money,    Crack the Code Right now" , " abc ,,,   ,  "
            };

            #region 생 Split 
            Print("======== Split , 파라미터 X =========");

            foreach (var s in str)
            {
                var split = s.Split(new char[] { ',' });

                Print($"---- {split} 을 Split ---- ");
                if (split != null)
                {
                    foreach (var s_ in split)
                    {
                        Print($"SplitResult : {s_}");
                    }
                }
            }
            #endregion

            PadLines(2);

            #region 빈 문자열은 제외시키는 옵션 StringSplitOptions.RemoveEmptyEntries 사용
            Print("======== Split , 파라미터 StringSplitOptions.RemoveEmptyEntires (split 된 결과 string 이 empty 라면 애초에 리턴 값에서 제외됨) =========");
            /// => 스페이스는 Empty 가 아니기에 제외되지 않음 .
            /// => 예를 들어 "12,,,ABC 를 콤마( , ) 로 Split 을 하면
            /// [0] = 1
            /// [1] = 2
            /// [2] = 
            /// ... 이런식인데 여기서 [2] 같은 빈 값을 애초에 return 해주지 않게해주는 옵션. 
            foreach (var s in str)
            {
                var split = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Print($"---- {split} 을 Split ---- ");
                if (split != null)
                {
                    foreach (var s_ in split)
                    {
                        Print($"SplitResult : {s_}");
                    }
                }
            }
            #endregion
            #endregion

        }
        #endregion

        #region Tuple Test
        static void TupleTest()
        {
            var test = new ConsoleApp3.ClassUnitTest.Tuple.TupleTest();

            test.RunTupleTest();
        }
        #endregion

        #region Iteration Usage Test
        static void IterationUsageTest()
        {
            var test = new ConsoleApp3.ClassUnitTest.Iteration.IterationTest();
            test.RunTest();
        }
        #endregion
    }
}