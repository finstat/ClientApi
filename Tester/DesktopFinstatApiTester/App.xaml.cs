using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace DesktopFinstatApiTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ViewModel.ApiApplication _instance = null;
        public ViewModel.ApiApplication Instance
        {
            get
            {
                if (_instance == null)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("sk-SK");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("sk-SK");

                    _instance = new ViewModel.ApiApplication();
                }
                return _instance;
            }
        }
    }
}
