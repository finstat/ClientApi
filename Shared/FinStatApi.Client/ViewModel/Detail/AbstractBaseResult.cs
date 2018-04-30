using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class AbstractBaseResult : FullAddress
    {
        public string Ico { get; set; }
        public string IcDPH { get; set; }
        public string Activity { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Cancelled { get; set; }
        public string Url { get; set; }

        public bool Warning { get; set; }
        public string WarningUrl { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("ICO: {0}", Ico));
            dataString.AppendLine(string.Format("Name: {0} in {1}", Name, Activity));
            dataString.AppendLine(string.Format("IC DPH: {0}", IcDPH));
            dataString.AppendLine(string.Format("Created: {0}", Created));
            dataString.AppendLine(string.Format("Canceled: {0}", Cancelled));
            dataString.AppendLine(string.Format("URL: {0}", Url));
            dataString.AppendLine(string.Format("Warning: {0}", Warning + " " + WarningUrl));

            return dataString.ToString();
        }
    }
}
