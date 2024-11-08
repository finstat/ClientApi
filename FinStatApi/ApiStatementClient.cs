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
    public class ApiStatementClient : AbstractApiClient
    {
        public ApiStatementClient(string url, string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(url, apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        public ApiStatementClient(string apiKey, string privateKey, string stationId, string stationName, int timeout)
            : base(apiKey, privateKey, stationId, stationName, timeout)
        {
        }

        /// <summary>
        /// Requests list of statements for given ico
        /// </summary>
        /// <returns>List of StatementItem items.</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exception>
        public Statement.StatementItem[] RequestStatements(string ico, bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "ico", ico },
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, ico) },
            };
            return DoApiCall<Statement.StatementItem[]>("/GetStatements", reqparm, json);
        }

        // <summary>
        /// Requests statement for given ico, year and template
        /// </summary>
        /// <returns>Statement.StatementResult or Statement.NonProfitStatementResult</returns>
        /// <exception cref="FinstatApi.FinstatApiException">
        /// Not valid API key!
        /// or Url {0} not found!
        /// or Timeout exception while communication with Finstat api!
        /// or Unknown exception while communication with Finstat api!
        /// </exceptio
        public Statement.AbstractStatementResult RequestStatementDetail(string ico, int year, Statement.TemplateTypeEnum template, bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "ico", ico },
                { "year", year.ToString() },
                { "template", template.ToString() },
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, ico + "|" + year) },
            };
            return (template == Statement.TemplateTypeEnum.TemplateNujPU || template == Statement.TemplateTypeEnum.TemplateROPO)
                 ? (DoApiCall<Statement.NonProfitStatementResult>("/GetStatementDetail", reqparm, json) as Statement.AbstractStatementResult)
                 : (DoApiCall<Statement.StatementResult>("/GetStatementDetail", reqparm, json) as Statement.AbstractStatementResult);
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
        public Statement.AbstractStatementLegendResult RequestStatementLegend(Statement.TemplateTypeEnum template, string lang = "sk", bool json = false)
        {
            System.Collections.Specialized.NameValueCollection reqparm =
            new System.Collections.Specialized.NameValueCollection
            {
                { "lang", lang },
                { "template", template.ToString() },
                { "Hash", ApiClient.ComputeVerificationHash(_apiKey, _privateKey, lang) },
            };
            return (template == Statement.TemplateTypeEnum.TemplateNujPU || template == Statement.TemplateTypeEnum.TemplateROPO)
                ? (DoApiCall<Statement.NonProfitStatementLegendResult>("/GetStatementTemplateLegend", reqparm, json) as Statement.AbstractStatementLegendResult)
                : (DoApiCall<Statement.StatementLegendResult>("/GetStatementTemplateLegend", reqparm, json) as Statement.AbstractStatementLegendResult);
        }
    }
}
