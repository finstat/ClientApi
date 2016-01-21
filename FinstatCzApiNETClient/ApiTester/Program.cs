using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using FinstatApi;
using System.Text.RegularExpressions;

namespace ApiTester
{
    internal class Program
    {
        public enum LicenceVersionEnum
        {
            ErrorToValidate,
            NotValid,
            Basic,
            LimitExceed
        }

        //private const string ApiUrlConst = "http://cz.localhost.fiddler:3376/api/";
        //private const string ApiUrlConst = "http://cz.localhost:3376/api/";
        //private const string ApiUrlConst = "http://cz.finstat.sk/api/";
        private const string ApiUrlConst = "http://cz.finstat.sk/api/";
        private const string TestIcoConst = "04581806";
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
            LoadCompanyDetails(TestIcoConst, apiKeyValidation);

            string ident = string.Empty;
            do
            {
                Console.WriteLine();
                Console.Write("Enter ICO or NAME or just ENTER to exit:");
                ident = Console.ReadLine();
                if (Regex.IsMatch(ident, "^[\\d ]*$"))
                {
                    ident = ident.Replace(" ", string.Empty);
                    LoadCompanyDetails(ident, apiKeyValidation);
                }
                else if (!string.IsNullOrEmpty(ident))
                {
                    var companies = LoadAutoComplete(ident);
                    if (companies != null)
                    {
                        Console.WriteLine();
                        Console.Write("Enter number of company to get details or just ENTER to exit:");
                        ident = Console.ReadLine();
                        int companyIndex;
                        if (int.TryParse(ident, out companyIndex) && companyIndex >= 0 && companyIndex < companies.Length)
                        {
                            LoadCompanyDetails(companies[companyIndex].Ico, apiKeyValidation);
                        }
                    }
                }
            } while (!string.IsNullOrEmpty(ident));
        }

        public static LicenceVersionEnum CheckApiVersion()
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                apiClient.RequestDetail(TestIcoConst);
                return LicenceVersionEnum.Basic;
            }
            catch (FinstatApiException apiException)
            {
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
        /// Test pre nacitanie detailu firmy
        /// </summary>
        public static void LoadCompanyDetails(string ident, LicenceVersionEnum version)
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                var result = apiClient.RequestDetail(ident);
                Console.WriteLine("Load OK with values\n {0}", result);
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Load Fails with exception: " + apiException);
            }

        }

        /// <summary>
        /// Test pre nacitanie detailu firmy
        /// </summary>
        public static ApiAutocomplete.Company[] LoadAutoComplete(string query)
        {
            try
            {
                ApiClient apiClient = new ApiClient(ApiUrlConst, _apiKey, _privateKey, "api test", "api test", 60000);
                var autocomplete = apiClient.RequestAutocomplete(query);
                if (autocomplete.Results.Length > 0)
                {
                    for (int i = 0; i < autocomplete.Results.Length; i++)
                    {
                        var company = autocomplete.Results[i];
                        Console.WriteLine("[{0}] {1}{2}, {3}", i, company.Name, company.Cancelled ? " !zrušená!" : null, company.City);
                    }
                }
                else
                {
                    Console.WriteLine("There is no results found for specified query '{0}'! Try to use some of suggestion below.");
                }
                if (autocomplete.Suggestions != null && autocomplete.Suggestions.Length > 0)
                {
                    Console.WriteLine("Suggestions: {0}", string.Join(", ", autocomplete.Suggestions));
                }
                if (autocomplete.Results.Length > 0)
                {
                    return autocomplete.Results;
                }
            }
            catch (FinstatApiException apiException)
            {
                Console.WriteLine("Load Fails with exception: " + apiException);
            }
            return null;
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
                ApiClient apiClient = new ApiClient(ApiUrlConst, "not valid key", "api test", "api test", 60000);
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
                ApiClient apiClient = new ApiClient("http://not.valid.sk/api", "not valid key", "api test", "api test",
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
