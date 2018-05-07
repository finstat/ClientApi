using DesktopFinstatApiTester.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DesktopFinstatApiTester.ViewModel
{
    public class Settings : ViewModel, IModelViewModel<Model.Settings>
    {
        public const string ApiKeysProperty = "ApiKeys";
        public const string ResponseTypeProperty = "ResponseType";
        public const string TimeOutProperty = "TimeOut";
        public const string StationIDProperty = "StationID";
        public const string StationNameProperty = "StationName";
        public const string SettingsObjectProperty = "SettingsObject";

        public Settings()
        {
            PropertyChanged += Settings_PropertyChanged;
            ApiKeys = new ApiKeys();
            FromModel(new Model.Settings());
        }

        private ApiKeys _apiKeys;
        public ApiKeys ApiKeys
        {
            get { return _apiKeys; }
            set
            {
                if (_apiKeys != value)
                {
                    if(_apiKeys != null)
                    {
                        _apiKeys.PropertyChanged -= _apiKeys_PropertyChanged;
                    }
                    _apiKeys = value;
                    _apiKeys.PropertyChanged += _apiKeys_PropertyChanged;

                }
            }
        }

        private int _timeOut;
        public int TimeOut
        {
            get { return _timeOut; }
            set
            {
                if (_timeOut != value)
                {
                    _timeOut = value;
                    RaisePropertyChanged(TimeOutProperty);
                }
            }
        }

        private string _stationName;
        public string StationName
        {
            get { return _stationName; }
            set
            {
                if (_stationName != value)
                {
                    _stationName = value;
                    RaisePropertyChanged(StationNameProperty);
                }
            }
        }

        private string _stationID;
        public string StationID
        {
            get { return _stationID; }
            set
            {
                if (_stationID != value)
                {
                    _stationID = value;
                    RaisePropertyChanged(StationIDProperty);
                }
            }
        }

        private ResponseType _responseType;
        public ResponseType ResponseType
        {
            get { return _responseType; }
            set
            {
                if (_responseType != value)
                {
                    _responseType = value;
                    RaisePropertyChanged(ResponseTypeProperty);
                }
            }
        }

        private void _apiKeys_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(new[] { ApiKeys.ApiKeysObjectProperty }.Contains(e.PropertyName))
            {
                RaisePropertyChanged(ApiKeysProperty);
            }
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (new[] { TimeOutProperty, StationNameProperty, StationIDProperty, ApiKeysProperty, ResponseTypeProperty }.Contains(e.PropertyName))
            {
                RaisePropertyChanged(SettingsObjectProperty);
            }
        }

        public void FromModel(Model.Settings model)
        {
            if (model != null)
            {
                ApiKeys.FromModel(model.ApiKeys);
                ResponseType = model.ResponseType;
                TimeOut = model.TimeOut;
                StationName = model.StationName;
                StationID = model.StationID;
            }
        }

        public Model.Settings ToModel(Model.Settings model)
        {
            if (model == null)
            {
                model = new Model.Settings();
            }
            model.ApiKeys = ApiKeys.ToModel(model.ApiKeys);
            model.ResponseType = ResponseType;
            model.StationName = StationName;
            model.StationID = StationID;
            model.TimeOut = TimeOut;
            return model;
        }
    }
}
