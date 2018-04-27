using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using FinstatApi;
using FinstatApi.ViewModel;
using FinstatApi.ViewModel.Diff;
using Ionic.Zip;

namespace ApiDailyDiffTester
{
    class Program
    {
        //private const string ApiUrlConst = "http://MYAPP/api/";
        //private const string ApiUrlConst = "http://localhost:3376/api/";
        private const string ApiUrlConst = "https://www.finstat.sk/api/";
        private static string _apiKey = null;
        private static string _privateKey = null;
        static void Main(string[] args)
        {
            _apiKey = ConfigurationManager.AppSettings["api_key"];
            if (string.IsNullOrEmpty(_apiKey) || _apiKey == "add_api_key")
            {
                Console.Write("api_key missing in .config file, please enter manually: ");
                _apiKey = Console.ReadLine().Trim();
            }

            _privateKey = ConfigurationManager.AppSettings["private_key"];
            if (string.IsNullOrEmpty(_privateKey) || _privateKey == "add_private_key")
            {
                Console.Write("private_key missing in .config file, please enter manually: ");
                _privateKey = Console.ReadLine().Trim();
            }

            var diffList = GetListOfDiffs();

            if (diffList != null && diffList.Files.Length > 0)
            {
                string pathToFirstZip = DownloadDiffFile(diffList.Files[0].FileName);
                if (!string.IsNullOrEmpty(pathToFirstZip))
                {
                    var parsedContent = ExtractAndDeserialize(pathToFirstZip);
                    Console.WriteLine("There are " + parsedContent.Length + " company changes in daily diff " + pathToFirstZip + ".");
                    for (int i = 0, count = parsedContent.Length >= 3 ? 3 : parsedContent.Length; i < count; i++)
                    {
                        Console.WriteLine(i + ": " + parsedContent[i]);
                    }
                }
            }
            Console.Write("Press any key to end...");
            Console.ReadKey();
        }



        /// <summary>
        /// Test pre stiahnutie zoznamu diff suborov z daily diff
        /// </summary>
        public static DailyDiffList GetListOfDiffs()
        {
            try
            {
                ApiDailyDiffClient apiClient = new ApiDailyDiffClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 200000);
                DailyDiffList result = apiClient.RequestListOfDailyDiffs();
                Console.WriteLine("There are " + result.Files.Length + " files in daily diff export.");
                for (int i = 0, count = result.Files.Length >= 10 ? 10 : result.Files.Length; i < count; i++)
                {
                    Console.WriteLine(i + ": " + result.Files[i]);
                }
                return result;
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Get current list of diff files fails with exception: " + apiException);
                return null;
            }
        }

        /// <summary>
        /// Test pre stiahnutie zoznamu diff suborov zavierok z daily diff
        /// </summary>
        public static DailyDiffList GetListOfStatementDiffs()
        {
            try
            {
                ApiDailyStatementDiffClient apiClient = new ApiDailyStatementDiffClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 200000);
                DailyDiffList result = apiClient.RequestListOfDailyStatementDiffs();
                Console.WriteLine("There are " + result.Files.Length + " files in daily diff export.");
                for (int i = 0, count = result.Files.Length >= 10 ? 10 : result.Files.Length; i < count; i++)
                {
                    Console.WriteLine(i + ": " + result.Files[i]);
                }
                return result;
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Get current list of diff files fails with exception: " + apiException);
                return null;
            }
        }

        /// <summary>
        /// Test pre stiahnutie daily diff zip suboru
        /// </summary>
        private static string DownloadDiffFile(string fileName)
        {
            try
            {
                ApiDailyDiffClient apiClient = new ApiDailyDiffClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                string pathToDownloadedFile = apiClient.DownloadDailyDiffFile(fileName, Directory.GetCurrentDirectory());
                if (!string.IsNullOrEmpty(pathToDownloadedFile))
                {
                    Console.WriteLine("File was succesfully downloaded to {0} with size {1}.", pathToDownloadedFile, new FileInfo(pathToDownloadedFile).Length);
                }
                return pathToDownloadedFile;
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Download file fails with exception: " + apiException);
                return null;
            }
        }

        /// <summary>
        /// Test pre stiahnutie daily diff zip suboru
        /// </summary>
        private static string DownloadStatementDiffFile(string fileName)
        {
            try
            {
                ApiDailyStatementDiffClient apiClient = new ApiDailyStatementDiffClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 200000);
                string pathToDownloadedFile = apiClient.DownloadDailyStatementDiffFile(fileName, Directory.GetCurrentDirectory());
                if (!string.IsNullOrEmpty(pathToDownloadedFile))
                {
                    Console.WriteLine("File was succesfully downloaded to {0} with size {1}.", pathToDownloadedFile, new FileInfo(pathToDownloadedFile).Length);
                }
                return pathToDownloadedFile;
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Download file fails with exception: " + apiException);
                return null;
            }
        }

        /// <summary>
        /// Test pre stiahnutie zoznamu diff suborov zavierok z daily diff
        /// </summary>
        public static KeyValue[] GetLegendOfStatementDiffs()
        {
            try
            {
                ApiDailyStatementDiffClient apiClient = new ApiDailyStatementDiffClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 200000);
                KeyValue[] result = apiClient.RequestStatementLegend();
                Console.WriteLine("There are " + result.Length + " items in statement legend.");
                for (int i = 0, count = result.Length; i < count; i++)
                {
                    Console.WriteLine(result[i]);
                }
                return result;
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Get current list of diff files fails with exception: " + apiException);
                return null;
            }
        }

        private static FinstatApi.ViewModel.Diff.ExtendedResult[] ExtractAndDeserialize(string fileName)
        {
            try
            {
                using (ZipFile zip = new ZipFile(fileName))
                {
                    var enumerator = zip.Entries.GetEnumerator();
                    enumerator.MoveNext();
                    ZipEntry firstItem = enumerator.Current;
                    XmlSerializer serializer = new XmlSerializer(typeof(FinstatApi.ViewModel.Diff.ExtendedResult[]));
                    return (FinstatApi.ViewModel.Diff.ExtendedResult[])serializer.Deserialize(firstItem.OpenReader());
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Unable to decompress and deserialize with exception: " + exception);
                return null;
            }
        }

        private static StatementResult[] ExtractAndDeserializeStatement(string fileName)
        {
            try
            {
                using (ZipFile zip = new ZipFile(fileName))
                {
                    var enumerator = zip.Entries.GetEnumerator();
                    enumerator.MoveNext();
                    ZipEntry firstItem = enumerator.Current;
                    XmlSerializer serializer = new XmlSerializer(typeof(StatementResult[]));
                    return (StatementResult[])serializer.Deserialize(firstItem.OpenReader());
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Unable to decompress and deserialize with exception: " + exception);
                return null;
            }
        }
    }
}
