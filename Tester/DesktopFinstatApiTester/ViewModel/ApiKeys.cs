using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DesktopFinstatApiTester.Model;

namespace DesktopFinstatApiTester.ViewModel
{
    public class ApiKeys : ViewModel, IModelViewModel<Model.ApiKeys>
    {
        public const string PublicKeyProperty = "PublicKey";
        public const string PrivateKeyProperty = "PrivateKey";
        public const string ApiKeysObjectProperty = "ApiKeysObject";


        public ApiKeys()
        {
            PropertyChanged += ApiKeys_PropertyChanged;
        }

        private string _publicKey;
        public string PublicKey
        {
            get { return _publicKey; }
            set
            {
                if (_publicKey != value)
                {
                    _publicKey = value;
                    RaisePropertyChanged(PublicKeyProperty);
                }
            }
        }

        private string _privateKey;
        public string PrivateKey
        {
            get { return _privateKey; }
            set
            {
                if (_privateKey != value)
                {
                    _privateKey = value;
                    RaisePropertyChanged(PrivateKeyProperty);
                }
            }
        }

        private void ApiKeys_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (new[] { PublicKeyProperty, PrivateKeyProperty }.Contains(e.PropertyName))
            {
                RaisePropertyChanged(ApiKeysObjectProperty);
            }
        }

        public void FromModel(Model.ApiKeys model)
        {
            if(model != null)
            {
                PublicKey = model.PublicKey;
                PrivateKey = model.PrivateKey;
            }
        }

        public Model.ApiKeys ToModel(Model.ApiKeys model)
        {
            if (model == null)
            {
                model = new Model.ApiKeys();
            }
            model.PrivateKey = PrivateKey;
            model.PublicKey = PublicKey;
            return model;
        }
    }
}
