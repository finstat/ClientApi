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
                var result = client.Add((string)parameters[0], (string)parameters[1], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO"),
                new ApiCallParameter(ParameterTypeEnum.String, "Category", (data) => true)
            });
        }

        private void buttonCZMonitoringIcoRemove_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICORemove", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.Remove((string)parameters[0], (string)parameters[1], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO"),
                new ApiCallParameter(ParameterTypeEnum.String, "Category", (data) => true)
            });
        }

        private void buttonCZMonitoringIcoList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOList", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.GetMonitorings((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "Category", (data) => true)
            });
        }

        private void buttonCZMonitoringIcoReport_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOReport", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.GetReport((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "Category", (data) => true)
            });
        }

        private void buttonCZMonitoringCategories_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringCategories", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.GetCategories(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }
    }
}
