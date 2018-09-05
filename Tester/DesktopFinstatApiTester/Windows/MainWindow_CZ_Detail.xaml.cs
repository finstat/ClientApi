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
        private void buttonCZDetail_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Detail", "CZ", (parameters) =>
            {
                var client = CreateCZApiClient();
                var result = client.RequestDetail((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }
    }
}
