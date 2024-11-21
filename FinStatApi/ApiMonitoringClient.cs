using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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
        public bool Add(string ico, string category = null, bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "ico", ico },
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, ico) },
            };
            if (!string.IsNullOrEmpty(category))
            {
                reqparm.Add("category", category);
            }
            return DoApiCall<bool>("/AddToMonitoring", reqparm, json);
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
        public bool Remove(string ico, string category = null, bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "ico", ico },
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, ico) },
            };
            if (!string.IsNullOrEmpty(category))
            {
                reqparm.Add("category", category);
            }
            return DoApiCall<bool>("/RemoveFromMonitoring", reqparm, json);
        }

        /// <summary>
        /// Retrieves list of current monitorings.
        /// </summary>
        /// <returns>List of monitored ICO's.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Unknown exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public string[] GetMonitorings(string category = null, bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "list") },
            };
            if (!string.IsNullOrEmpty(category))
            {
                reqparm.Add("category", category);
            }
            return DoApiCall<string[]>("/MonitoringList", reqparm, json);
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
        public Monitoring[] GetReport(string category = null, bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "report") },
            };
            if (!string.IsNullOrEmpty(category))
            {
                reqparm.Add("category", category);
            }
            return DoApiCall<Monitoring[]>("/MonitoringReport", reqparm, json);
        }

        /// <summary>
        /// Retrieves last 10 days of Proccesing events
        /// </summary>
        /// <returns>List of processing events.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or TimeOut exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public ProceedingResult[] GetProceedings(bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "proceedings") },
            };
            return DoApiCall<ProceedingResult[]>("/MonitoringProceedings", reqparm, json);
        }

        /// <summary>
        /// Adds specified date to monitoring.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>True if succeed otherwise false.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Invalid Date format
        /// or Url {0} not found!
        /// or TimeOut exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public bool AddDate(string date, bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "date", date },
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, date) },
            };
            return DoApiCall<bool>("/AddDateToMonitoring", reqparm, json);
        }

        /// <summary>
        /// Removes specified date from monitoring.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>True if succeed otherwise false.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Invalid Date format
        /// or Url {0} not found!
        /// or TimeOut exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public bool RemoveDate(string date, bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "date", date },
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, date) },
            };
            return DoApiCall<bool>("/RemoveDateFromMonitoring", reqparm, json);
        }

        /// <summary>
        /// Retrieves list of current monitoring dates.
        /// </summary>
        /// <returns>List of monitored dates.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or TimeOut exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public string[] GetDateMonitorings(bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "datelist") },
            };
            return DoApiCall<string[]>("/MonitoringDateList", reqparm, json);
        }

        /// <summary>
        /// Retrieves report of date events in current monitorings.
        /// </summary>
        /// <returns>List of monitoring events.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or TimeOut exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public MonitoringDate[] GetDateReport(bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "datereport") },
            };
            return DoApiCall<MonitoringDate[]>("/MonitoringDateReport", reqparm, json);
        }

        /// <summary>
        /// Retrieves last 10 days of Proccesing events for dates
        /// </summary>
        /// <returns>List of processing events.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or TimeOut exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public ProceedingResult[] GetDateProceedings(bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "dateproceedings") },
            };
            return DoApiCall<ProceedingResult[]>("/MonitoringDateProceedings", reqparm, json);
        }

        /// <summary>
        /// Retrieves list of user monitoring categories
        /// </summary>
        /// <returns>lLst of user monitoring categories.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or TimeOut exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        ///
        public object GetCategories(bool json)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "monitoringcategories") },
            };
            return DoApiCall<MonitoringCategory[]>("/MonitoringCategories", reqparm, json);
        }
    }
}
