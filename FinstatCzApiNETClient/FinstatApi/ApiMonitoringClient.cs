using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace FinstatApi
{
    public class ApiMonitoringClient : AbstractApiClient
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
        public bool Add(string ico, bool json = false)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection
                        {
                            { "ico", ico },
                            { "apiKey", _apiKey },
                            { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, ico) },
                            { "StationId", _stationId },
                            { "StationName", _stationName }
                        };
                    byte[] responsebytes = client.UploadValues(_url + "/AddToMonitoring" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                    {
                        if (json)
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            return (bool)serializer.Deserialize(reader, typeof(bool));
                        }
                        else
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(bool));
                            return (bool)serializer.Deserialize(reader);
                        }
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
        public bool Remove(string ico, bool json = false)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection
                        {
                            { "ico", ico },
                            { "apiKey", _apiKey },
                            { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, ico) },
                            { "StationId", _stationId },
                            { "StationName", _stationName }
                        };
                    byte[] responsebytes = client.UploadValues(_url + "/RemoveFromMonitoring" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                    {
                        if (json)
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            return (bool)serializer.Deserialize(reader, typeof(bool));
                        }
                        else
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(bool));
                            return (bool)serializer.Deserialize(reader);
                        }
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
        public string[] GetMonitorings(bool json = false)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection
                        {
                            { "apiKey", _apiKey },
                            { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "list") },
                            { "StationId", _stationId },
                            { "StationName", _stationName }
                        };
                    byte[] responsebytes = client.UploadValues(_url + "/MonitoringList" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                    {
                        if (json)
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            return (string[])serializer.Deserialize(reader, typeof(string[]));
                        }
                        else
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(string[]));
                            return (string[])serializer.Deserialize(reader);
                        }
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
        public Monitoring[] GetReport(bool json = false)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection
                        {
                            { "apiKey", _apiKey },
                            { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "report") },
                            { "StationId", _stationId },
                            { "StationName", _stationName }
                        };
                    byte[] responsebytes = client.UploadValues(_url + "/MonitoringReport" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                    {
                        if (json)
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            return (Monitoring[])serializer.Deserialize(reader, typeof(Monitoring[]));
                        }
                        else
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(Monitoring[]));
                            return (Monitoring[])serializer.Deserialize(reader);
                        }
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
    }
}
