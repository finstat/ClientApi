using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace DesktopFinstatApiTester.Windows
{
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private void buttonDistraintSearch_Click(object sender, RoutedEventArgs e)
        {
            if (GetPrompt(new ApiCallParameter(ParameterTypeEnum.Prompt, "This method will charge your FinStat credit. Do you want to continue?")))
            {
                doApiRequest("DistraintSearch", "SK", (parameters) =>
                {
                    var client = CreateSKApiDistraintClient();
                    var result = client.RequestDistraintSearch((string)parameters[0], (string)parameters[1], (string)parameters[2], (string)parameters[3], (string)parameters[4], (string)parameters[5]);
                    AppInstance.Limits.FromModel(client.Limits);
                    return result;
                }, new[] {
                    new ApiCallParameter(ParameterTypeEnum.String, "IČO", (parameter) => true),
                    new ApiCallParameter(ParameterTypeEnum.String, "Surname", (parameter) => true),
                    new ApiCallParameter(ParameterTypeEnum.String, "Date of Birth", (parameter) => true),
                    new ApiCallParameter(ParameterTypeEnum.String, "City", (parameter) => true),
                    new ApiCallParameter(ParameterTypeEnum.String, "Company Name", (parameter) => true),
                    new ApiCallParameter(ParameterTypeEnum.String, "File Reference", (parameter) => true),
                });
            }
        }

        private void buttonDistraintDetail_Click(object sender, RoutedEventArgs e)
        {
            if (GetPrompt(new ApiCallParameter(ParameterTypeEnum.Prompt, "This method will charge your FinStat credit. Do you want to continue?")))
            {
                doApiRequest("DistraintDetail", "SK", (parameters) =>
                {
                    var client = CreateSKApiDistraintClient();
                    var result = client.RequestDistraintDetail((string)parameters[0], ((string)parameters[1]).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => Int32.Parse(x.Trim())).ToArray());
                    AppInstance.Limits.FromModel(client.Limits);
                    return result;
                }, new[] {
                    new ApiCallParameter(ParameterTypeEnum.String, "Token"),
                    new ApiCallParameter(ParameterTypeEnum.String, "Detail ID List"),
                });
            }
        }

        private void buttonDistraintResults_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DistraintResults", "SK", (parameters) =>
            {
                var client = CreateSKApiDistraintClient();
                var result = client.RequestDistraintResults((string)parameters[0], (string)parameters[1], (string)parameters[2], (string)parameters[3], (string)parameters[4], (string)parameters[5]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO", (parameter) => true),
                new ApiCallParameter(ParameterTypeEnum.String, "Surname", (parameter) => true),
                new ApiCallParameter(ParameterTypeEnum.String, "Date of Birth", (parameter) => true),
                new ApiCallParameter(ParameterTypeEnum.String, "City", (parameter) => true),
                new ApiCallParameter(ParameterTypeEnum.String, "Company Name", (parameter) => true),
                new ApiCallParameter(ParameterTypeEnum.String, "File Reference", (parameter) => true),
            });
        }

        private void buttonDistraintResultsToken_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DistraintResultsByToken", "SK", (parameters) =>
            {
                var client = CreateSKApiDistraintClient();
                var result = client.RequestDistraintResultsByToken((string)parameters[0]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "Token"),
            });
        }

        private void buttonDistraintStoredDetail_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DistraintStoredDetail", "SK", (parameters) =>
            {
                var client = CreateSKApiDistraintClient();
                var result = client.RequestDistraintStoredDetail((string)parameters[0]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "Detail ID"),
            });
        }
    }
}
