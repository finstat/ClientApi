using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace DesktopFinstatApiTester.ViewModel
{
    public class ApiApplication : ViewModel
    {
        private const string fileName = @"application.json";
        public const string SettingsProperty = "SettingsKey";
        public const string ResponseItemsProperty = "ResponseItems";
        public const string ApplicationObjectProperty = "ApplicationObject";

        public ApiApplication()
        {
            PropertyChanged += ApiApplication_PropertyChanged;
            Settings = new Model.Settings();
            Load();
        }

        public Model.Settings Settings { get; set; }

        private ObservableCollection<ResponseItem> _items = new ObservableCollection<ResponseItem>();
        public ObservableCollection<ResponseItem> ResponseItems
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(ResponseItemsProperty); }
        }

        internal ResponseItem Add()
        {
            return Add(string.Empty);
        }

        internal ResponseItem Add(string request)
        {
            return Add(request, string.Empty);
        }

        internal ResponseItem Add(string request, string parameter)
        {
            return Add(request, "SK", parameter);
        }

        internal ResponseItem Add(string request, string apisource, string parameter)
        {
            return Add(new ResponseItem(request, apisource, parameter));
        }



        internal ResponseItem Add(ResponseItem item)
        {
            if (ResponseItems != null)
            {
                ResponseItems.Insert(0, item);
                RaisePropertyChanged(ResponseItemsProperty);
                return item;
            }
            return null;
        }

        internal ResponseItem RemoveAt(int index)
        {
            if (index < ResponseItems.Count)
            {
                ResponseItem result = ResponseItems[index];
                ResponseItems.RemoveAt(index);
                return result;
            }
            return null;
        }

        public void Load()
        {
            if (File.Exists(fileName))
            {
                using (StreamReader file = File.OpenText(fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Settings = (Model.Settings)serializer.Deserialize(file, typeof(Model.Settings));
                }
            }
        }

        public void Save()
        {
            using (StreamWriter file = File.CreateText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Settings);
            }
        }

        private void ApiApplication_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (new[] { SettingsProperty }.Contains(e.PropertyName))
            {
                RaisePropertyChanged(ApplicationObjectProperty);
            }
        }
    }
}
