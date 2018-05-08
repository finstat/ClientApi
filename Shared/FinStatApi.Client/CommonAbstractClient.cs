using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace FinstatApi
{
    public class CommonAbstractApiClient
    {
        internal readonly string _url;
        internal readonly string _apiKey;
        internal readonly string _privateKey;
        internal readonly string _stationId;
        internal readonly string _stationName;
        internal readonly int _timeout;

        public ViewModel.Limits Limits { get; protected set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" /> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="privateKey">The API private key.</param>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="stationName">Name of the station.</param>
        /// <param name="timeout">The timeout in miliseconds.</param>
        public CommonAbstractApiClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
        {
            if (!string.IsNullOrEmpty(url) && !url.Contains("localhost"))
            {
                if (url.StartsWith("http://"))
                {
                    url = url.Replace("http://", "https://");
                }
                if (!url.StartsWith("https://"))
                {
                    url = "https://" + url;
                }
            }
            _apiKey = apiKey;
            _privateKey = privateKey;
            _stationId = stationId;
            _stationName = stationName;
            _timeout = timeout;
            _url = url.TrimEnd('/');
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" /> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="privateKey">The API private key.</param>
        /// <param name="stationId">The station identifier.</param>
        /// <param name="stationName">Name of the station.</param>
        public CommonAbstractApiClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : this("http://www.finstat.sk/api/", apiKey, privateKey, stationId, stationName, timeout)
        {

        }

        public static string ComputeVerificationHash(string apiKey, string privateKey, string parameter)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(string.Format("SomeSalt+{0}+{1}++{2}+ended", apiKey, privateKey, parameter));
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            StringBuilder hashString = new StringBuilder();
            foreach (byte x in hash)
            {
                hashString.Append(String.Format("{0:x2}", x));
            }
            return hashString.ToString();
        }
    }
}
