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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopFinstatApiTester.Controls
{
    /// <summary>
    /// Interaction logic for ApiKeysControl.xaml
    /// </summary>
    public partial class ApiKeysControl : UserControl, IAppControl
    {
        public ApiKeysControl()
        {
            InitializeComponent();
        }

        public IDictionary<string, object[]> GetErrors()
        {
            if (!IsValid())
            {
                var result = new Dictionary<string, object[]>();
                var errors = Validation.GetErrors(textBoxPublicKey);

                if (errors != null && errors.Any())
                {
                    result.Add("PublicKey", errors.Select(x => x.ErrorContent).ToArray());
                }
                else if (string.IsNullOrEmpty(textBoxPublicKey.Text.Trim()))
                {
                    result.Add("PublicKey", new[] { "Value is empty" });
                }
                errors = Validation.GetErrors(textBoxPrivateKey);
                if (errors != null && errors.Any())
                {
                    result.Add("PrivateKey", errors.Select(x => x.ErrorContent).ToArray());
                }
                if (result.Any(x => x.Value != null && x.Value.Any()))
                {
                    return result;
                }
            }
            return null;
        }

        public bool IsValid()
        {
            if (Validation.GetHasError(textBoxPublicKey) || string.IsNullOrEmpty(textBoxPublicKey.Text.Trim()))
            {
                return false;
            }
            if (Validation.GetHasError(textBoxPrivateKey))
            {
                return false;
            }
            return true;
        }
    }
}
