using System;

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
            return string.Format("{0} {1}, {2} {3}, {4}, {5}, {6}", Street, StreetNumber, ZipCode, City, Country, District, Region);
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
    }
}