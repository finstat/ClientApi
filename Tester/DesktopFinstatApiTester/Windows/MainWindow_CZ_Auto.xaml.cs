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
        private void buttonCZAutoComplete_Click(object sender, RoutedEventArgs e)
        {

            doApiRequest("Autocomplete", "CZ", (parameters) =>
            {
                var client = CreateCZApiClient();
                var result = client.RequestAutocomplete((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "Text")
            });
        }

        private void buttonCZAutoLogin_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("AutoLogIn", "CZ", (parameters) =>
            {
                var client = CreateCZApiClient();
                return client.RequestAutoLogin((string)parameters[0], parameters.Length > 1 ? (string)parameters[1] : null);
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "FinStat URL"),
                new ApiCallParameter(ParameterTypeEnum.String, "Email", (parameter) => true)
            });
        }
    }
}
