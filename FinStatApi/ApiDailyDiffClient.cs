using FinstatApi.ViewModel.Diff;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace FinstatApi
{
    public class ApiDailyDiffClient : AbstractApiClient
    {
        public ApiDailyDiffClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        public ApiDailyDiffClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        /// <summary>
        /// Requests the list of DailyDiff files.
        /// </summary>
        /// <returns>List of DailyDiff files.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public DailyDiffList RequestListOfDailyDiffs(bool json = false)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection
                        {
                            { "apiKey", _apiKey },
                            { "Hash", ComputeVerificationHash(_apiKey, _privateKey, null) },
                            { "StationId", _stationId },
                            { "StationName", _stationName }
                        };
                    byte[] responsebytes = client.UploadValues(_url + "/GetListOfDiffs" + (json ? ".json" : null), "POST", reqparm);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(response))))
                    {
                        if (json)
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            return (DailyDiffList)serializer.Deserialize(reader, typeof(DailyDiffList));
                        }
                        else
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(DailyDiffList));
                            return (DailyDiffList)serializer.Deserialize(reader);
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
        /// Downloads .
        /// </summary>
        /// <returns>List of DailyDiff files.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public string DownloadDailyDiffFile(string fileName, string exportPath)
        {
            try
            {
                using (WebClient client = new WebClientWithTimeout(_timeout))
                {
                    System.Collections.Specialized.NameValueCollection reqparm =
                        new System.Collections.Specialized.NameValueCollection
                        {
                            { "apiKey", _apiKey },
                            { "fileName", fileName },
                            { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, fileName) },
                            { "StationId", _stationId },
                            { "StationName", _stationName }
                        };
                    byte[] responsebytes = client.UploadValues(_url + "/GetFile", "POST", reqparm);
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
            }
            catch (WebException e)
            {
                if (e.Response is HttpWebResponse response)
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Forbidden:
                            throw new FinstatApiException(FinstatApiException.FailTypeEnum.NotValidCustomerKey,
                                "Not valid API key!", e);
                        case HttpStatusCode.PaymentRequired:
                            throw new FinstatApiException(FinstatApiException.FailTypeEnum.LimitExceed,
                                response.StatusDescription, e);
                    }
                }
                else if (e.Status == WebExceptionStatus.ConnectFailure || e.Status == WebExceptionStatus.NameResolutionFailure)
                {
                    throw new FinstatApiException(FinstatApiException.FailTypeEnum.UrlNotFound,
                                string.Format("Url {0} not found!", _url), e);
                }
                else if (e.Status == WebExceptionStatus.Timeout)
                {
                    throw new FinstatApiException(FinstatApiException.FailTypeEnum.Timeout,
                                string.Format("Request to url {0} timeouts in {1} miliseconds!", _url, _timeout), e);
                }
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.OtherCommunicationFail, "Unknown exception while communication with Finstat api!", e);
            }
            catch (Exception e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Unknown, "Unknown exception while processing Finstat api request!", e);
            }
        }
    }
}
