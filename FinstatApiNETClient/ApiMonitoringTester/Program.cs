using FinstatApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ApiMonitoringTester
{
    class Program
    {
        //private const string ApiUrlConst = "http://ipv4.fiddler:3376/api/";
        //private const string ApiUrlConst = "http://localhost:3376/api/";
        private const string ApiUrlConst = "https://www.finstat.sk/api/";
        private const string TestIcoConst = "35763469";
        private const string TestDateConst = "1.1.1997";
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

            FailsWithNotValidCustomerKey();
            AddNotExistingCompany();
            AddToMonitoring(TestIcoConst);
            GetCurrentMonitorings(TestIcoConst);
            AddDateToMonitoring(TestDateConst);
            GetCurrentMonitoringDatess(TestDateConst);
            GetMonitoringReport();
            GetMonitoringDateReport();
            RemoveFromMonitoring(TestIcoConst);
            RemoveDateFromMonitoring(TestDateConst);
            Console.Write("Press any key to end...");
            Console.ReadKey();
        }

        /// <summary>
        /// Test chyby: naplatny api kluc.
        /// </summary>
        public static void FailsWithNotValidCustomerKey()
        {
            try
            {
                ApiMonitoringClient apiClient = new ApiMonitoringClient(ApiUrlConst, "not valid key", "not valid private key",  "api test", "api test", 60000);
                apiClient.GetMonitorings();
            }
            catch (FinstatApiException apiException)
            {
                if (apiException.FailType == FinstatApiException.FailTypeEnum.NotValidCustomerKey)
                {
                    Console.WriteLine("Not valid customer key: Test OK!");
                    return;
                }
            }
            Console.WriteLine("Not valid customer key: Test FAIL!");
        }

        /// <summary>
        /// Test chyby: ak ico nie je vo finstat databaze
        /// </summary>
        public static void AddNotExistingCompany()
        {
            try
            {
                ApiMonitoringClient apiClient = new ApiMonitoringClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                apiClient.Add("a12345678");
            }
            catch (FinstatApiException apiException)
            {
                if (apiException.FailType == FinstatApiException.FailTypeEnum.NotFound)
                {
                    Console.WriteLine("Not existing company: Test OK!");
                    return;
                }
            }
            Console.WriteLine("Not existing company: Test FAIL!");
        }

        /// <summary>
        /// Test pre pridanie do monitoringu
        /// </summary>
        public static void AddToMonitoring(string ico)
        {
            try
            {
                ApiMonitoringClient apiClient = new ApiMonitoringClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                bool result = apiClient.Add(ico);
                Console.WriteLine("Ident " + ico + " added to monitoring with state: " + result);
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Add to monitoring fails with exception: " + apiException);
            }
        }

        /// <summary>
        /// Test pre pridanie do monitoringu datum
        /// </summary>
        public static void AddDateToMonitoring(string date)
        {
            try
            {
                ApiMonitoringClient apiClient = new ApiMonitoringClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                bool result = apiClient.AddDate(date);
                Console.WriteLine("Date " + date + " added to monitoring with state: " + result);
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Add to monitoring fails with exception: " + apiException);
            }
        }

        /// <summary>
        /// Test pre stiahnutie zoznamu firiem v monitoring (s testom na existenciu ico)
        /// </summary>
        public static void GetCurrentMonitorings(string ico)
        {
            try
            {
                ApiMonitoringClient apiClient = new ApiMonitoringClient(ApiUrlConst, _apiKey,_privateKey, "api test", "api test", 60000);
                string[] result = apiClient.GetMonitorings();
                Console.WriteLine("There are " + result.Length +" items in monitoring");
                if (Array.IndexOf(result, ico) >= 0)
                {
                    Console.WriteLine("Ico " + ico +" found in current monitoring");
                }
                else
                {
                    Console.WriteLine("Ico " + ico +" NOT found in current monitoring");
                }
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Get current monitorings fails with exception: " + apiException);
            }
        }

        /// <summary>
        /// Test pre stiahnutie zoznamu datumov v monitoringu (s testom na existenci datum)
        /// </summary>
        public static void GetCurrentMonitoringDatess(string date)
        {
            try
            {
                ApiMonitoringClient apiClient = new ApiMonitoringClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                string[] result = apiClient.GetDateMonitorings();
                Console.WriteLine("There are " + result.Length + " items in monitoring");
                if (Array.IndexOf(result, date) >= 0)
                {
                    Console.WriteLine("Date " + date + " found in current monitoring");
                }
                else
                {
                    Console.WriteLine("Date " + date + " NOT found in current monitoring");
                }
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Get current monitorings fails with exception: " + apiException);
            }
        }

        /// <summary>
        /// Test pre stiahnutie zoznamu udalosti pre aktualne firmy v monitoring
        /// </summary>
        public static void GetMonitoringReport()
        {
            try
            {
                ApiMonitoringClient apiClient = new ApiMonitoringClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                Monitoring[] result = apiClient.GetReport();
                Console.WriteLine("There are " + result.Length +" events in monitoring for last 30 days.");
                for (int i = 0, count = result.Length >= 10 ? 10 : result.Length; i < count; i++)
                {
                    Console.WriteLine(i + ": " + result[i]);
                }
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Get current monitorings fails with exception: " + apiException);
            }
        }

        /// <summary>
        /// Test pre stiahnutie zoznamu udalosti pre aktualne datumy v monitoringu
        /// </summary>
        public static void GetMonitoringDateReport()
        {
            try
            {
                ApiMonitoringClient apiClient = new ApiMonitoringClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                MonitoringDate[] result = apiClient.GetDateReport();
                Console.WriteLine("There are " + result.Length + " events in monitoring for last 30 days.");
                for (int i = 0, count = result.Length >= 10 ? 10 : result.Length; i < count; i++)
                {
                    Console.WriteLine(i + ": " + result[i]);
                }
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Get current monitorings fails with exception: " + apiException);
            }
        }

        /// <summary>
        /// Test pre odobratie z monitoringu
        /// </summary>
        public static void RemoveFromMonitoring(string ico)
        {
            try
            {
                ApiMonitoringClient apiClient = new ApiMonitoringClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                bool result = apiClient.Remove(ico);
                Console.WriteLine("Ident " + ico + " removed to monitoring with state: " + result);
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Remove from monitoring fails with exception: " + apiException);
            }
        }

        /// <summary>
        /// Test pre odobratie datumu z monitoringu
        /// </summary>
        public static void RemoveDateFromMonitoring(string date)
        {
            try
            {
                ApiMonitoringClient apiClient = new ApiMonitoringClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                bool result = apiClient.RemoveDate(date);
                Console.WriteLine("Date " + date + " removed to monitoring with state: " + result);
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Remove from monitoring fails with exception: " + apiException);
            }
        }
    }
}
