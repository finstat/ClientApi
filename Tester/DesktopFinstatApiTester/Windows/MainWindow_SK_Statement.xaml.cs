using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesktopFinstatApiTester.Windows
{
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private void buttonStatements_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Statements", "SK", (parameters) =>
            {
                var client = CreateSKApiStatementClient();
                var result = client.RequestStatements((string)parameters[0]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                 new ApiCallParameter(ParameterTypeEnum.String, "ICO")
            });
        }

        private void buttonStatementDetiail_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Statements", "SK", (parameters) =>
            {
                var client = CreateSKApiStatementClient();
                var result = client.RequestStatementDetail((string)parameters[0], (int)parameters[1], (FinstatApi.Statement.TemplateTypeEnum)parameters[2]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                 new ApiCallParameter(ParameterTypeEnum.String, "ICO"),
                 new ApiCallParameter(ParameterTypeEnum.Int, "Year"),
                 new ApiCallParameter(ParameterTypeEnum.Pick, "Template") {
                     Values =  new object[]
                     {
                         FinstatApi.Statement.TemplateTypeEnum.Template2011v2,
                         FinstatApi.Statement.TemplateTypeEnum.Template2014,
                         FinstatApi.Statement.TemplateTypeEnum.Template2014micro,
                         FinstatApi.Statement.TemplateTypeEnum.TemplateFinancial,
                         FinstatApi.Statement.TemplateTypeEnum.TemplateNujPU,
                         FinstatApi.Statement.TemplateTypeEnum.TemplateROPO
                     }
                 },
            });
        }

        private void buttonStatementLegend_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("StatementDiffFile", "SK", (parameters) =>
            {
                var client = CreateSKApiStatementClient();
                var result = client.RequestStatementLegend((FinstatApi.Statement.TemplateTypeEnum)parameters[0], (string)parameters[1]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.Pick, "Template") {
                     Values =  new object[]
                     {
                         FinstatApi.Statement.TemplateTypeEnum.Template2011v2,
                         FinstatApi.Statement.TemplateTypeEnum.Template2014,
                         FinstatApi.Statement.TemplateTypeEnum.Template2014micro,
                         FinstatApi.Statement.TemplateTypeEnum.TemplateFinancial,
                         FinstatApi.Statement.TemplateTypeEnum.TemplateNujPU,
                         FinstatApi.Statement.TemplateTypeEnum.TemplateROPO
                     }
                 },
                new ApiCallParameter(ParameterTypeEnum.Pick, "Language") {
                     Values =  new object[]
                     {
                         "SK",
                         "EN"
                     }
                 }
            });
        }
    }
}
