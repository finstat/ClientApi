using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<bool> Add(string ico, bool json = false)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var content = new FormUrlEncodedContent(new[] {
                         new KeyValuePair<string, string>("ico", ico),
                         new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, ico)),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });

                    result = await client.PostAsync(_url + "/AddToMonitoring" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
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
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                throw ParseErrorResponse(e, (result != null) ? result.StatusCode : (HttpStatusCode?)null, ico);
            }
            catch (TaskCanceledException e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Timeout, "Timeout exception while processing Finstat api request!", e);
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
        public async Task<bool> Remove(string ico, bool json = false)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var content = new FormUrlEncodedContent(new[] {
                         new KeyValuePair<string, string>("ico", ico),
                         new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, ico)),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });

                    result = await client.PostAsync(_url + "/RemoveFromMonitoring" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
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
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                throw ParseErrorResponse(e, (result != null) ? result.StatusCode : (HttpStatusCode?)null, ico);
            }
            catch (TaskCanceledException e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Timeout, "Timeout exception while processing Finstat api request!", e);
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
        public async Task<string[]> GetMonitorings(bool json = false)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var content = new FormUrlEncodedContent(new[] {
                         new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, "list")),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });

                    result = await client.PostAsync(_url + "/MonitoringList" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
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
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                throw ParseErrorResponse(e, (result != null) ? result.StatusCode : (HttpStatusCode?)null);
            }
            catch (TaskCanceledException e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Timeout, "Timeout exception while processing Finstat api request!", e);
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
        public async Task<Monitoring[]> GetReport(bool json = false)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var content = new FormUrlEncodedContent(new[] {
                         new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, "report")),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });

                    result = await client.PostAsync(_url + "/MonitoringReport" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
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
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                throw ParseErrorResponse(e, (result != null) ? result.StatusCode : (HttpStatusCode?)null);
            }
            catch (TaskCanceledException e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Timeout, "Timeout exception while processing Finstat api request!", e);
            }
            catch (Exception e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Unknown, "Unknown exception while processing Finstat api request!", e);
            }
        }
    }
}
