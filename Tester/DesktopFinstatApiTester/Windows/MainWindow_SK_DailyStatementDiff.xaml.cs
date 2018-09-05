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
        private void buttonDailyStatementDiffList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyStatementDiffList", "SK", (parameter) =>
            {
                var client = CreateSKApiDailyStatementDiffClient();
                var result = client.RequestListOfDailyStatementDiffs(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonDailyStatementDiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyStatementDiffFile", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyStatementDiffClient();
                var result = client.DownloadDailyStatementDiffFile((string)parameters[0], (string)parameters[1]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "File"),
                new ApiCallParameter(ParameterTypeEnum.Folder, "Select Save Folder")
            });
        }

        private void buttonOpenDailyStatementDiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Open DailyStatementDiffFile", "SK", (parameters) =>
            {
                using (ZipFile zip = new ZipFile((string)parameters[0]))
                {
                    var enumerator = zip.Entries.GetEnumerator();
                    enumerator.MoveNext();
                    ZipEntry firstItem = enumerator.Current;
                    XmlSerializer serializer = new XmlSerializer(typeof(FinstatApi.ViewModel.Diff.StatementResult[]));
                    return (FinstatApi.ViewModel.Diff.StatementResult[])serializer.Deserialize(firstItem.OpenReader());
                }
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.File, "Open Zip File")
            });
        }

        private void buttonOpenDailyStatementDiffLegend_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyStatementDiffLegend", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyStatementDiffClient();
                var result = client.RequestStatementLegend();
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }
    }
}
