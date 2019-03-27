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
    public class ApiDailyStatement2014DiffClient : AbstractApiClient
    {
        public ApiDailyStatement2014DiffClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        public ApiDailyStatement2014DiffClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        /// <summary>
        /// Requests the list of Statement v2014 DailyDiff files.
        /// </summary>
        /// <returns>List of Statement v2014 DailyDiff files.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public DailyDiffList RequestListOfDailyStatement2014Diffs(bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, null) },
            };
            return DoApiCall<DailyDiffList>("/GetListOfStatement2014Diffs", reqparm, json);
        }

        /// <summary>
        /// Downloads .
        /// </summary>
        /// <returns>List of Statement v2014 DailyDiff files.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public string DownloadDailyStatement2014DiffFile(string fileName, string exportPath)
        {
            try
            {
                System.Collections.Specialized.NameValueCollection reqparm =
                new System.Collections.Specialized.NameValueCollection
                {
                    { "fileName", fileName },
                    { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, fileName) },
                };
                byte[] responsebytes = DoApiCall("/GetStatement2014File", reqparm);
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
        /// Requests Legend of statement v2014 files.
        /// </summary>
        /// <returns>List of KeyValue items.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public Statement.StatementLegendResult RequestStatement2014Legend(string lang = "sk", bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "lang", lang },
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, lang) },
            };
            return DoApiCall<Statement.StatementLegendResult>("/GetStatement2014Legend", reqparm, json);
        }
    }
}
