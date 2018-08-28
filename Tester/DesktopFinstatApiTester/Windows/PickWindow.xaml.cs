using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopFinstatApiTester.Windows
{
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class PickWindow : Window
    {
        public PickWindow(IEnumerable<object> values)
        {
            InitializeComponent();
            comboBoxInput.ItemsSource = values;
        }

        public object Value
        {
            get
            {
                return comboBoxInput.SelectedItem;
            }
            set
            {
                comboBoxInput.SelectedItem = value;
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
