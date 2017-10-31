using System;
using System.Collections.Generic;
using System.Linq;
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

    public class FullAddress : Address
    {
        public string Name { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("Name: {0}", Name));
            dataString.AppendLine(base.ToString());
            return dataString.ToString();
        }
    }

    public class BaseResult : FullAddress
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
        public string RpvsInsert { get; set; }
        public string RpvsUrl { get; set; }

        public string SalesCategory { get; set; }
        public IcDphAdditonalData IcDphAdditional { get; set; }
        public double? ProfitActual { get; set; }
        public double? RevenueActual { get; set; }
        public JudgementIndicator[] JudgementIndicators { get; set; }
        public string JudgementFinstatLink { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("ICO: {0}", Ico));
            dataString.AppendLine(string.Format("Name: {0}{1} in {2}", Name, SuspendedAsPerson ? "[pozastavená]" : null, Activity));
            dataString.AppendLine(string.Format("LegalForm: {0} {1}",LegalFormCode, LegalFormText));
            dataString.AppendLine(string.Format("Register Number: {0}", RegisterNumberText));
            dataString.AppendLine(string.Format("DIC: {0}", Dic));
            dataString.AppendLine(string.Format("IC DPH: {0}", IcDPH));
            dataString.AppendLine(string.Format("IcDphAdditional: {0}", IcDphAdditional?.ToString()));
            dataString.AppendLine(string.Format("RpvsInsert: {0} {1}", RpvsInsert, RpvsUrl));
            dataString.AppendLine(string.Format("SalesCategory: {0}", SalesCategory));
            dataString.AppendLine(string.Format("Register Number: {0}", RegisterNumberText));
            dataString.AppendLine(string.Format("SK Nace: {0}", SkNaceCode + "  " + SkNaceText + " " + SkNaceDivision + " " + SkNaceGroup));
            dataString.AppendLine(base.ToString());
            dataString.AppendLine(string.Format("ProfitActual: {0}", ProfitActual));
            dataString.AppendLine(string.Format("RevenueActual: {0}", RevenueActual));
            dataString.AppendLine(string.Format("Created: {0}", Created));
            dataString.AppendLine(string.Format("Canceled: {0}", Cancelled));
            dataString.AppendLine(string.Format("URL: {0}", Url));
            dataString.AppendLine(string.Format("Warning: {0}", Warning + " " + WarningUrl));
            dataString.AppendLine(string.Format("Payment order warning: {0}", PaymentOrderWarning + " " + PaymentOrderUrl));
            dataString.AppendLine(string.Format("OrChange: {0}", OrChange + " " + OrChangeUrl));
            dataString.AppendLine(string.Format("JudgementFinstatLink: {0}", JudgementFinstatLink));
            dataString.AppendLine(string.Format("JudgementIndicators: [{0}]", (JudgementIndicators != null) ? string.Join(",", JudgementIndicators.Select(x => x.ToString())) : null));

            return dataString.ToString();
        }

        public class IcDphAdditonalData
        {
            public string IcDph { get; set; }
            public string Paragraph { get; set; }
            public DateTime? CancelListDetectedDate { get; set; }
            public DateTime? RemoveListDetectedDate { get; set; }
            public override string ToString()
            {
                StringBuilder dataString = new StringBuilder();
                dataString.AppendFormat("IcDph: {0} ", IcDph);
                dataString.AppendFormat("{0}", Paragraph);
                dataString.AppendFormat("{0}", CancelListDetectedDate != null ? "[zoznam s dovodom na zrušenie]" : null);
                dataString.AppendFormat("{0}", RemoveListDetectedDate != null ? "[zoznam vymazaných]" : null);
                return dataString.ToString();
            }
        }

        public class JudgementIndicator
        {
            public string Name { get; set; }
            public bool? Value { get; set; }

            public override string ToString()
            {
                return string.Format("{0}:{1}", Name, Value);
            }
        }
    }
}