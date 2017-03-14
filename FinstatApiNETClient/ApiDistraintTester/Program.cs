using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using FinstatApi;
using System.Text.RegularExpressions;
using System.Linq;

namespace ApiDistraintTester
{
    internal class Program
    {
        public enum LicenceVersionEnum
        {
            ErrorToValidate,
            NotValid,
            Basic,
            Extended,
            Ultimate,
            LimitExceed,
            InsufficientLicense,
            Disabled,
        }

        //private const string ApiUrlConst = "http://localhost.fiddler:3376/api/";
        private const string ApiUrlConst = "http://localhost:3376/api/";
        //private const string ApiUrlConst = "http://cz.finstat.sk/api/";
        //private const string ApiUrlConst = "http://www.finstat.sk/api/";
        private const string TestIcoConst = "35757442";
        private static string _apiKey = null;
        private static string _privateKey = null;

        private static void Main(string[] args)
        {
            _apiKey = ConfigurationManager.AppSettings["api_key"];
            _privateKey = ConfigurationManager.AppSettings["private_key"];
            if (string.IsNullOrEmpty(_apiKey) || _apiKey == "add_api_key")
            {
                Console.Write("api_key missing in .config file, please enter manually: ");
                _apiKey = Console.ReadLine().Trim();
            }
            if (string.IsNullOrEmpty(_privateKey) || _privateKey == "add_private_key")
            {
                Console.Write("private_key missing in .config file, please enter manually: ");
                _privateKey = Console.ReadLine().Trim();
            }

            var apiKeyValidation = CheckApiVersion();
            switch (apiKeyValidation)
            {
                case LicenceVersionEnum.ErrorToValidate:
                    Console.WriteLine("Not able to validate api key, probably no connection to server!!");
                    return;
                case LicenceVersionEnum.NotValid:
                    Console.WriteLine("Not valid api key!!");
                    return;
                case LicenceVersionEnum.Basic:
                    Console.WriteLine("Basic api key detected!!");
                    break;
                case LicenceVersionEnum.Extended:
                    Console.WriteLine("Extended api key detected!!");
                    break;
                case LicenceVersionEnum.Ultimate:
                    Console.WriteLine("Ultimate api key detected!!");
                    break;
                case LicenceVersionEnum.Disabled:
                    Console.WriteLine("You api access is disabled !!");
                    break;
                case LicenceVersionEnum.InsufficientLicense:
                    Console.WriteLine("Your api level is insufficient!!");
                    break;
                case LicenceVersionEnum.LimitExceed:
                    Console.WriteLine("You exceed your api limit!!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UrlNotFound();
            FailsWithNotValidCustomerKey();
            TimeoutRequest();
            NotExistingCompany();
            /*
            var searchResult = Search(null, "Kocianová", null, "Bratislava", null, null);
            if (searchResult != null && searchResult.Distraints.Length > 0)
            {
                Detail(searchResult.Distraints.FirstOrDefault().DetailToken, searchResult.Distraints.Select(x => x.DetailId).Take(3).ToArray());
            }
            */
            //var historyResult = Results(null, "Kocianová2", null, "Bratislava", null, null);
            //var historyResult = Results("xxx");
            //var detailResult = StoredDetail("ex-user-erik@myself.sk-131336264713268476-EB7D559B-F75C-4297-8F16-F9EC3077C6D3-173");

            string ident = string.Empty;
            do
            {
                Console.WriteLine();
                Console.Write("Enter ICO,SURNAME,DATE_OF_BIRTH,CITY,COMPANY_NAME,FILE_REFERENCE or just ENTER to exit:");
                ident = Console.ReadLine();
                var idents = ident.Trim(' ').Split(',').Select(x => string.IsNullOrEmpty(x) ? null : x).ToArray();
                if (idents.Length == 6)
                {
                    Search(idents[0], idents[1], idents[2], idents[3], idents[4], idents[5]);
                }
                else
                {
                    Console.WriteLine();
                    Console.Write("Some parameters (ICO, SURNAME, DATE_OF_BIRTH, CITY, COMPANY_NAME,FILE_REFERENCE) are missing!");
                }
                
            } while (!string.IsNullOrEmpty(ident));
        }

        public static LicenceVersionEnum CheckApiVersion()
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                apiClient.RequestUltimateDetail(TestIcoConst);
                return LicenceVersionEnum.Ultimate;
            }
            catch (FinstatApiException apiException)
            {
            }
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                apiClient.RequestExtendedDetail(TestIcoConst);
                return LicenceVersionEnum.Extended;
            }
            catch (FinstatApiException apiException)
            {
            }
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                apiClient.RequestDetail(TestIcoConst);
                return LicenceVersionEnum.Basic;
            }
            catch (FinstatApiException apiException)
            {
                if (apiException.FailType == FinstatApiException.FailTypeEnum.AccessDisabled)
                {
                    return LicenceVersionEnum.Disabled;
                }
                if (apiException.FailType == FinstatApiException.FailTypeEnum.InsufficientAccess)
                {
                    return LicenceVersionEnum.InsufficientLicense;
                }
                if (apiException.FailType == FinstatApiException.FailTypeEnum.LimitExceed)
                {
                    return LicenceVersionEnum.LimitExceed;
                }
                if (apiException.FailType == FinstatApiException.FailTypeEnum.NotValidCustomerKey)
                {
                    return LicenceVersionEnum.NotValid;
                }
            }
            return LicenceVersionEnum.ErrorToValidate;
        }

        /// <summary>
        /// Test pre vyhladavanie exekucie
        /// </summary>
        public static DistraintResult Search(string ico, string surname, string dateOfBirth, string city, string companyName, string fileReference)
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                var result = apiClient.RequestDistraintSearch(ico, surname, dateOfBirth, city, companyName, fileReference);
                Console.WriteLine("Load OK with values\n {0}", result);
                return result;
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Load Fails with exception: " + apiException);
                return null;
            }
        }

        /// <summary>
        /// Test pre nacitanie detailu exekucie
        /// </summary>
        public static DistraintDetailResults Detail(string token, int[] ids)
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                var result = apiClient.RequestDistraintDetail(token, ids);
                Console.WriteLine("Load OK with values\n {0}", result);
                return result;
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Load Fails with exception: " + apiException);
                return null;
            }
        }

        /// <summary>
        /// Test pre historiu vyhladavani exekucii
        /// </summary>
        public static DistraintResult Results(string ico, string surname, string dateOfBirth, string city, string companyName, string fileReference)
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                var result = apiClient.RequestDistraintResults(ico, surname, dateOfBirth, city, companyName, fileReference);
                Console.WriteLine("Load OK with values\n {0}", result);
                return result;
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Load Fails with exception: " + apiException);
                return null;
            }
        }

        /// <summary>
        /// Test pre historiu vyhladavani exekucii
        /// </summary>
        public static DistraintResult Results(string token)
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                var result = apiClient.RequestDistraintResultsByToken(token);
                Console.WriteLine("Load OK with values\n {0}", result);
                return result;
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Load Fails with exception: " + apiException);
                return null;
            }
        }

        /// <summary>
        /// Test pre nacitanie ulozeneho detailu exekucie
        /// </summary>
        public static DistraintDetailResults StoredDetail(string id)
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                var result = apiClient.RequestDistraintStoredDetail(id);
                Console.WriteLine("Load OK with values\n {0}", result);
                return result;
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Load Fails with exception: " + apiException);
                return null;
            }
        }

        /// <summary>
        /// Test chyby: ak ico nie je vo finstat databaze
        /// </summary>
        public static void NotExistingCompany()
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                apiClient.RequestDetail("a12345678");
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
        /// Test chyby: request trva dlhsie ako definovany cas
        /// </summary>
        public static void TimeoutRequest()
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 10);
                apiClient.RequestDetail(TestIcoConst);
            }
            catch (FinstatApiException apiException)
            {
                if (apiException.FailType == FinstatApiException.FailTypeEnum.Timeout)
                {
                    Console.WriteLine("Request timeout: Test OK!");
                    return;
                }
            }
            Console.WriteLine("Request timeout: Test FAIL!");
        }

        /// <summary>
        /// Test chyby: naplatny api kluc.
        /// </summary>
        public static void FailsWithNotValidCustomerKey()
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, "not valid key", "not valid key", "api test", "api test", 60000);
                apiClient.RequestDetail(TestIcoConst);
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
        /// Test chyby: ak api url nie je platne alebo nie je mozne sa pripojit
        /// </summary>
        public static void UrlNotFound()
        {
            try
            {
                ApiClient apiClient = new ApiClient("http://not.valid.sk/api", "not valid key", "not valid key", "api test", "api test",
                    60000);
                apiClient.RequestDetail(TestIcoConst);
            }
            catch (FinstatApiException apiException)
            {
                if (apiException.FailType == FinstatApiException.FailTypeEnum.UrlNotFound)
                {
                    Console.WriteLine("Url not found: Test OK!");
                    return;
                }
            }
            Console.WriteLine("Url not found: Test FAIL!");
        }

    }
}
