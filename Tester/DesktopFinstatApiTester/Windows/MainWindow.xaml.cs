extern alias CZ;

using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace DesktopFinstatApiTester.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        protected enum ParameterTypeEnum
        {
            String,
            Folder,
            File,
            None
        }

        protected ViewModel.ApiApplication AppInstance
        {
            get
            {
                return (Application.Current != null) ? (Application.Current as App).Instance : null;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            DataContext = AppInstance;
            if (String.IsNullOrEmpty(AppInstance?.Settings?.ApiKeys?.PublicKey))
            {
                settingBackStage.IsOpen = true;
            }
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ShowException(e.Exception);
        }

        public void ShowException(Exception ex, string message = null)
        {
            if (ex != null)
            {
                if (ex is FinstatApi.FinstatApiException)
                {
                    var fex = (ex as FinstatApi.FinstatApiException);
                    MessageBox.Show(fex.Message, "FinStat SK API Exception", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                else if (ex is CZ::FinstatApi.FinstatApiException)
                {
                    var fex = (ex as CZ::FinstatApi.FinstatApiException);
                    MessageBox.Show(fex.Message, "FinStat CZ API Exception", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                else if (ex is WebException)
                {
                    MessageBox.Show(ex.Message, "Web Exception", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                else if (ex is TimeoutException)
                {
                    MessageBox.Show(ex.Message, "Timeout Exception", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                else if(MessageBox.Show(((!String.IsNullOrEmpty(message)) ? message : "An error occured.") + " Want to see more details?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    var text = ex.Message + "\n-------------\n" + ex.StackTrace;
                    OutputWindow window = new OutputWindow(text)
                    {
                        Owner = this,
                        Title = "Error"
                    };
                    var result = window.ShowDialog();
                }
            }
            else if (ex == null)
            {
                MessageBox.Show("An error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            LoadSettingsViewModel();
            settingBackStage.IsOpen = false;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Save(false);
        }

        private void buttonSaveAndStore_Click(object sender, RoutedEventArgs e)
        {
            Save(true);
        }

        private void Save(bool store = false)
        {
            if (controlSettings.IsValid())
            {
                var viewModel = (ViewModel.Settings)backStageTabItemAccess.DataContext;
                AppInstance.Settings = viewModel.ToModel(AppInstance.Settings);
                if (store)
                {
                    AppInstance.Save();
                }
                settingBackStage.IsOpen = false;
            }
        }

        private void backStageTabItemAccess_Initialized(object sender, EventArgs e)
        {
            LoadSettingsViewModel();
        }

        private void LoadSettingsViewModel()
        {
            var viewModel = (ViewModel.Settings)backStageTabItemAccess.DataContext;
            if (viewModel == null)
            {
                viewModel = new ViewModel.Settings();
                backStageTabItemAccess.DataContext = viewModel;
            }
            viewModel.FromModel(AppInstance.Settings);
        }

        private string GetInput()
        {
            InputWindow input = new InputWindow()
            {
                Owner = this,
            };
            if (input.ShowDialog() == true)
            {
                return input.Text?.Trim();
            }
            return null;
        }

        private string GetFolderBrowserDialog()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK || dialog.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                   return dialog.SelectedPath;
                }
            }
            return null;
        }

        private string GetFileBrowserDialog()
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK || dialog.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    return dialog.FileName;
                }
            }
            return null;
        }

        private void datagridResponse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            treeViewObjectGraph.ItemsSource = null;
            var grid = (DataGrid)sender;
            if (grid.SelectedItems != null && grid.SelectedItems.Count > 0)
            {
                var item = (ViewModel.ResponseItem)grid.SelectedItem;
                if (item != null && item.DataCount > 0)
                {
                    var graph = new ViewModel.ObjectViewModelHierarchy(item.Data[0]);
                    treeViewObjectGraph.ItemsSource = graph.FirstGeneration;
                }
            }
        }

        private void doApiRequest(string requestname, string apisource, Func<string[], object> apiCallFunc, bool hasParameter, ParameterTypeEnum[] parameterTypes = null)
        {
            List<string> parameters = new List<string>();
            if (hasParameter)
            {
                if (parameterTypes != null && parameterTypes.Any())
                foreach(var parameterType in parameterTypes)
                switch (parameterType)
                {
                    case ParameterTypeEnum.String: parameters.Add(GetInput()); break;
                    case ParameterTypeEnum.Folder: parameters.Add(GetFolderBrowserDialog()); break;
                    case ParameterTypeEnum.File: parameters.Add(GetFileBrowserDialog()); break;
                }
            }
            if (!hasParameter || (hasParameter && parameters != null && !parameters.Any(x=> string.IsNullOrEmpty(x))))
            {
                object detail = null;
                ViewModel.ResponseItem item = null;
                Exception ex = null;
                var statusWindow = new Windows.StatusWindow(3)
                {
                    Owner = this
                };
                statusWindow.Start(() =>
                {
                    try
                    {
                        statusWindow.Update(1, "Creating Request");
                        if (Dispatcher.CheckAccess())
                        {
                            item = AppInstance.Add(requestname, apisource, string.Join("; ", parameters));
                        }
                        else
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                item = AppInstance.Add(requestname, apisource, string.Join("; ", parameters));
                            }));
                        }
                        statusWindow.Update(2, "Requesting api call");
                        detail = apiCallFunc(parameters.ToArray());
                        statusWindow.Update(3, "Storing Response");
                    }
                    catch (Exception e)
                    {
                        ex = e;
                    }
                },
                () =>
                {
                    if (ex != null)
                        if (Dispatcher.CheckAccess())
                        {
                            ShowException(ex);
                        }
                        else
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                ShowException(ex);
                            }));
                        }
                    else
                    {
                        if (Dispatcher.CheckAccess())
                        {
                            item.AddData(new[] { detail });
                        }
                        else
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                item.AddData(new[] { detail });
                            }));
                        }
                    }
                });
            }
            else if (hasParameter && ( parameters == null || !parameters.Any()|| parameters.Any(x => string.IsNullOrEmpty(x))))
            {
                MessageBox.Show("No parameters supplied or missing parameters", "Error", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
            }
        }

        private FinstatApi.ApiClient CreateSKApiClient()
        {
            return new FinstatApi.ApiClient("https://www.finstat.sk/api", AppInstance.Settings.ApiKeys.PublicKey, AppInstance.Settings.ApiKeys.PrivateKey, AppInstance.Settings.StationID, AppInstance.Settings.StationName, AppInstance.Settings.TimeOut);
        }

        private FinstatApi.ApiMonitoringClient CreateSKApiMonitoringClient()
        {
            return new FinstatApi.ApiMonitoringClient("https://www.finstat.sk/api", AppInstance.Settings.ApiKeys.PublicKey, AppInstance.Settings.ApiKeys.PrivateKey, AppInstance.Settings.StationID, AppInstance.Settings.StationName, AppInstance.Settings.TimeOut);
        }

        private FinstatApi.ApiDailyDiffClient CreateSKApiDailyDiffClient()
        {
            return new FinstatApi.ApiDailyDiffClient("https://www.finstat.sk/api", AppInstance.Settings.ApiKeys.PublicKey, AppInstance.Settings.ApiKeys.PrivateKey, AppInstance.Settings.StationID, AppInstance.Settings.StationName, AppInstance.Settings.TimeOut);
        }

        private FinstatApi.ApiDailyStatementDiffClient CreateSKApiDailyStatementDiffClient()
        {
            return new FinstatApi.ApiDailyStatementDiffClient("https://www.finstat.sk/api", AppInstance.Settings.ApiKeys.PublicKey, AppInstance.Settings.ApiKeys.PrivateKey, AppInstance.Settings.StationID, AppInstance.Settings.StationName, AppInstance.Settings.TimeOut);
        }

        private FinstatApi.ApiDailyUltimateDiffClient CreateSKApiDailyUltimateDiffClient()
        {
            return new FinstatApi.ApiDailyUltimateDiffClient("https://www.finstat.sk/api", AppInstance.Settings.ApiKeys.PublicKey, AppInstance.Settings.ApiKeys.PrivateKey, AppInstance.Settings.StationID, AppInstance.Settings.StationName, AppInstance.Settings.TimeOut);
        }

        private bool IsJSON()
        {
            return AppInstance?.Settings?.ResponseType == Model.ResponseType.JSON;
        }

        private void buttonDetail_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Detail", "SK", (parameters) =>
            {
                FinstatApi.ApiClient client = CreateSKApiClient();
                return client.RequestDetail(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonExtended_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Extended", "SK", (parameters) =>
            {
                var client = CreateSKApiClient();
                return client.RequestExtendedDetail(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonUltimate_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Ultimate", "SK", (parameters) =>
            {
                var client = CreateSKApiClient();
                return client.RequestUltimateDetail(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonAutoComplete_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Autocomplete", "SK", (parameters) =>
            {
                var client = CreateSKApiClient();
                return client.RequestAutocomplete(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonAutoLogin_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("AutoLogIn", "SK", (parameters) =>
            {
                var client = CreateSKApiClient();
                return client.RequestAutoLogin(parameters[0]);
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonMonitoringIcoAdd_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOAdd", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                return client.Add(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonMonitoringIcoRemove_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICORemove", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                return client.Remove(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonMonitoringIcoList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOList", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                return client.GetMonitorings(IsJSON());
            }, false);
        }

        private void buttonMonitoringIcoReport_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOReport", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                return client.GetReport(IsJSON());
            }, false);
        }

        private void buttonMonitoringIcoProceedings_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOProceedings", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                return client.GetProceedings(IsJSON());
            }, false);
        }

        private void buttonMonitoringDateAdd_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateAdd", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                return client.AddDate(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonMonitoringDateRemove_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateRemove", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                return client.RemoveDate(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonMonitoringDateList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateList", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                return client.GetDateMonitorings(IsJSON());
            }, false);
        }

        private void buttonMonitoringDateReport_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateReport", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                return client.GetDateReport(IsJSON());
            }, false);
        }

        private void buttonMonitoringDateProceedings_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateProceedings", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                return client.GetDateProceedings(IsJSON());
            }, false);
        }

        private void buttonDailyDiffList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyDiffList", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyDiffClient();
                return client.RequestListOfDailyDiffs(IsJSON());
            }, false);
        }

        private void buttonDailyDiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyDiffFile", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyDiffClient();
                return client.DownloadDailyDiffFile(parameters[0], parameters[1]);
            }, true, new[] { ParameterTypeEnum.String, ParameterTypeEnum.Folder });
        }

        private void buttonOpenDailyDiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Open DailyDiffFile", "SK", (parameters) =>
            {
                using (ZipFile zip = new ZipFile(parameters[0]))
                {
                    var enumerator = zip.Entries.GetEnumerator();
                    enumerator.MoveNext();
                    ZipEntry firstItem = enumerator.Current;
                    XmlSerializer serializer = new XmlSerializer(typeof(FinstatApi.ViewModel.Diff.ExtendedResult[]));
                    return (FinstatApi.ViewModel.Diff.ExtendedResult[])serializer.Deserialize(firstItem.OpenReader());
                }
            }, true, new[] { ParameterTypeEnum.File });
        }

        private void buttonDailyStatementDiffList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyStatementDiffList", "SK", (parameter) =>
            {
                var client = CreateSKApiDailyStatementDiffClient();
                return client.RequestListOfDailyStatementDiffs(IsJSON());
            }, false);
        }

        private void buttonDailyStatementDiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyStatementDiffFile", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyStatementDiffClient();
                return client.DownloadDailyStatementDiffFile(parameters[0], parameters[1]);
            }, true, new[] { ParameterTypeEnum.String, ParameterTypeEnum.Folder });
        }

        private void buttonOpenDailyStatementDiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Open DailyStatementDiffFile", "SK", (parameters) =>
            {
                using (ZipFile zip = new ZipFile(parameters[0]))
                {
                    var enumerator = zip.Entries.GetEnumerator();
                    enumerator.MoveNext();
                    ZipEntry firstItem = enumerator.Current;
                    XmlSerializer serializer = new XmlSerializer(typeof(FinstatApi.ViewModel.Diff.StatementResult[]));
                    return (FinstatApi.ViewModel.Diff.StatementResult[])serializer.Deserialize(firstItem.OpenReader());
                }
            }, true, new[] { ParameterTypeEnum.File });
        }

        private void buttonDailyUltimateDiffList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyUltimateDiffList", "SK", (parameter) =>
            {
                var client = CreateSKApiDailyUltimateDiffClient();
                return client.RequestListOfDailyUltimateDiffs(IsJSON());
            }, false);
        }

        private void buttonDailyUltimateDiffFile_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("DailyUltimateDiffFile", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyUltimateDiffClient();
                return client.DownloadDailyUltimateDiffFile(parameters[0], parameters[1]);
            }, true, new[] { ParameterTypeEnum.String, ParameterTypeEnum.Folder });
        }


        private void treeViewObjectGraph_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            treeViewObjectGraph.ContextMenu = treeViewObjectGraph.Resources["TreeViewItemContextMenu"] as System.Windows.Controls.ContextMenu;
        }

        private void treeViewCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjectGraph.SelectedItem != null)
            {
                Clipboard.SetText(treeViewObjectGraph.SelectedItem.ToString());
            }
        }

        private void treeViewCopyValueToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjectGraph.SelectedItem != null)
            {
                var item = (ViewModel.ObjectViewModel)treeViewObjectGraph.SelectedItem;
                Clipboard.SetText(item.Value.ToString());
            }
        }

        private void treeViewShowInOutputWindow_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjectGraph.SelectedItem != null)
            {
                OutputWindow window = new OutputWindow(treeViewObjectGraph.SelectedItem.ToString())
                {
                    Owner = this,
                    Title = "Selected Result Node"
                };
                var result = window.ShowDialog();
            }
        }

        private void treeViewSelect_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjectGraph.SelectedItem != null)
            {
                var item = (ViewModel.ObjectViewModel)treeViewObjectGraph.SelectedItem;
                item.IsSelected = !item.IsSelected;
            }
        }

        private void treeViewToggle_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjectGraph.SelectedItem != null)
            {
                var item = (ViewModel.ObjectViewModel)treeViewObjectGraph.SelectedItem;
                item.IsExpanded = !item.IsExpanded;
            }
        }

        private CZ::FinstatApi.ApiClient CreateCZApiClient()
        {

            return new CZ::FinstatApi.ApiClient("https://cz.finstat.sk/api", AppInstance.Settings.ApiKeys.PublicKey, AppInstance.Settings.ApiKeys.PrivateKey, AppInstance.Settings.StationID, AppInstance.Settings.StationName, AppInstance.Settings.TimeOut);
        }

        private CZ::FinstatApi.ApiMonitoringClient CreateCZApiMonitoringClient()
        {
            return new CZ::FinstatApi.ApiMonitoringClient("https://cz.finstat.sk/api", AppInstance.Settings.ApiKeys.PublicKey, AppInstance.Settings.ApiKeys.PrivateKey, AppInstance.Settings.StationID, AppInstance.Settings.StationName, AppInstance.Settings.TimeOut);
        }

        private void buttonCZDetail_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Detail", "CZ", (parameters) =>
            {
                var client = CreateCZApiClient();
                return client.RequestDetail(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonCZAutoComplete_Click(object sender, RoutedEventArgs e)
        {

            doApiRequest("Autocomplete", "CZ", (parameters) =>
            {
                var client = CreateCZApiClient();
                return client.RequestAutocomplete(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonCZAutoLogin_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("AutoLogIn", "CZ", (parameters) =>
            {
                var client = CreateCZApiClient();
                return client.RequestAutoLogin(parameters[0]);
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonCZMonitoringIcoAdd_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOAdd", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                return client.Add(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonCZMonitoringIcoRemove_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICORemove", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                return client.Remove(parameters[0], IsJSON());
            }, true, new[] { ParameterTypeEnum.String });
        }

        private void buttonCZMonitoringIcoList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOList", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                return client.GetMonitorings(IsJSON());
            }, false);
        }

        private void buttonCZMonitoringIcoReport_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOReport", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                return client.GetReport(IsJSON());
            }, false);
        }
    }
}
