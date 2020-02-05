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

namespace ConsoleApp3
{
    public class Test
    {

    }

    public enum TYPE
    {
        t01 = 0,
        t02 = 3,
        t03 = 7
    }

    [Serializable]
    public class Class
    {
        public int vInt;
        public float vFloat;
        public string vStr;
    }


    class Program
    {
        static void Main(string[] args)
        {
            // var str = File.ReadAllLines(@"C:\Users\user\Downloads\ppablist__1_android (1).bundlelist");
            // TEST(str);
            


            //            FileInfo metaFileInfo = new FileInfo(szResource + @".meta");
            //   string ext = Path.GetExtension(szResourceFullPath);
            //    [NonSerialized]    
            // Convert.ToString
            // Convert.Int32
            // 		string sGUID = AssetDatabase.AssetPathToGUID(szAssetBundlePath);
            // 			Directory.CreateDirectory(AssetBundleConfig.m_LocalPath + AssetBundleConfig.m_bundle_outputPath);
            /*   BinaryFormatter bf = new BinaryFormatter();
            //  FileStream writeFile = new FileStream(EAAssetBundleInfoMgr.GetBundleFileListPathInProject(AssetBundleConfig.INITIAL_BUNDLE_VERSION, "current"), FileMode.Create);
         //   bf.Serialize(writeFile, m_CurrentMgr);
            writeFile.Close(); */
            // EditorUtility.ClearProgressBar();
            // Ionic.Zip (namespace, nuget 으로 설치가능)
            //    File.Copy(path2, copyPath);
            //     File.Delete(copyPath);
            // Convert.ToDateTime

            //  FTP();

            //FileInfoTask();
            //   CharUnicodeTask();

            //   DateTime_();
        }

        static void TEST(string[] stringArray)
        {
            {
                //정상적으로 마스터 파일을 불러왔을경우

                Char[] delimiters = { '\t', '\t', '\t', '\t', '\t' };
                string key = string.Empty;
                for (int index = 1; index < stringArray.Length; index++)
                {
                    //순서 : 파일경로, 파일이름, CRC, 파일용량
                    string[] items = stringArray[index].Split(delimiters);

                    //11
                    key = string.Format(@"{0}/{1}", items[0], items[2]).ToLower();

                    if (items.Length == 5)
                    {
                        int n = 0;
                        //       dic.Add(key, new AssetBundleMasterFile(items[0], items[1], items[2], Int32.Parse(items[3]), Int32.Parse(items[4]), string.Empty));
                    }
                    else if (items.Length == 6)
                    {
                        int n = 0;
                        //             dic.Add(key, new AssetBundleMasterFile(items[0], items[1], items[2], Int32.Parse(items[3]), Int32.Parse(items[4]), items[5]));
                    }
                }
            }
        }

        // 주의 ** 바이너리로 만들 class 는 [Serializable] attribute 가 추가돼있어야함. 태그 . 
        static void WriteBinaryFormatter()
        {
            // 이런 방법으로도 초기화가능함. 
            var data = new Class[] {
                new Class() { vInt = 10, vFloat = 20 , vStr = "first" },
                new Class() { vInt = 20, vFloat = 30, vStr  = "second" },
                new Class() { vInt = 30, vFloat =40  , vStr = "third" } };

            string outputDir = @"C:\Users\user\Desktop\Result";

            // 맨앞에 역슬래쉬 넣으니까 에러남 뭐임 ? 
            // 무튼 데이터(.dat) 파일 생성하고 FileMode.Create 니까 Write 권한 획득됨. 
            using (var fs = new FileStream(outputDir + "/binaryFormatterResultFile.dat", FileMode.Create))
            {
                var bf = new BinaryFormatter();

                // 직렬화해서 쭉 넣어줌 
                bf.Serialize(fs, data);

                // 폴더오픈 
                System.Diagnostics.Process.Start(outputDir);
            }
        }

        // ionic zip 라이브러리 필요 . nuget에서 다운가능
        static void IonicSaveZip()
        {
            Console.WriteLine("IonicSaveZip !! task zipping");

            using (ZipFile zip = new ZipFile())
            {
                // 바탕화면에 생성할 폴더 디렉토리임 
                string dir = @"C:\Users\user\Desktop\Result";

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
                        zip.AddFile(files[i].FullName, @"Dir\Internal\Zip"); // Dir\Internal 은 해당 파일을 압축할때 zip 파일안에 생성해 넣을 디렉토리임
                        // 보통 비어놓겠지? 
                    }

                    // 해당 디렉토리에 MaZip.zip 으로 저장함 . 
                    zip.Save(dir + @"\MaZip.zip");

                    // 이 zip 파일을 extract 해서 특정 디렉토리에 더 생성 가능. 
                    zip.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                    zip.ExtractAll(dir + @"\Exctracted.zip");

                    // 익스플로어를 켜주기위해 . 
                    System.Diagnostics.Process.Start(dir);
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
        static void DateTime_()
        {
            Console.WriteLine(Convert.ToDateTime("05 /01/1996 17:23:29"));
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


    }
}
