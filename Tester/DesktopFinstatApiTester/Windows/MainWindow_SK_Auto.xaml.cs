using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DesktopFinstatApiTester.Windows
{
    public partial class MainWindow : Fluent.RibbonWindow
    {
        #region SK-Auto
        private void buttonAutoComplete_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Autocomplete", "SK", (parameters) =>
            {
                var client = CreateSKApiClient();
                var result = client.RequestAutocomplete((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "Text")
            });
        }

        private void buttonAutoLogin_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("AutoLogIn", "SK", (parameters) =>
            {
                var client = CreateSKApiClient();
                var result = client.RequestAutoLogin((string)parameters[0]);
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "FinStat URL"),
                new ApiCallParameter(ParameterTypeEnum.String, "Email", (parameter) => true)
            });
        }
        #endregion
    }
}
