using FinstatApi.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace FinstatApi
{
    public class ApiReportingClient : AbstractApiClient
    {
        public ApiReportingClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        public ApiReportingClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        /// <summary>
        /// Requests list of topics of reporting
        /// </summary>
        /// <returns>List of ReportingTopic items.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public Reporting.ReportingTopic[] RequestTopics(bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "reporting-topics") },
            };
            return DoApiCall<Reporting.ReportingTopic[]>("/GetReportingTopics", reqparm, json);
        }

        // <summary>
        /// Requests list of Generated user reporting outputs for given topic
        /// </summary>
        /// <returns>List of ReportOutput</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exceptio
        public Reporting.ReportOutput[] RequestList(string topic, bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "topic", topic },
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, "reporting-list|" + topic) },
            };
            return DoApiCall<Reporting.ReportOutput[]>("/GetReportingList", reqparm, json);
        }

        /// <summary>
        /// Downloads reporting excel File .
        /// </summary>
        /// <returns>Reporting output file.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public string DownloadReportFile(string fileName, string exportPath)
        {
            try
            {
                System.Collections.Specialized.NameValueCollection reqparm =
                new System.Collections.Specialized.NameValueCollection
                {
                    { "FileName", fileName },
                    { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, fileName) },
                };
                byte[] responsebytes = DoApiCall("/GetReportingOutput", reqparm);
                if (responsebytes != null)
                {
                    string fullExportPath = Path.Combine(exportPath, fileName + ".xlsx");
                    if (File.Exists(fullExportPath))
                    {
                        File.Delete(fullExportPath);
                    }
                    File.WriteAllBytes(fullExportPath, responsebytes);
                    return fullExportPath;
                }
                return null;
            }
            catch (FinstatApiException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Unknown, "Unknown exception while processing Finstat api request!", e);
            }
        }
    }
}
