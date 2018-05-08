﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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

        internal Exception ParseErrorResponse(WebException e, string parameter = null)
        {
            if (e.Response is HttpWebResponse)
            {
                ParseWebResponse(e, parameter);
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
                    else if (response.StatusDescription.StartsWith("Your API access and Finstat license expired"))
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

        internal byte[] DoApiCall(string methodUrl, System.Collections.Specialized.NameValueCollection methodParams, bool json = false, string method = "POST")
        {
            try
            {
                System.Collections.Specialized.NameValueCollection reqparm =
                new System.Collections.Specialized.NameValueCollection
                {
                    { "apiKey", _apiKey },
                    { "StationId", _stationId },
                    { "StationName", _stationName }
                };
                if (methodParams != null && methodParams.Count > 0)
                {
                    reqparm.Add(methodParams);
                }

                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    var result = client.UploadValues(_url + methodUrl + (json ? ".json" : null), method, reqparm);
                    Limits = new ViewModel.Limits
                    {
                        Daily = new ViewModel.Limit
                        {
                            Current = (client.ResponseHeaders != null && !string.IsNullOrEmpty(client.ResponseHeaders.Get("Finstat-Daily-Limit-Current")))
                                ? long.Parse(client.ResponseHeaders.Get("Finstat-Daily-Limit-Current")) : 0,
                            Max = (client.ResponseHeaders != null && !string.IsNullOrEmpty(client.ResponseHeaders.Get("Finstat-Daily-Limit-MAx")))
                                ? long.Parse(client.ResponseHeaders.Get("Finstat-Daily-Limit-Max")) : 0
                        },
                        Monthly = new ViewModel.Limit
                        {
                            Current = (client.ResponseHeaders != null && !string.IsNullOrEmpty(client.ResponseHeaders.Get("Finstat-Monthly-Limit-Current")))
                                ? long.Parse(client.ResponseHeaders.Get("Finstat-Monthly-Limit-Current")) : 0,
                            Max = (client.ResponseHeaders != null && !string.IsNullOrEmpty(client.ResponseHeaders.Get("Finstat-Monthly-Limit-Max")))
                                ? long.Parse(client.ResponseHeaders.Get("Finstat-Monthly-Limit-Max")) : 0
                        }
                    };

                    return result;
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

        internal T DoApiCall<T>(string methodUrl, System.Collections.Specialized.NameValueCollection methodParams, bool json = false, string method = "POST")
        {
            try
            {
                byte[] responsebytes = DoApiCall(methodUrl, methodParams, json, method);
                var response = Encoding.UTF8.GetString(responsebytes);
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
            catch (FinstatApiException e)
            {
                throw e;
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
