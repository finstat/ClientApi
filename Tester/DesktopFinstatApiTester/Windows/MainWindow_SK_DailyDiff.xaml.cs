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
        private void buttonDailyDiffList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyDiffList", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyDiffClient();
                var result = client.RequestListOfDailyDiffs(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonDailyDiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyDiffFile", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyDiffClient();
                var result = client.DownloadDailyDiffFile((string)parameters[0], (string)parameters[1]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "File"),
                new ApiCallParameter(ParameterTypeEnum.Folder, "Select Save Folder")
            });
        }

        private void buttonOpenDailyDiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Open DailyDiffFile", "SK", (parameters) =>
            {
                using (ZipFile zip = new ZipFile((string)parameters[0]))
                {
                    var enumerator = zip.Entries.GetEnumerator();
                    enumerator.MoveNext();
                    ZipEntry firstItem = enumerator.Current;
                    XmlSerializer serializer = new XmlSerializer(typeof(FinstatApi.ViewModel.Diff.ExtendedResult[]));
                    return (FinstatApi.ViewModel.Diff.ExtendedResult[])serializer.Deserialize(firstItem.OpenReader());
                }
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.File, "Open Zip File")
            });
        }
    }
}
