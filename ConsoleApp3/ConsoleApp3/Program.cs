using System;
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
//using ConsoleApp3.ClassUnitTest.ConvarianceTest;

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

        static void PadLines(int lineCount = 1)
        {
            string str = "";

            for (int i = 0; i < lineCount; i++)
            {
                str += "\n";
            }

            if (string.IsNullOrEmpty(str) == false)
                Console.Write(str);
        }
        #endregion

        static void Main(string[] args)
        {
            //// 기본 테스트 환경 세팅 . ///////////
            ProjectUtility.SetupTestEnvironment();
            //////////////////////////////////////

            // 이 밑에서 테스트 진행 
            //BasicMathOperationSpeedTest();
            //   EncryptionDecryptionTest();
            // LinqUsage();
            // ExcelTest();
            // ReflectionTest();
            // RegexTest();
            //CharacterTest();
            ConvarianceTest();
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

        static void TestDateTime()
        {
            PrintL(Convert.ToDateTime("05 /01/1996 17:23:29"));
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

        // Fix 
        static void TestForeachImplement()
        {

        }

        // 숫자로 ToString 할때 여러가지 표현 방식 테스트 .
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
        static void RegexTest()
        {
            string[] address =
            {
                "riot@gmail.com"
                ,"blizzard@gmail.com"
                ,"corgi@gmail.com"
            };

            {
                var r = Regex.Match("ABc854dEF24gh", @"\d");

                if (r.Success)
                {
                    Match mat = r;
                    for (int i = 0; i < 10; i++)
                    {
                        var m = mat.Value;
                        PrintL(m + " , " + mat.Index);
                        mat = mat.NextMatch();
                    }
                }
                else PrintL("fail");
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

        #region 아이작 테스트 코드 

        #endregion
    }
}