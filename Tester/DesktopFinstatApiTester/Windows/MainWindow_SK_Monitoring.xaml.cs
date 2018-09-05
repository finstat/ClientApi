using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesktopFinstatApiTester.Windows
{
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private void buttonMonitoringIcoAdd_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOAdd", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.Add((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonMonitoringIcoRemove_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICORemove", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.Remove((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonMonitoringIcoList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOList", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetMonitorings(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonMonitoringIcoReport_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOReport", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetReport(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonMonitoringIcoProceedings_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOProceedings", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetProceedings(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }
    }
}
