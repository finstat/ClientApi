using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesktopFinstatApiTester.Windows
{
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private void buttonReportingTopics_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("ReportingTopics", "SK", (parameters) =>
            {
                var client = CreateSKApiReportingClient();
                var result = client.RequestTopics();
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonReportingList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("ReportingList", "SK", (parameters) =>
            {
                var client = CreateSKApiReportingClient();
                var result = client.RequestList((string)parameters[0]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                 new ApiCallParameter(ParameterTypeEnum.String, "Topic"),
            });
        }

        private void buttonDownloadReportingFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DownloadReportingOutput", "SK", (parameters) =>
            {
                var client = CreateSKApiReportingClient();
                var result = client.DownloadReportFile((string)parameters[0], (string)parameters[1]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "File Name"),
                new ApiCallParameter(ParameterTypeEnum.Folder, "Select Save Folder")
            });
        }
    }
}
