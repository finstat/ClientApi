using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesktopFinstatApiTester.Windows
{
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private void buttonFree_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Basic", "SK", (parameters) =>
            {
                FinstatApi.ApiClient client = CreateSKApiClient();
                var result = client.RequestBasic((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonDetail_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Detail", "SK", (parameters) =>
            {
                FinstatApi.ApiClient client = CreateSKApiClient();
                var result = client.RequestDetail((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonExtended_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Extended", "SK", (parameters) =>
            {
                var client = CreateSKApiClient();
                var result = client.RequestExtendedDetail((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonUltimate_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Ultimate", "SK", (parameters) =>
            {
                var client = CreateSKApiClient();
                var result = client.RequestUltimateDetail((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }
    }
}
