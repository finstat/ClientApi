using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace FinstatApi
{
    public class ApiMonitoringClient
    {
        private readonly string _url;
        private readonly string _apiKey;
        private readonly string _privateKey;
        private readonly string _stationId;
        private readonly string _stationName;
        private readonly int _timeout;


        /// <summary>
        /// Initializes a new instance of the <see cref="ApiMonitoringClient" /> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="privateKey">The API private key.</param>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="stationName">Name of the station.</param>
        /// <param name="timeout">The timeout in miliseconds.</param>
        public ApiMonitoringClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
        {
            _apiKey = apiKey;
            _privateKey = privateKey;
            _stationId = stationId;
            _stationName = stationName;
            _timeout = timeout;
            _url = url.TrimEnd('/');
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiMonitoringClient" /> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="privateKey">The API private key.</param>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="stationName">Name of the station.</param>
        public ApiMonitoringClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : this("http://www.finstat.sk/api/", apiKey, privateKey, stationId, stationName, timeout)
        {

        }

        /// <summary>
        /// Adds specified ico to monitoring.
        /// </summary>
        /// <param name="ico">The ico.</param>
        /// <returns>True if succeed otherwise false.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Specified ico {0} not found in database!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public bool Add(string ico)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("ico", ico);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ApiClient.ComputeVerificationHash(_apiKey,_privateKey, ico));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/AddToMonitoring", "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    XmlSerializer serializer = new XmlSerializer(typeof (bool));
                    using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        return (bool) serializer.Deserialize(reader);
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
        /// Removes specified ico from monitoring.
        /// </summary>
        /// <param name="ico">The ico.</param>
        /// <returns>True if succeed otherwise false.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Specified ico {0} not found in database!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public bool Remove(string ico)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("ico", ico);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, ico));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/RemoveFromMonitoring", "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    XmlSerializer serializer = new XmlSerializer(typeof (bool));
                    using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        return (bool) serializer.Deserialize(reader);
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
        /// Retrieves list of current monitorings.
        /// </summary>
        /// <returns>List of monitored ICO's.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public string[] GetMonitorings()
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "list"));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/MonitoringList", "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    XmlSerializer serializer = new XmlSerializer(typeof (string[]));
                    using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        return (string[]) serializer.Deserialize(reader);
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
        /// Retrieves report of events in current monitorings.
        /// </summary>
        /// <returns>List of monitoring events.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public Monitoring[] GetReport()
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "report"));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/MonitoringReport", "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    XmlSerializer serializer = new XmlSerializer(typeof (Monitoring[]));
                    using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        return (Monitoring[]) serializer.Deserialize(reader);
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
    }
}
