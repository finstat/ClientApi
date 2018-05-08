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
    public class ApiDailyStatementDiffClient : AbstractApiClient
    {
        public ApiDailyStatementDiffClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        public ApiDailyStatementDiffClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        /// <summary>
        /// Requests the list of Statement DailyDiff files.
        /// </summary>
        /// <returns>List of Statement DailyDiff files.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public DailyDiffList RequestListOfDailyStatementDiffs(bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, null) },
            };
            return DoApiCall<DailyDiffList>("/GetListOfStatementDiffs", reqparm, json);
        }

        /// <summary>
        /// Downloads .
        /// </summary>
        /// <returns>List of Statement DailyDiff files.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public string DownloadDailyStatementDiffFile(string fileName, string exportPath)
        {
            try
            {
                System.Collections.Specialized.NameValueCollection reqparm =
                new System.Collections.Specialized.NameValueCollection
                {
                    { "fileName", fileName },
                    { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, fileName) },
                };
                byte[] responsebytes = DoApiCall("/GetStatementFile", reqparm);
                if (responsebytes != null)
                {
                    string fullExportPath = Path.Combine(exportPath, fileName);
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

        /// <summary>
        /// Requests Legend of statement files.
        /// </summary>
        /// <returns>List of KeyValue items.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public KeyValue[] RequestStatementLegend(string lang = "sk", bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "lang", lang },
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, lang) },
            };
            return DoApiCall<KeyValue[]>("/GetStatementLegend", reqparm, json);
        }
    }
}
