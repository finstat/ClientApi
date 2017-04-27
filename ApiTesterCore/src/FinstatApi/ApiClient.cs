using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FinstatApi
{
    public class ApiClient : AbstractApiClient
    {
        public ApiClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        public ApiClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(apiKey, privateKey, stationId, stationName, timeout)
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
        public async Task<DetailResult> RequestDetail(string ico, bool json = false)
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
                    result = await client.PostAsync(_url + "/detail" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
                        using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                        {
                            if (json)
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                return (DetailResult)serializer.Deserialize(reader, typeof(DetailResult));
                            }
                            else
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(DetailResult));
                                return (DetailResult)serializer.Deserialize(reader);
                            }
                        }
                    }
                    return null;
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
        /// Requests the extended detail for specified ico.
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
        public async Task<ExtendedResult> RequestExtendedDetail(string ico, bool json = false)
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

                    result = await client.PostAsync(_url + "/extended" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
                        using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                        {
                            if (json)
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                return (ExtendedResult)serializer.Deserialize(reader, typeof(ExtendedResult));
                            }
                            else
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(ExtendedResult));
                                return (ExtendedResult)serializer.Deserialize(reader);
                            }
                        }
                    }
                    return null;
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
        /// Requests the ultimate detail for specified ico.
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
        public async Task<UltimateResult> RequestUltimateDetail(string ico, bool json = false)
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

                    result = await client.PostAsync(_url + "/ultimate" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
                        using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                        {
                            if (json)
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                return (UltimateResult)serializer.Deserialize(reader, typeof(UltimateResult));
                            }
                            else
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(UltimateResult));
                                return (UltimateResult)serializer.Deserialize(reader);
                            }
                        }
                    }
                    return null;
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
        public async Task<ApiAutocomplete> RequestAutocomplete(string query, bool json = false)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var content = new FormUrlEncodedContent(new[] {
                         new KeyValuePair<string, string>("query", query ),
                         new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, query)),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });

                    result = await client.PostAsync(_url + "/autocomplete" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
                        using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                        {
                            if (json)
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                return (ApiAutocomplete)serializer.Deserialize(reader, typeof(ApiAutocomplete));
                            }
                            else
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(ApiAutocomplete));
                                return (ApiAutocomplete)serializer.Deserialize(reader);
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
        /// Requests the autologin url.
        /// </summary>
        /// <param name="url">redirect url.</param>
        /// <param name="email">optional user email for login</param>
        /// <returns>Autologin url</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Not valid finstat url!
        /// or Url is empty!
        /// or Not valid user license email!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<string> RequestAutoLogin(string url, string email = null, bool json = false)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var list = new List<KeyValuePair<string, string>>(new[] {
                        new KeyValuePair<string, string>("url", url),
                        new KeyValuePair<string, string>("apiKey", _apiKey),
                        new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, "autologin")),
                        new KeyValuePair<string, string>("StationId", _stationId),
                        new KeyValuePair<string, string>("StationName", _stationName),
                    });
                    if (!string.IsNullOrEmpty(email))
                    {
                        list.Add(new KeyValuePair<string, string>("email", email));
                    }
                    var content = new FormUrlEncodedContent(list);
                    result = await client.PostAsync(_url + "/autologin" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
                        using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                        {
                            if (json)
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                return (string)serializer.Deserialize(reader, typeof(string));
                            }
                            else
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(string));
                                return (string)serializer.Deserialize(reader);
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
