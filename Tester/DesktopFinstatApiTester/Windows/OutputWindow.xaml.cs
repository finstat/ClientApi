﻿using System;
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
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {
        public string Text = null;
        public string OutputText
        {
            get
            {
                return textBoxOutput.Text;
            }
            set
            {
                textBoxOutput.Text = value;
            }
        }

        public OutputWindow(string text)
        {
            InitializeComponent();
            Text = text;
            RefreshTextBox();
        }

        private void RefreshTextBox()
        {
            OutputText = Text;
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTextBox();
        }

        private void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Text);
        }
    }
}
