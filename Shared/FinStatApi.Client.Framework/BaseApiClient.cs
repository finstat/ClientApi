using FinstatApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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
        public ApiAutocomplete RequestAutocomplete(string query, bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
                new System.Collections.Specialized.NameValueCollection
                {
                    { "query", query },
                    { "Hash", ComputeVerificationHash(_apiKey, _privateKey, query) },
                };
            return DoApiCall<ApiAutocomplete>("/autocomplete", reqparm, json);
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
            System.Collections.Specialized.NameValueCollection reqparm =
                new System.Collections.Specialized.NameValueCollection
                {
                    { "url", url },
                    { "Hash", ComputeVerificationHash(_apiKey, _privateKey, "autologin") },
                };
            if (!string.IsNullOrEmpty(email))
            {
                reqparm.Add("email", email);
            }
            return DoApiCall<string>("/autologin", reqparm, json);
        }
    }
}
