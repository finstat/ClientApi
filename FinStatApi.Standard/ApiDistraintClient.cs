using FinstatApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FinstatApi
{
    public class ApiDistraintClient : AbstractApiClient
    {
        public ApiDistraintClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        public ApiDistraintClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        /// <summary>
        /// Requests the distraint results for specified search.
        /// </summary>
        /// <param name="ico"></param>
        /// <param name="surname"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="city"></param>
        /// <param name="companyName"></param>
        /// <param name="fileReference"></param>
        /// <param name="json"></param>
        /// <returns>DistraintResult</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<DistraintResult> RequestDistraintSearch(string ico, string surname, string dateOfBirth, string city, string companyName, string fileReference, bool json = false)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var search = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", ico, surname, dateOfBirth, city, companyName, fileReference);
                    var content = new FormUrlEncodedContent(new[] {
                         new KeyValuePair<string, string>("search", search ),
                         new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, search)),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });

                    result = await client.PostAsync(_url + "/distraintSearch" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
                        using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                        {
                            if (json)
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                return (DistraintResult)serializer.Deserialize(reader, typeof(DistraintResult));
                            }
                            else
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(DistraintResult));
                                return (DistraintResult)serializer.Deserialize(reader);
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
        /// Requests the detail results for specified token and list of detail ids.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ids"></param>
        /// <param name="json"></param>
        /// <returns>DistraintDetailResult</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<DistraintDetailResults> RequestDistraintDetail(string token, int[] ids, bool json = false)
        {
            var idsString = String.Empty;
            var idsParam = String.Empty;
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    idsString += id;
                    idsParam += (!string.IsNullOrEmpty(idsParam) ? "," : null) + id;
                }
            }

            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var content = new FormUrlEncodedContent(new[] {
                         new KeyValuePair<string, string>("token", token ),
                         new KeyValuePair<string, string>("ids", idsParam ),
                         new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, token + idsString)),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });

                    result = await client.PostAsync(_url + "/distraintDetail" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
                        using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                        {
                            if (json)
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                return (DistraintDetailResults)serializer.Deserialize(reader, typeof(DistraintDetailResults));
                            }
                            else
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(DistraintDetailResults));
                                return (DistraintDetailResults)serializer.Deserialize(reader);
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
        /// Requests the distraint results for specified search.
        /// </summary>
        /// <param name="ico"></param>
        /// <param name="surname"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="city"></param>
        /// <param name="companyName"></param>
        /// <param name="fileReference"></param>
        /// <param name="json"></param>
        /// <returns>DistraintResult</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<DistraintResult> RequestDistraintResults(string ico, string surname, string dateOfBirth, string city, string companyName, string fileReference, bool json = false)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var search = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", ico, surname, dateOfBirth, city, companyName, fileReference);
                    var content = new FormUrlEncodedContent(new[] {
                         new KeyValuePair<string, string>("search", search ),
                         new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, search)),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });

                    result = await client.PostAsync(_url + "/distraintResults" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
                        using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                        {
                            if (json)
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                return (DistraintResult)serializer.Deserialize(reader, typeof(DistraintResult));
                            }
                            else
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(DistraintResult));
                                return (DistraintResult)serializer.Deserialize(reader);
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
        /// Requests the distraint results for specified search.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="fileReference"></param>
        /// <param name="json"></param>
        /// <returns>DistraintResult</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<DistraintResult> RequestDistraintResultsByToken(string token, bool json = false)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var content = new FormUrlEncodedContent(new[] {
                         new KeyValuePair<string, string>("token", token ),
                         new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, token)),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });

                    result = await client.PostAsync(_url + "/distraintResultsByToken" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
                        using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                        {
                            if (json)
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                return (DistraintResult)serializer.Deserialize(reader, typeof(DistraintResult));
                            }
                            else
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(DistraintResult));
                                return (DistraintResult)serializer.Deserialize(reader);
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
        /// Requests the stored detail results for specified id.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ids"></param>
        /// <param name="json"></param>
        /// <returns>DistraintDetailResult</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<DistraintDetailResults> RequestDistraintStoredDetail(string id, bool json = false)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var content = new FormUrlEncodedContent(new[] {
                         new KeyValuePair<string, string>("id", id ),
                         new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, id)),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });

                    result = await client.PostAsync(_url + "/distraintStoredDetail" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
                        using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                        {
                            if (json)
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                return (DistraintDetailResults)serializer.Deserialize(reader, typeof(DistraintDetailResults));
                            }
                            else
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(DistraintDetailResults));
                                return (DistraintDetailResults)serializer.Deserialize(reader);
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
