using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi.ViewModel.Diff
{
    public class BaseResult
    {
        public string Ico { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Activity { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Cancelled { get; set; }
        public string Url { get; set; }
        public bool Warning { get; set; }
        public string WarningUrl { get; set; }
        public bool OrChange { get; set; }
        public string OrChangeUrl { get; set; }
        public string RegisterNumberText { get; set; }
        public string Dic { get; set; }
        public string IcDPH { get; set; }
        public bool SuspendedAsPerson { get; set; }
        public bool PaymentOrderWarning { get; set; }
        public string PaymentOrderUrl { get; set; }
    }
}
