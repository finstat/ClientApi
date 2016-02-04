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
    public class ApiClient :AbstractApiClient
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
        public DetailResult RequestDetail(string ico)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("ico", ico);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, ico));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/detail", "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    XmlSerializer serializer = new XmlSerializer(typeof (DetailResult));
                    using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        return (DetailResult) serializer.Deserialize(reader);
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
        public ExtendedResult RequestExtendedDetail(string ico)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("ico", ico);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, ico));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/extended", "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    XmlSerializer serializer = new XmlSerializer(typeof (ExtendedResult));
                    using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        return (ExtendedResult)serializer.Deserialize(reader);
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
        public UltimateResult RequestUltimateDetail(string ico)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("ico", ico);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, ico));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/ultimate", "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    XmlSerializer serializer = new XmlSerializer(typeof (UltimateResult));
                    using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        return (UltimateResult)serializer.Deserialize(reader);
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
        public ApiAutocomplete RequestAutocomplete(string query)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("query", query);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, query));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/autocomplete", "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    XmlSerializer serializer = new XmlSerializer(typeof (ApiAutocomplete));
                    using (var reader = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        return (ApiAutocomplete)serializer.Deserialize(reader);
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
