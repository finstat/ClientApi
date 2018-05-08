using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace FinstatApi
{
    public class ApiDailyUltimateDiffClient : AbstractApiClient
    {
        public ApiDailyUltimateDiffClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        public ApiDailyUltimateDiffClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        /// <summary>
        /// Requests the list of Ultimate DailyDiff files.
        /// </summary>
        /// <returns>List of Ultimate DailyDiff files.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public DailyDiffList RequestListOfDailyUltimateDiffs(bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, null) },
            };
            return DoApiCall<DailyDiffList>("/GetListOfUltimateDiffs", reqparm, json);
        }

        /// <summary>
        /// Downloads .
        /// </summary>
        /// <returns>List of Ultimate DailyDiff files.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public string DownloadDailyUltimateDiffFile(string fileName, string exportPath)
        {
            try
            {
                System.Collections.Specialized.NameValueCollection reqparm =
                new System.Collections.Specialized.NameValueCollection
                {
                    { "fileName", fileName },
                    { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, fileName) },
                };
                byte[] responsebytes = DoApiCall("/GetUltimateFile", reqparm);
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
    }
}
