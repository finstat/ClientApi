extern alias CZ;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesktopFinstatApiTester.Windows
{
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private void buttonCZMonitoringIcoAdd_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOAdd", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.Add((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonCZMonitoringIcoRemove_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICORemove", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.Remove((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonCZMonitoringIcoList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOList", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.GetMonitorings(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonCZMonitoringIcoReport_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOReport", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.GetReport(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }
    }
}
