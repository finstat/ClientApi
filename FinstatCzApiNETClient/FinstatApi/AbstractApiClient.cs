using System;
using System.Collections.Generic;
using System.Net;
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
            if (!string.IsNullOrEmpty(url) && !url.Contains("localhost"))
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
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            StringBuilder hashString = new StringBuilder();
            foreach (byte x in hash)
            {
                hashString.Append(String.Format("{0:x2}", x));
            }
            return hashString.ToString();
        }

        internal Exception ParseErrorResponse(WebException e, string parameter = null)
        {
            if (e.Response is HttpWebResponse)
            {
                ParseWebResponse(e);
            }
            else if (e.Status == WebExceptionStatus.ConnectFailure || e.Status == WebExceptionStatus.NameResolutionFailure)
            {
                return new FinstatApiException(FinstatApiException.FailTypeEnum.UrlNotFound,
                            string.Format("Url {0} not found!", _url), e);
            }
            else if (e.Status == WebExceptionStatus.Timeout)
            {
                return new FinstatApiException(FinstatApiException.FailTypeEnum.Timeout,
                            string.Format("Request to url {0} timeouts in {1} miliseconds!", _url, _timeout), e);
            }
            return new FinstatApiException(FinstatApiException.FailTypeEnum.OtherCommunicationFail, "Unknown exception while communication with Finstat api!", e);
        }
        internal static void ParseWebResponse(WebException e, string parameter = null)
        {
            HttpWebResponse response = (HttpWebResponse)e.Response;
            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    if (response.StatusDescription.StartsWith("Insufficient access"))
                    {
                        throw new FinstatApiException(FinstatApiException.FailTypeEnum.InsufficientAccess,
                           response.StatusDescription, e);
                    }
                    else if (response.StatusDescription.StartsWith("Your API access and FinStat license expired"))
                    {
                        throw new FinstatApiException(FinstatApiException.FailTypeEnum.LicenseExpired,
                           response.StatusDescription, e);
                    }
                    else if (response.StatusDescription.StartsWith("Your API access is disabled"))
                    {
                        throw new FinstatApiException(FinstatApiException.FailTypeEnum.AccessDisabled,
                           response.StatusDescription, e);
                    }
                    else if (response.StatusDescription.StartsWith("Invalid verification hash"))
                    {
                        throw new FinstatApiException(FinstatApiException.FailTypeEnum.InvalidHash,
                           response.StatusDescription, e);
                    }
                    else
                    {
                        throw new FinstatApiException(FinstatApiException.FailTypeEnum.NotValidCustomerKey,
                            response.StatusDescription, e);
                    }
                case HttpStatusCode.PaymentRequired:
                    throw new FinstatApiException(FinstatApiException.FailTypeEnum.LimitExceed,
                        response.StatusDescription, e);
                case HttpStatusCode.NotFound:
                    if (!string.IsNullOrEmpty(parameter))
                    {
                        throw new FinstatApiException(FinstatApiException.FailTypeEnum.NotFound, string.Format("Specified ico {0} not found in database!", parameter), e);
                    }
                    else
                    {
                        throw new FinstatApiException(FinstatApiException.FailTypeEnum.TooShort, "Specified query is too short!", e);
                    }
                default:
                    throw new FinstatApiException(FinstatApiException.FailTypeEnum.Unknown, "Unspecified exception!", e);
            }
        }
    }
}
