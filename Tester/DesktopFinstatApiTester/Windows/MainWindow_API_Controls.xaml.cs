using DesktopFinstatApiTester.ViewModel;
using FinstatApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;

namespace DesktopFinstatApiTester.Windows
{
    public partial class MainWindow : Fluent.RibbonWindow
    {
        #region Control-DataGridResponse
        private void datagridResponse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            treeViewObjectGraph.ItemsSource = null;
            treeViewHeadersGraph.ItemsSource = null;
            var grid = (DataGrid)sender;
            if (grid.SelectedItems != null && grid.SelectedItems.Count > 0)
            {
                var item = (ViewModel.ResponseItem)grid.SelectedItem;
                if (item != null)
                {
                    var graph = new ViewModel.ObjectViewModelHierarchy((item.DataCount > 0) ? item.Data[0] : null);
                    treeViewObjectGraph.ItemsSource = graph.FirstGeneration;
                    graph.FirstGeneration[0].IsSelected = true;
                    graph.FirstGeneration[0].IsExpanded = true;

                    var graph2 = new ViewModel.ObjectViewModelHierarchy(new BasicResponse
                    {
                        RequestHeaders = item.RequestHeaders,
                        ResponseHeaders = item.ResponseHeaders,
                    });
                    treeViewHeadersGraph.ItemsSource = graph2.FirstGeneration;
                    graph2.FirstGeneration[0].IsSelected = true;
                    graph2.FirstGeneration[0].IsExpanded = true;
                }
            }
        }

        private void dataGridShowResponseOutputWindow_Click(object sender, RoutedEventArgs e)
        {
            if (datagridResponse.SelectedItems != null && datagridResponse.SelectedItems.Count > 0)
            {
                var item = (ViewModel.ResponseItem)datagridResponse.SelectedItem;
                if (item != null && item.Content != null && item.Content.Any())
                {
                    OutputWindow window = new OutputWindow(Encoding.UTF8.GetString(item.Content))
                    {
                        Owner = this,
                        Title = "Response body"
                    };
                    var result = window.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No Response for this request", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                }
            }
        }

        private void dataGridCurlCommand_Click(object sender, RoutedEventArgs e)
        {
            if (datagridResponse.SelectedItems != null && datagridResponse.SelectedItems.Count > 0)
            {
                var item = (ViewModel.ResponseItem)datagridResponse.SelectedItem;
                if (item != null && item.Request == "Basic")
                {
                    var parameters = item.Parameter?.Split(';').Select(x => x.Trim());
                    var hashParam = !string.IsNullOrEmpty(item.Parameter.Trim())
                        ? string.Join((item.Request != "DistraintDetail") ? "!" : "", parameters.Take(2))
                        : null;
                    var userApiKey = AppInstance.Settings.ApiKeys.PublicKey;
                    var privateKey = AppInstance.Settings.ApiKeys.PrivateKey;
                    var calculatedHash = CommonAbstractApiClient.ComputeVerificationHash(userApiKey, privateKey, hashParam);

                    var url = (item.ApiSource == "CZ")
                        ? AppInstance.Settings.FinStatApiUrlCZ
                        : AppInstance.Settings.FinStatApiUrl;
                    url = url.TrimEnd('/');

                    StringBuilder str = new StringBuilder();
                    str.AppendLine($"curl '{url}/detail.json?ico={parameters.FirstOrDefault()}&apiKey={userApiKey}&hash={calculatedHash}&StationId=curl-test&StationName='curl-test' -v'");
                    str.AppendLine();
                    str.AppendLine($"curl -X POST '{url}/detail.json' -d 'ico={parameters.FirstOrDefault()}&apiKey={userApiKey}&hash={calculatedHash}&StationId=curl-test&StationName='curl-test' -v ");
                    //str.AppendLine();
                    // str.AppendLine($"curl -H \"Content-Type: application/json\" '{url}/api/autocomplete.json' -d '{{Query:\\\"{parameters.FirstOrDefault()}\\\", ApiKey:\\\"{userApiKey}\\\", Hash:\\\"{calculatedHash}\\\", StationId:\\\"curl-test\\\", StationName:\\\"curl-test\\\"}}' -v");
                    OutputWindow window = new OutputWindow(str.ToString())
                    {
                        Owner = this,
                        Title = "Curl"
                    };
                    var result = window.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Nothing selected", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                }
            }
        }

        private void dataGridRepeatRequest_Click(object sender, RoutedEventArgs e)
        {
            if (datagridResponse.SelectedItems != null && datagridResponse.SelectedItems.Count > 0)
            {
                var item = (ViewModel.ResponseItem)datagridResponse.SelectedItem;
                if (item != null)
                {
                    // TODO helper for api call according item.Request
                    //doApiRequest(item.Request, item.ApiSource)
                }
                else
                {
                    MessageBox.Show("Nothing selected", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
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
            if (treeViewObjectGraph.SelectedItem != null)
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

        private void treeViewShowTreeInOutputWindow_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjectGraph.SelectedItem != null)
            {
                var item = (ViewModel.ObjectViewModel)treeViewObjectGraph.SelectedItem;
                OutputWindow window = new OutputWindow(item.ToString())
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
    }
}
