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
        private void buttonDailyUltimateDiffList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyUltimateDiffList", "SK", (parameter) =>
            {
                var client = CreateSKApiDailyUltimateDiffClient();
                var result = client.RequestListOfDailyUltimateDiffs(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonDailyUltimateDiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyUltimateDiffFile", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyUltimateDiffClient();
                var result = client.DownloadDailyUltimateDiffFile((string)parameters[0], (string)parameters[1]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "File"),
                new ApiCallParameter(ParameterTypeEnum.Folder, "Select Save Folder")
            });
        }

        private void buttonOpenDailyUltimateDiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Open DailyUltimateDiffFile", "SK", (parameters) =>
            {
                using (ZipFile zip = new ZipFile((string)parameters[0]))
                {
                    var enumerator = zip.Entries.GetEnumerator();
                    enumerator.MoveNext();
                    ZipEntry firstItem = enumerator.Current;
                    XmlSerializer serializer = new XmlSerializer(typeof(FinstatApi.ViewModel.Diff.UltimateResult[]));
                    return (FinstatApi.ViewModel.Diff.UltimateResult[])serializer.Deserialize(firstItem.OpenReader());
                }
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.File, "Open Zip File")
            });
        }
    }
}
