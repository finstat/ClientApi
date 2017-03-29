using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace FinstatApi
{
    public class AbstractApiClient
    {
        internal readonly string _url;
        internal readonly string _apiKey;
        internal readonly string _privateKey;
        internal readonly string _stationId;
        internal readonly string _stationName;
        internal readonly int _timeout;


        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" /> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="privateKey">The API private key.</param>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="stationName">Name of the station.</param>
        /// <param name="timeout">The timeout in miliseconds.</param>
        public AbstractApiClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (url.StartsWith("http://"))
                {
                    url = url.Replace("http://", "https://");
                }
                if (!url.StartsWith("https://"))
                {
                    url = "https://" + url;
                }
            }
            _apiKey = apiKey;
            _privateKey = privateKey;
            _stationId = stationId;
            _stationName = stationName;
            _timeout = timeout;
            _url = url.TrimEnd('/');
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" /> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="privateKey">The API private key.</param>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="stationName">Name of the station.</param>
        public AbstractApiClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : this("http://www.finstat.sk/api/", apiKey, privateKey, stationId, stationName, timeout)
        {

        }

        public static string ComputeVerificationHash(string apiKey, string privateKey, string parameter)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(string.Format("SomeSalt+{0}+{1}++{2}+ended", apiKey, privateKey, parameter));
            var algorithm = SHA256.Create();
            byte[] hash = algorithm.ComputeHash(bytes);
            StringBuilder hashString = new StringBuilder();
            foreach (byte x in hash)
            {
                hashString.Append(String.Format("{0:x2}", x));
            }
            return hashString.ToString();
        }

        internal Exception ParseErrorResponse(HttpRequestException e, HttpStatusCode? code, string parameter = null)
        {
            if (code.HasValue)
            {
                switch (code.Value)
                {
                    case HttpStatusCode.Forbidden:
                        if (e.Message.Contains("Insufficient access"))
                        {
                            return new FinstatApiException(FinstatApiException.FailTypeEnum.InsufficientAccess,
                               e.Message, e);
                        }
                        else if (e.Message.Contains("Your API access and FinStat license expired"))
                        {
                            return new FinstatApiException(FinstatApiException.FailTypeEnum.LicenseExpired,
                               e.Message, e);
                        }
                        else if (e.Message.Contains("Your API access is disabled"))
                        {
                            return new FinstatApiException(FinstatApiException.FailTypeEnum.AccessDisabled,
                               e.Message, e);
                        }
                        else if (e.Message.Contains("Invalid verification hash"))
                        {
                            return new FinstatApiException(FinstatApiException.FailTypeEnum.InvalidHash,
                               e.Message, e);
                        }
                        else
                        {
                            return new FinstatApiException(FinstatApiException.FailTypeEnum.NotValidCustomerKey,
                               e.Message, e);
                        }
                    case HttpStatusCode.PaymentRequired:
                        return new FinstatApiException(FinstatApiException.FailTypeEnum.LimitExceed,
                            e.Message, e);
                    case HttpStatusCode.NotFound:
                        if (!string.IsNullOrEmpty(parameter))
                        {
                            return new FinstatApiException(FinstatApiException.FailTypeEnum.NotFound, string.Format("Specified ico {0} not found in database!", parameter), e);
                        }
                        else
                        {
                            return new FinstatApiException(FinstatApiException.FailTypeEnum.TooShort, "Specified query is too short!", e);
                        }
                    case HttpStatusCode.RequestTimeout:
                        return new FinstatApiException(FinstatApiException.FailTypeEnum.Timeout,
                               string.Format("Request to url {0} timeouts in {1} miliseconds!", _url, _timeout), e);
                    default:
                        return new FinstatApiException(FinstatApiException.FailTypeEnum.Unknown, "Unspecified exception!", e);
                }
            }
            else
            {
                if (e.InnerException != null && e.InnerException.Message.Contains("The server name or address could not be resolved"))
                {
                    return new FinstatApiException(FinstatApiException.FailTypeEnum.UrlNotFound,   string.Format("Url {0} not found!", _url), e.InnerException);
                }

                return new FinstatApiException(FinstatApiException.FailTypeEnum.OtherCommunicationFail, "Unknown exception while communication with Finstat api!", e);
            }

        }

        internal static  HttpClient CreateClient(int? timeoutMiliSeconds)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            if (timeoutMiliSeconds.HasValue)
            {
                client.Timeout = new TimeSpan(0, 0, 0, 0, timeoutMiliSeconds.Value);
            }
            return client;
        }
    }
}
