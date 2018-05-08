﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FinstatApi
{
    public class AbstractApiClient : CommonAbstractApiClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractApiClient" /> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="privateKey">The API private key.</param>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="stationName">Name of the station.</param>
        /// <param name="timeout">The timeout in miliseconds.</param>
        public AbstractApiClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractApiClient" /> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="privateKey">The API private key.</param>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="stationName">Name of the station.</param>
        public AbstractApiClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : this("http://www.finstat.sk/api/", apiKey, privateKey, stationId, stationName, timeout)
        {

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
                        else if (e.Message.Contains("Your API access and Finstat license expired"))
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

        internal async Task<byte[]> DoApiCall(string methodUrl, List<KeyValuePair<string, string>> methodParams, bool json = false, string method = "POST")
        {
            HttpResponseMessage result = null;
            try
            {
                var list = new List<KeyValuePair<string, string>>(new[] {
                    new KeyValuePair<string, string>("apiKey", _apiKey),
                    new KeyValuePair<string, string>("StationId", _stationId),
                    new KeyValuePair<string, string>("StationName", _stationName),
                });
                if (methodParams != null && methodParams.Count > 0)
                {
                    list.AddRange(methodParams);
                }
                using (HttpClient client = CreateClient(_timeout))
                {
                    var content = new FormUrlEncodedContent(list);
                    result = (method == "POST") ? await client.PostAsync(_url + methodUrl + (json ? ".json" : null), content) : await client.GetAsync(_url + methodUrl);
                    Limits = new ViewModel.Limits
                    {
                        Daily = new ViewModel.Limit
                        {
                            Current = long.Parse(result.Headers.GetValues("Finstat-Daily-Limit-Current").First()),
                            Max = long.Parse(result.Headers.GetValues("Finstat-Daily-Limit-Max").First())
                        },
                        Monthly = new ViewModel.Limit
                        {
                            Current = long.Parse(result.Headers.GetValues("Finstat-Monthly-Limit-Current").First()),
                            Max = long.Parse(result.Headers.GetValues("Finstat-Monthly-Limit-Max").First())
                        }
                    };
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsByteArrayAsync();
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

        internal async Task<T> DoApiCall<T>(string methodUrl, List<KeyValuePair<string, string>> methodParams, bool json = false, string method = "POST")
        {
            try
            {
                var bytes = await DoApiCall(methodUrl, methodParams, json, method);
                if (bytes != null)
                {
                    var response = Encoding.UTF8.GetString(bytes);
                    using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                    {
                        if (json)
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            return (T)serializer.Deserialize(reader, typeof(T));
                        }
                        else
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(T));
                            return (T)serializer.Deserialize(reader);
                        }
                    }
                }
                return default(T);
            }
            catch (FinstatApiException e)
            {
                throw e;
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
