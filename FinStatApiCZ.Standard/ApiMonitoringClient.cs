﻿using Newtonsoft.Json;
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
    public class ApiMonitoringClient : AbstractApiClient
    {

        public ApiMonitoringClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        public ApiMonitoringClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        /// <summary>
        /// Adds specified ico to monitoring.
        /// </summary>
        /// <param name="ico">The ico.</param>
        /// <returns>True if succeed otherwise false.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Specified ico {0} not found in database!
        /// or Url {0} not found!
        /// or TimeOut exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<bool> Add(string ico, bool json = false)
        {
            var list = new List<KeyValuePair<string, string>>(new[] {
                new KeyValuePair<string, string>("ico", ico),
                new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, ico)),
            });
            return await DoApiCall<bool>("/AddToMonitoring", list, json);
        }

        /// <summary>
        /// Removes specified ico from monitoring.
        /// </summary>
        /// <param name="ico">The ico.</param>
        /// <returns>True if succeed otherwise false.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Specified ico {0} not found in database!
        /// or Url {0} not found!
        /// or TimeOut exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<bool> Remove(string ico, bool json = false)
        {
            var list = new List<KeyValuePair<string, string>>(new[] {
                new KeyValuePair<string, string>("ico", ico),
                new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, ico)),
            });
            return await DoApiCall<bool>("/RemoveFromMonitoring", list, json);
        }

        /// <summary>
        /// Retrieves list of current monitorings.
        /// </summary>
        /// <returns>List of monitored ICO's.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or TimeOut exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<string[]> GetMonitorings(bool json = false)
        {
            var list = new List<KeyValuePair<string, string>>(new[] {
                new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, "list")),
            });
            return await DoApiCall<string[]>("/MonitoringList", list, json);
        }

        /// <summary>
        /// Retrieves report of events in current monitorings.
        /// </summary>
        /// <returns>List of monitoring events.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or TimeOut exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<Monitoring[]> GetReport(bool json = false)
        {
            var list = new List<KeyValuePair<string, string>>(new[] {
                new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, "report")),
            });
            return await DoApiCall<Monitoring[]>("/MonitoringReport", list, json);
        }
    }
}
