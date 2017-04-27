using Newtonsoft.Json;
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
        public DetailResult RequestDetail(string ico, bool json = false)
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
                            { "Hash", ComputeVerificationHash(_apiKey, _privateKey, ico) },
                            { "StationId", _stationId },
                            { "StationName", _stationName }
                        };
                    byte[] responsebytes = client.UploadValues(_url + "/detail" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
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
        public ApiAutocomplete RequestAutocomplete(string query, bool json = false)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection
                        {
                            { "query", query },
                            { "apiKey", _apiKey },
                            { "Hash", ComputeVerificationHash(_apiKey, _privateKey, query) },
                            { "StationId", _stationId },
                            { "StationName", _stationName }
                        };
                    byte[] responsebytes = client.UploadValues(_url + "/autocomplete" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
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
        public string RequestAutoLogin(string url, string email = null, bool json = false)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection
                        {
                            { "url", url },
                            { "apiKey", _apiKey },
                            { "Hash", ComputeVerificationHash(_apiKey, _privateKey, "autologin") },
                            { "StationId", _stationId },
                            { "StationName", _stationName }
                        };
                    if (!string.IsNullOrEmpty(email))
                    {
                        reqparm.Add("email", email);
                    }
                    byte[] responsebytes = client.UploadValues(_url + "/autologin" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
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
