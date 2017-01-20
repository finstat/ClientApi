using Newtonsoft.Json;
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
    public class ApiDailyDiffClient :AbstractApiClient
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
        /// or Unknown exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<DailyDiffList> RequestListOfDailyDiffs(bool json = false)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var content = new FormUrlEncodedContent(new[] {
                         new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, null)),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });

                    result = await client.PostAsync(_url + "/GetListOfDiffs" + (json ? ".json" : null), content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        var response = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());
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
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                throw ParseErrorResponse(e, (result != null) ? result.StatusCode : (HttpStatusCode?)null);
            }
            catch (TaskCanceledException e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Timeout, "Timeout exception while processing Finstat api request!", e);
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
        /// or Unknown exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public async Task<string> DownloadDailyDiffFile(string fileName, string exportPath)
        {
            HttpResponseMessage result = null;
            try
            {
                using (HttpClient client = CreateClient(_timeout))
                {
                    var content = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("apiKey", _apiKey),
                         new KeyValuePair<string, string>("fileName", fileName),
                         new KeyValuePair<string, string>("Hash", ComputeVerificationHash(_apiKey, _privateKey, fileName)),
                         new KeyValuePair<string, string>("StationId", _stationId),
                         new KeyValuePair<string, string>("StationName", _stationName),
                    });
                    result = await client.PostAsync(_url + "/GetFile", content);
                    result.EnsureSuccessStatusCode();
                    if (result.IsSuccessStatusCode)
                    {
                        byte[] responsebytes = result.Content.ReadAsByteArrayAsync().Result;
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
                    }
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                throw ParseErrorResponse(e, (result != null) ? result.StatusCode : (HttpStatusCode?)null);
            }
            catch (TaskCanceledException e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Timeout, "Timeout exception while processing Finstat api request!", e);
            }
            catch (Exception e)
            {
                throw new FinstatApiException(FinstatApiException.FailTypeEnum.Unknown, "Unknown exception while processing Finstat api request!", e);
            }
        }
    }
}
