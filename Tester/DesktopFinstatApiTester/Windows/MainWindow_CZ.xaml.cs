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
        private CZ::FinstatApi.ApiClient CreateCZApiClient()
        {
            var client = new CZ::FinstatApi.ApiClient(AppInstance.Settings.FinStatApiUrlCZ, AppInstance.Settings.ApiKeys.PublicKey, AppInstance.Settings.ApiKeys.PrivateKey, AppInstance.Settings.StationID, AppInstance.Settings.StationName, AppInstance.Settings.TimeOut);
            client.OnRequest += Client_OnRequest;
            client.OnResponse += Client_OnResponse;
            client.OnErrorResponseContent += Client_OnResponseContent;
            return client;
        }

        private CZ::FinstatApi.ApiMonitoringClient CreateCZApiMonitoringClient()
        {
            var client = new CZ::FinstatApi.ApiMonitoringClient(AppInstance.Settings.FinStatApiUrlCZ, AppInstance.Settings.ApiKeys.PublicKey, AppInstance.Settings.ApiKeys.PrivateKey, AppInstance.Settings.StationID, AppInstance.Settings.StationName, AppInstance.Settings.TimeOut);
            client.OnRequest += Client_OnRequest;
            client.OnResponse += Client_OnResponse;
            client.OnErrorResponseContent += Client_OnResponseContent;
            return client;
        }
    }
}
