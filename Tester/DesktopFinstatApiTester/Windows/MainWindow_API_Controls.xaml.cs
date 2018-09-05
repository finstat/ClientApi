using DesktopFinstatApiTester.ViewModel;
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

                    textBoxRespones.Text = (item.Content != null && item.Content.Any()) ? Encoding.UTF8.GetString(item.Content) : null;
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
