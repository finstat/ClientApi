using DesktopFinstatApiTester.Model;
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
    /// Interaction logic for SettingControl.xaml
    /// </summary>
    public partial class SettingControl : UserControl, IAppControl
    {
        public SettingControl()
        {
            InitializeComponent();
            comboBoxResponseType.ItemsSource = Enum.GetValues(typeof(ResponseType)).Cast<ResponseType>();
        }

        public IDictionary<string, object[]> GetErrors()
        {
            if (!IsValid())
            {
                var result = new Dictionary<string, object[]>();
                var errors = Validation.GetErrors(comboBoxResponseType);
                if (errors != null && errors.Any())
                {
                    result.Add("ResponseType", errors.Select(x => x.ErrorContent).ToArray());
                }
                else if (comboBoxResponseType.SelectedIndex < 0)
                {
                    result.Add("ResponseType", new[] { "Value is empty" });
                }

                errors = Validation.GetErrors(textBoxStationName);
                if (errors != null && errors.Any())
                {
                    result.Add("StationName", errors.Select(x => x.ErrorContent).ToArray());
                }
                else if (string.IsNullOrEmpty(textBoxStationName.Text.Trim()))
                {
                    result.Add("StationName", new[] { "Value is empty" });
                }

                errors = Validation.GetErrors(textBoxStationID);
                if (errors != null && errors.Any())
                {
                    result.Add("StationID", errors.Select(x => x.ErrorContent).ToArray());
                }
                else if (string.IsNullOrEmpty(textBoxStationID.Text.Trim()))
                {
                    result.Add("StationID", new[] { "Value is empty" });
                }

                errors = Validation.GetErrors(textBoxTimeOut);
                if (errors != null && errors.Any())
                {
                    result.Add("StationTimeOut", errors.Select(x => x.ErrorContent).ToArray());
                }
                else if (string.IsNullOrEmpty(textBoxTimeOut.Text.Trim()))
                {
                    result.Add("StationTimeOut", new[] { "Value is empty" });
                }

                var controlErrors = controlApiKeys.GetErrors();
                if (controlErrors != null && controlErrors.Any(x => x.Value != null && x.Value.Any()))
                {
                    foreach (var kvp in controlErrors.Where(x => x.Value != null && x.Value.Any()))
                    {
                        result.Add(kvp.Key, kvp.Value);
                    }
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
            if (!controlApiKeys.IsValid())
            {
                return false;
            }
            if (Validation.GetHasError(textBoxStationName) || string.IsNullOrEmpty(textBoxStationName.Text))
            {
                return false;
            }
            if (Validation.GetHasError(textBoxStationID) || string.IsNullOrEmpty(textBoxStationID.Text))
            {
                return false;
            }
            if (Validation.GetHasError(textBoxTimeOut) || string.IsNullOrEmpty(textBoxTimeOut.Text))
            {
                return false;
            }
            if (Validation.GetHasError(comboBoxResponseType) || comboBoxResponseType.SelectedIndex < 0)
            {
                return false;
            }
            return true;
        }
    }
}
