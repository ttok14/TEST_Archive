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

namespace ConsoleApp3
{
    class Program
    {
        #region HELPER_METHOD
        static void Print(string str)
        {
            Console.WriteLine(str);
        }
        #endregion

        static void Main(string[] args)
        {
            //// 기본 테스트 환경 세팅 . ///////////
            ProjectUtility.SetupTestEnvironment();
            //////////////////////////////////////

            // 이 밑에서 테스트 진행 
            NumberToStringUsage();
            //            WriteBinaryFormatter();
        }

        #region 제이스 테스트 코드
        // 주의 ** 바이너리로 만들 class 는 [Serializable] attribute 가 추가돼있어야함. 태그 . 
        static void WriteBinaryFormatter()
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
            Console.WriteLine(Convert.ToDateTime("05 /01/1996 17:23:29"));
        }

        // 숫자로 ToString 할때 여러가지 표현 방식 테스트 .
        /*
         * 
         */
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
            Print(tf.ToString("n"));
            // 1,234.5600 => 4 가 붙어서 소수점 4개까지 표시 
            Print(tf.ToString("n4"));
            // 1,234
            Print(tf.ToString("n0"));
            // 1234
            Print(tf.ToString("f0"));
            // 1234.560
            Print(tf.ToString("f3"));
            // 1234.6 
            Print(tf.ToString("0.0"));
            // 1234.56
            Print(tf.ToString("0.00"));
            // 01234.560
            Print(tf.ToString("00000.000"));
            // 1234.6
            Print(tf.ToString("#.#"));
            // 1234.56 
            Print(tf.ToString("#.###"));
            // preview 끝 

            Console.WriteLine();

            int n = int.MaxValue;

            Print(n.ToString("n"));
            Print(n.ToString("n0"));
            Print(n.ToString("n1"));
            Print(n.ToString("n2"));
            Print(n.ToString("n3"));
            Print(n.ToString("f"));
            Print(n.ToString("f3"));
            Print(n.ToString("0.000"));

            Console.WriteLine();

            float f = 1234.56f;

            Print(f.ToString("n"));
            Print(f.ToString("n0"));
            Print(f.ToString("n1"));
            Print(f.ToString("n2"));

            Print(f.ToString("f0"));
            Print(f.ToString("f1"));
            Print(f.ToString("f2"));
            Print(f.ToString("f0"));

            Print(f.ToString("0.0"));
            Print(f.ToString("000.0000"));
            Print(f.ToString("0000000000"));
            Print(f.ToString("0,0"));

            Print(f.ToString("##.####"));
            Print(f.ToString("#.#"));
            Print(f.ToString("#.0"));
            Print(f.ToString("#.##"));
            Print(f.ToString("#.00"));
            Print(f.ToString("#.###"));
            Print(f.ToString("#.000"));
            Print(f.ToString("#.#0#"));
            Print(f.ToString("#.0#0"));
            Print(f.ToString("#.#0#0#"));
            Print(f.ToString("#.0#0#0"));

            Console.WriteLine();

            float f02 = 0.12f;

            Print(f02.ToString("#.0"));
            Print(f02.ToString("#.##"));
            Print(f02.ToString("0.#"));
            Print(f02.ToString("#0.#0"));
            Print(f02.ToString("0#0.#0"));
            Print(f02.ToString("00#0.#####"));

            Console.WriteLine();

            int n02 = 1234;

            Print(n02.ToString("##.##"));
            Print(n02.ToString("00.00"));
            Print(n02.ToString("000000.000"));
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
        #endregion

        #region 아이작 테스트 코드 

        #endregion
    }
}
