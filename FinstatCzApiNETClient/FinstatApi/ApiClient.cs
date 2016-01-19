using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FinstatApi
{
    public class ApiClient
    {
        private readonly string _url;
        private readonly string _apiKey;
        private readonly string _privateKey;
        private readonly string _stationId;
        private readonly string _stationName;
        private readonly int _timeout;


        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" /> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="privateKey">The API private key.</param>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="stationName">Name of the station.</param>
        /// <param name="timeout">The timeout in miliseconds.</param>
        public ApiClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
        {
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
        public ApiClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : this("http://www.finstat.sk/api/", apiKey, privateKey, stationId, stationName, timeout)
        {

        }

        /// <summary>
        /// Requests the detail for specified ico.
        /// </summary>
        /// <param name="ico">The ico.</param>
        /// <returns>Details</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Specified ico {0} not found in database!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public BaseResultCZ RequestDetail(string ico)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("ico", ico);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, ico));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/detail", "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    XmlSerializer serializer = new XmlSerializer(typeof (BaseResultCZ));
                    using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        return (BaseResultCZ) serializer.Deserialize(reader);
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Response is HttpWebResponse)
                {
                    HttpWebResponse response = (HttpWebResponse)e.Response;
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Forbidden:
                            throw new FinstatApiException(FinstatApiException.FailTypeEnum.NotValidCustomerKey,
                                "Not valid API key!", e);
                        case HttpStatusCode.PaymentRequired:
                            throw new FinstatApiException(FinstatApiException.FailTypeEnum.LimitExceed,
                                 response.StatusDescription, e);
                        case HttpStatusCode.NotFound:
                            throw new FinstatApiException(FinstatApiException.FailTypeEnum.NotFound,
                                string.Format("Specified ico {0} not found in database!", ico), e);
                    }
                }
                else if (e.Status == WebExceptionStatus.ConnectFailure || e.Status == WebExceptionStatus.NameResolutionFailure)
                {
                    throw new FinstatApiException(FinstatApiException.FailTypeEnum.UrlNotFound,
                                string.Format("Url {0} not found!", _url), e);
                }
                else if (e.Status == WebExceptionStatus.Timeout)
                {
                    throw new FinstatApiException(FinstatApiException.FailTypeEnum.Timeout,
                                string.Format("Request to url {0} timeouts in {1} miliseconds!", _url, _timeout), e);
                }
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.OtherCommunicationFail, "Unknown exception while communication with Finstat api!", e);
            }
            catch (Exception e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Unknown, "Unknown exception while processing Finstat api request!", e);
            }
        }

        /// <summary>
        /// Requests the autocomplete results for specified query.
        /// </summary>
        /// <param name="query">Query for getting autocomplete results.</param>
        /// <returns>Details</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Specified query {0} too short!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public ApiAutocomplete RequestAutocomplete(string query)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("query", query);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, query));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/autocomplete", "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    XmlSerializer serializer = new XmlSerializer(typeof (ApiAutocomplete));
                    using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        return (ApiAutocomplete)serializer.Deserialize(reader);
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Response is HttpWebResponse)
                {
                    HttpWebResponse response = (HttpWebResponse)e.Response;
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Forbidden:
                            throw new FinstatApiException(FinstatApiException.FailTypeEnum.NotValidCustomerKey,
                                "Not valid API key!", e);
                        case HttpStatusCode.PaymentRequired:
                            throw new FinstatApiException(FinstatApiException.FailTypeEnum.LimitExceed,
                                response.StatusDescription, e);
                        case HttpStatusCode.NotFound:
                            throw new FinstatApiException(FinstatApiException.FailTypeEnum.TooShort,
                                "Specified query is too short!", e);
                    }
                }
                else if (e.Status == WebExceptionStatus.ConnectFailure || e.Status == WebExceptionStatus.NameResolutionFailure)
                {
                    throw new FinstatApiException(FinstatApiException.FailTypeEnum.UrlNotFound,
                                string.Format("Url {0} not found!", _url), e);
                }
                else if (e.Status == WebExceptionStatus.Timeout)
                {
                    throw new FinstatApiException(FinstatApiException.FailTypeEnum.Timeout,
                                string.Format("Request to url {0} timeouts in {1} miliseconds!", _url, _timeout), e);
                }
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.OtherCommunicationFail, "Unknown exception while communication with Finstat api!", e);
            }
            catch (Exception e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Unknown, "Unknown exception while processing Finstat api request!", e);
            }
        }

        public static string ComputeVerificationHash(string apiKey, string privateKey, string ico)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(string.Format("SomeSalt+{0}+{1}++{2}+ended", apiKey, privateKey, ico));
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            StringBuilder hashString = new StringBuilder();
            foreach (byte x in hash)
            {
                hashString.Append(String.Format("{0:x2}", x));
            }
            return hashString.ToString();
        }
    }
}
