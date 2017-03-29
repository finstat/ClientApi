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
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("ico", ico);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, ico));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
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
        public ExtendedResult RequestExtendedDetail(string ico, bool json = false)
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
                    byte[] responsebytes = client.UploadValues(_url + "/extended" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
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
        public UltimateResult RequestUltimateDetail(string ico, bool json = false)
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
                    byte[] responsebytes = client.UploadValues(_url + "/ultimate" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
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
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("query", query);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, query));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
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
        public DistraintResult RequestDistraintSearch(string ico, string surname, string dateOfBirth, string city, string companyName, string fileReference, bool json = false)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    var search = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", ico, surname, dateOfBirth, city, companyName, fileReference);

                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("search", search);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, search));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/distraintSearch" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
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
        public DistraintDetailResults RequestDistraintDetail(string token, int[] ids, bool json = false)
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

            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("token", token);
                    reqparm.Add("ids", idsParam);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, token + idsString));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/distraintDetail" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
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
        public DistraintResult RequestDistraintResults(string ico, string surname, string dateOfBirth, string city, string companyName, string fileReference, bool json = false)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    var search = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", ico, surname, dateOfBirth, city, companyName, fileReference);

                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("search", search);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, search));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/distraintResults" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
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
        public DistraintResult RequestDistraintResultsByToken(string token, bool json = false)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("token", token);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, token));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/distraintResultsByToken" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
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
        public DistraintDetailResults RequestDistraintStoredDetail(string id, bool json = false)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("id", id);
                    reqparm.Add("apiKey", _apiKey);
                    reqparm.Add("Hash", ComputeVerificationHash(_apiKey, _privateKey, id));
                    reqparm.Add("StationId", _stationId);
                    reqparm.Add("StationName", _stationName);
                    byte[] responsebytes = client.UploadValues(_url + "/distraintStoredDetail" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
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
