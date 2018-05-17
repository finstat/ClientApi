extern alias CZ;
using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        protected class ApiCallParameter
        {
            public ParameterTypeEnum Type { get; set; } = ParameterTypeEnum.String;
            public string Title { get; set; }

            public ApiCallParameter() : this(string.Empty) { }
            public ApiCallParameter(string title = null) : this(ParameterTypeEnum.String, title) { }
            public ApiCallParameter(ParameterTypeEnum type = ParameterTypeEnum.String, string title = null)
            {
                Type = Type;
                Title = title;
            }
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

        #region Controls
        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ShowException(e.Exception);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        #region Helpers
        public static void Invoke(DispatcherObject control, Action action)
        {
            if (control.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                control.Dispatcher.Invoke(action);
            }
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
        #endregion

        #region Input-Dialogs
        private string GetInput(ApiCallParameter parameter)
        {
            InputWindow dialog = new InputWindow()
            {
                Owner = this,
            };

            if (parameter != null && !string.IsNullOrEmpty(parameter?.Title))
            {
                dialog.Title = parameter?.Title;
            }

            if (dialog.ShowDialog() == true)
            {
                return (!string.IsNullOrEmpty(dialog.Text)) ? dialog.Text.Trim() : null;
            }
            return null;
        }

        private string GetFolderBrowserDialog(ApiCallParameter parameter)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (parameter != null && !string.IsNullOrEmpty(parameter?.Title))
                {
                    dialog.Description = parameter?.Title;
                }
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK || dialog.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    return dialog.SelectedPath;
                }
            }
            return null;
        }

        private string GetFileBrowserDialog(ApiCallParameter parameter)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                if (parameter != null && !string.IsNullOrEmpty(parameter?.Title))
                {
                    dialog.Title = parameter?.Title;
                }
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK || dialog.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    return dialog.FileName;
                }
            }
            return null;
        }
        #endregion

        #region Control-Settings
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
            else
            {
                var errors = controlSettings.GetErrors();
                StringBuilder text = new StringBuilder();
                foreach (var kvp in errors)
                {
                    foreach (var item in kvp.Value)
                    {
                        text.AppendLine(string.Format("{0}: {1}", kvp.Key, item));
                    }
                }
                if (text.Length > 0)
                {
                    MessageBox.Show(text.ToString(), "Form errors", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                }
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
        #endregion

        #region Control-DataGridResponse
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
                    graph.FirstGeneration[0].IsSelected = true;
                    graph.FirstGeneration[0].IsExpanded = true;
                }
            }
        }
        #endregion

        #region Control-TreeViewObjectGraph
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

        private void treeViewShowXMLInOutputWindow_Click(object sender, RoutedEventArgs e)
        {
            if(treeViewObjectGraph.SelectedItem != null)
            {
                var item = (ViewModel.ObjectViewModel)treeViewObjectGraph.SelectedItem;
                MemoryStream ms = new MemoryStream();
                XmlSerializer serializer = new XmlSerializer(item.Object.GetType());
                serializer.Serialize(ms, item.Object);
                ms.Position = 0;
                StreamReader r = new StreamReader(ms);
                OutputWindow window = new OutputWindow(r.ReadToEnd())
                {
                    Owner = this,
                    Title = "Selected Result Node"
                };
                var result = window.ShowDialog();
            }
        }

        private void treeViewShowJSONInOutputWindow_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjectGraph.SelectedItem != null)
            {
                var item = (ViewModel.ObjectViewModel)treeViewObjectGraph.SelectedItem;
                OutputWindow window = new OutputWindow(JsonConvert.SerializeObject(item.Object, Formatting.Indented))
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
        #endregion
        #endregion

        #region API
        #region API-Helpers
        private bool IsJSON()
        {
            return AppInstance?.Settings?.ResponseType == Model.ResponseType.JSON;
        }

        private void doApiRequest(string requestname, string apisource, Func<object[], object> apiCallFunc, ApiCallParameter[] parameterTypes = null)
        {
            bool hasParameter = parameterTypes != null && parameterTypes.Any();

            List<object> parameters = new List<object>();
            if (hasParameter)
            {
                if (parameterTypes != null && parameterTypes.Any())
                {
                    foreach (var parameterType in parameterTypes)
                    {
                        switch (parameterType.Type)
                        {
                            case ParameterTypeEnum.String: parameters.Add(GetInput(parameterType)); break;
                            case ParameterTypeEnum.Folder: parameters.Add(GetFolderBrowserDialog(parameterType)); break;
                            case ParameterTypeEnum.File: parameters.Add(GetFileBrowserDialog(parameterType)); break;
                        }
                    }
                }
            }

            if (!hasParameter || (hasParameter && parameters != null && !parameters.Any(x =>  x == null)))
            {
                object detail = null;
                ViewModel.ResponseItem item = null;
                Exception ex = null;
                var statusWindow = new StatusWindow(3)
                {
                    Owner = this
                };
                statusWindow.Start(() =>
                {
                    try
                    {
                        statusWindow.Update(1, "Creating Request");
                        Invoke(this, () => item = AppInstance.Add(requestname, apisource, string.Join("; ", parameters)));
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
                    {
                        Invoke(this, () => ShowException(ex));
                    }
                    else
                    {
                        Invoke(this, () => {
                            item.AddData(new[] { detail });
                            if (item.Data != null && item.Data.Any())
                            {
                                datagridResponse.SelectedIndex = 0;
                            }
                        });
                    }
                });
            }
            else if (hasParameter && (parameters == null || !parameters.Any() || parameters.Any(x => x == null)))
            {
                MessageBox.Show("No parameters supplied or missing parameters", "Error", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
            }
        }
        #endregion

        #region SK-Init
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
        #endregion

        #region CZ-Init
        private CZ::FinstatApi.ApiClient CreateCZApiClient()
        {

            return new CZ::FinstatApi.ApiClient("https://cz.finstat.sk/api", AppInstance.Settings.ApiKeys.PublicKey, AppInstance.Settings.ApiKeys.PrivateKey, AppInstance.Settings.StationID, AppInstance.Settings.StationName, AppInstance.Settings.TimeOut);
        }

        private CZ::FinstatApi.ApiMonitoringClient CreateCZApiMonitoringClient()
        {
            return new CZ::FinstatApi.ApiMonitoringClient("https://cz.finstat.sk/api", AppInstance.Settings.ApiKeys.PublicKey, AppInstance.Settings.ApiKeys.PrivateKey, AppInstance.Settings.StationID, AppInstance.Settings.StationName, AppInstance.Settings.TimeOut);
        }
        #endregion

        #region SK-Detail
        private void buttonDetail_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Detail", "SK", (parameters) =>
            {
                FinstatApi.ApiClient client = CreateSKApiClient();
                var result = client.RequestDetail((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO") }
            );
        }

        private void buttonExtended_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Extended", "SK", (parameters) =>
            {
                var client = CreateSKApiClient();
                var result = client.RequestExtendedDetail((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonUltimate_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("Ultimate", "SK", (parameters) =>
            {
                var client = CreateSKApiClient();
                var result = client.RequestUltimateDetail((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }
        #endregion

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
                new ApiCallParameter(ParameterTypeEnum.String, "FinStat URL")
            });
        }
        #endregion

        #region Sk-MonitoringICO
        private void buttonMonitoringIcoAdd_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOAdd", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.Add((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonMonitoringIcoRemove_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICORemove", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.Remove((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonMonitoringIcoList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOList", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetMonitorings(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonMonitoringIcoReport_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOReport", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetReport(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonMonitoringIcoProceedings_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOProceedings", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetProceedings(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }
        #endregion

        #region SK-MonitoringDate
        private void buttonMonitoringDateAdd_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateAdd", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.AddDate((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "Date")
            });
        }

        private void buttonMonitoringDateRemove_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateRemove", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.RemoveDate((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "Date")
            });
        }

        private void buttonMonitoringDateList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateList", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetDateMonitorings(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonMonitoringDateReport_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateReport", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetDateReport(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonMonitoringDateProceedings_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringDateProceedings", "SK", (parameters) =>
            {
                var client = CreateSKApiMonitoringClient();
                var result = client.GetDateProceedings(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }
        #endregion

        #region SK-DailyDiff
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
        #endregion

        #region SK-DailyStatementDiff
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
            doApiRequest("DailyStatementDiffFile", "SK", (parameters) =>
            {
                var client = CreateSKApiDailyStatementDiffClient();
                var result = client.RequestStatementLegend();
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }
        #endregion

        #region SK-DailyUltimateDiff
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
        #endregion

        #region CZ-Detail
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
        #endregion

        #region CZ-Auto
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
                return client.RequestAutoLogin((string)parameters[0]);
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "FinStat URL")
            });
        }
        #endregion

        #region CZ-Monitoring
        private void buttonCZMonitoringIcoAdd_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOAdd", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.Add((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonCZMonitoringIcoRemove_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICORemove", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.Remove((string)parameters[0], IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            }, new[] {
                new ApiCallParameter(ParameterTypeEnum.String, "IČO")
            });
        }

        private void buttonCZMonitoringIcoList_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOList", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.GetMonitorings(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }

        private void buttonCZMonitoringIcoReport_Click(object sender, RoutedEventArgs e)
        {
            doApiRequest("MonitoringICOReport", "CZ", (parameters) =>
            {
                var client = CreateCZApiMonitoringClient();
                var result = client.GetReport(IsJSON());
                AppInstance.Limits.FromModel(client.Limits);
                return result;
            });
        }
        #endregion

        #endregion
    }
}
