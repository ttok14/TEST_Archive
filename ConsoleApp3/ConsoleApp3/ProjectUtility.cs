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
    class ProjectUtility
    {
        public static readonly string TestDataStoragePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/JayceExperimentProjectDataCollection";

        public static void SetupTestEnvironment()
        {
            if (Directory.Exists(TestDataStoragePath) == false)
            {
                Directory.CreateDirectory(TestDataStoragePath);
            }
        }

        public static void OpenDataStorageFolder()
        {
            System.Diagnostics.Process.Start(TestDataStoragePath);
        }
    }
}
