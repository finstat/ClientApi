using System;
using System.Text;

namespace FinstatApi
{
    public class Address
    {
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string District { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("Street: {0} {1}", Street, StreetNumber));
            dataString.AppendLine(string.Format("ZIP: {0}", ZipCode));
            dataString.AppendLine(string.Format("City: {0}", City));
            dataString.AppendLine(string.Format("Country: {0}",Country));
            dataString.AppendLine(string.Format("District: {0}", District));
            dataString.AppendLine(string.Format("Region: {0}", Region));
            return dataString.ToString();
        }
    }

    public class BaseResult :Address
    {
        public string Name { get; set; }
        public string Ico { get; set; }
        public string RegisterNumberText { get; set; }
        public string Dic { get; set; }
        public string IcDPH { get; set; }
        public string Activity { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Cancelled { get; set; }
        public bool SuspendedAsPerson { get; set; }
        public string Url { get; set; }

        public bool Warning { get; set; }
        public string WarningUrl { get; set; }
        public bool PaymentOrderWarning { get; set; }
        public string PaymentOrderUrl { get; set; }
        public bool OrChange { get; set; }
        public string OrChangeUrl { get; set; }

        public string SkNaceCode { get; set; }
        public string SkNaceText { get; set; }
        public string SkNaceDivision { get; set; }
        public string SkNaceGroup { get; set; }
        public string LegalFormCode { get; set; }
        public string LegalFormText { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("ICO: {0}", Ico));
            dataString.AppendLine(string.Format("Name: {0}{1} in {2}", Name, SuspendedAsPerson ? "[pozastavená]" : null, Activity));
            dataString.AppendLine(string.Format("LegalForm: {0} {1}",LegalFormCode, LegalFormText));
            dataString.AppendLine(string.Format("Register Number: {0}", RegisterNumberText));
            dataString.AppendLine(string.Format("DIC: {0}", Dic));
            dataString.AppendLine(string.Format("IC DPH: {0}", IcDPH));
            dataString.AppendLine(string.Format("Register Number: {0}", RegisterNumberText));
            dataString.AppendLine(string.Format("SK Nace: {0}", SkNaceCode + "  " + SkNaceText + " " + SkNaceDivision + " " + SkNaceGroup));
            dataString.AppendLine(base.ToString());
            dataString.AppendLine(string.Format("Created: {0}", Created));
            dataString.AppendLine(string.Format("Canceled: {0}", Cancelled));
            dataString.AppendLine(string.Format("URL: {0}", Url));
            dataString.AppendLine(string.Format("Warning: {0}", Warning + " " + WarningUrl));
            dataString.AppendLine(string.Format("Payment order warning: {0}", PaymentOrderWarning + " " + PaymentOrderUrl));
            dataString.AppendLine(string.Format("OrChange: {0}", OrChange + " " + OrChangeUrl));
                      
            return dataString.ToString();
        }
    }
}