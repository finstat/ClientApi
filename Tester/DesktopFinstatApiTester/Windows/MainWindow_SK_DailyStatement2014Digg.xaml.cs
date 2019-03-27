using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace DesktopFinstatApiTester.Windows
{
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private void buttonDailyStatement2014DiffList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyStatement2014DiffList", "SK", (parameter) =>
            {
                var client = CreateSKApiDailyStatement2014DiffClient();
                var result = client.RequestListOfDailyStatement2014Diffs(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonDailyStatement2014DiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyStatement2014DiffFile", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyStatement2014DiffClient();
                var result = client.DownloadDailyStatement2014DiffFile((string)parameters[0], (string)parameters[1]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "File"),
                new ApiCallParameter(ParameterTypeEnum.Folder, "Select Save Folder")
            });
        }

        private void buttonOpenDailyStatement2014DiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Open DailyStatement2014DiffFile", "SK", (parameters) =>
            {
                using (ZipFile zip = new ZipFile((string)parameters[0]))
                {
                    var enumerator = zip.Entries.GetEnumerator();
                    enumerator.MoveNext();
                    ZipEntry firstItem = enumerator.Current;
                    XmlSerializer serializer = new XmlSerializer(typeof(FinstatApi.ViewModel.Diff.Statement.StatementResult[]));
                    return (FinstatApi.ViewModel.Diff.Statement.StatementResult[])serializer.Deserialize(firstItem.OpenReader());
                }
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.File, "Open Zip File")
            });
        }

        private void buttonOpenDailyStatement2014DiffLegend_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyStatementDiff2014Legend", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyStatement2014DiffClient();
                var result = client.RequestStatement2014Legend();
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }
    }
}
