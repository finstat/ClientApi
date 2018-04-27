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
    public class BaseApiClient : AbstractApiClient
    {
        public BaseApiClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        public BaseApiClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(apiKey, privateKey, stationId, stationName, timeout)
        {
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
    }
}
