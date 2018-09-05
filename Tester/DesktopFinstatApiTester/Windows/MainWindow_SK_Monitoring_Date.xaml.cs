using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesktopFinstatApiTester.Windows
{
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private void buttonMonitoringDateAdd_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateAdd", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.AddDate((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "Date")
            });
        }

        private void buttonMonitoringDateRemove_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateRemove", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.RemoveDate((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "Date")
            });
        }

        private void buttonMonitoringDateList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateList", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetDateMonitorings(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonMonitoringDateReport_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateReport", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetDateReport(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonMonitoringDateProceedings_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateProceedings", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetDateProceedings(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }
    }
}
