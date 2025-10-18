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
using System.Runtime.CompilerServices;

namespace ConsoleApp3
{
    static class Global
    {
        // public static readonly string TestDataStoragePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/JayceExperimentProjectDataCollection";

        // 프로젝트 오른쪽 마우스 클릭 => 속성 => 글로벌 => 디버그 시작 프로필 UI 열기 > <작업 디렉터리> 에 $(ProjectDir) 라고 설정하고 나면
        /// <see cref="Environment.CurrentDirectory"/> 에 프로젝트 파일 (.csproj) 의 경로가 들어감. 참고.
        public static readonly string TEST_DATA_DIRECTORY = Path.Combine(Environment.CurrentDirectory, "TestData");

        public static void SetupTestEnvironment()
        {

        }

        public static void OpenDataFolder()
        {
            System.Diagnostics.Process.Start(TEST_DATA_DIRECTORY);
        }

        public static void Shuffle<T>(this List<T> list)
        {
            int rndIdx;
            T temp;
            Random rand = new Random();

            for (int i = 0; i < list.Count; ++i)
            {
                rndIdx = rand.Next(0, list.Count);

                temp = list[i];
                list[i] = list[rndIdx];
                list[rndIdx] = temp;
            }
        }

        //public static byte[] Serialize_BinaryFormatter(object obj)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();

        //    using (var ms = new MemoryStream())
        //    {
        //        bf.Serialize(ms, obj);
        //        return ms.ToArray();
        //    }
        //}

        //public static object Deserialize_BinaryFormatter(byte[] data)
        //{
        //    using (var ms = new MemoryStream(data))
        //    {
        //        var binForm = new BinaryFormatter();
        //        return binForm.Deserialize(ms);
        //    }
        //}

        public static string ToBinary(this string data, bool formatBits = false)
        {
            char[] buffer = new char[(((data.Length * 8) + (formatBits ? (data.Length - 1) : 0)))];
            int index = 0;
            for (int i = 0; i < data.Length; i++)
            {
                string binary = Convert.ToString(data[i], 2).PadLeft(8, '0');
                for (int j = 0; j < 8; j++)
                {
                    buffer[index] = binary[j];
                    index++;
                }
                if (formatBits && i < (data.Length - 1))
                {
                    buffer[index] = ' ';
                    index++;
                }
            }
            return new string(buffer);
        }

        // 해당 함수를 호출한 File 의 Path 를 가져옴
        public static string GetCallerFilePath([CallerFilePath] string path = null)
        {
            return path;
        }
    }
}
