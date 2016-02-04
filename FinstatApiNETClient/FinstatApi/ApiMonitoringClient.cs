using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace FinstatApi
{
    public class ApiMonitoringClient :AbstractApiClient
    {
        public ApiMonitoringClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        public ApiMonitoringClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(apiKey, privateKey, stationId, stationName, timeout)
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
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey,_privateKey, ico));
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
                throw ParseErrorResponse(e, ico);
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
                throw ParseErrorResponse(e, ico);
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
                throw ParseErrorResponse(e);
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
                throw ParseErrorResponse(e);
            }
            catch (Exception e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Unknown, "Unknown exception while processing Finstat api request!", e);
            }
        }

        /// <summary>
        /// Request the ZRSR scan for specific ico and sends notifivation mail after saccing.
        /// </summary>
        /// <param name="ico">The ico.</param>
        /// <param name="email">The optional notification email,</param>
        /// <returns>True if succeed otherwise false.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Specified ico {0} not found in database!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public bool RequestZRSRScan(string ico, string email = null)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "requestzrsr"));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("ico", ico);
                    if (!string.IsNullOrEmpty(email))
                    {
                        reqparm.Add("email",  email);
                    }
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/RequestZRSRScan", "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    XmlSerializer serializer = new XmlSerializer(typeof(bool));
                    using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        return (bool)serializer.Deserialize(reader);
                    }
                }
            }
            catch (WebException e)
            {
                throw ParseErrorResponse(e, ico);
            }
            catch (Exception e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Unknown, "Unknown exception while processing Finstat api request!", e);
            }
        }
    }
}
