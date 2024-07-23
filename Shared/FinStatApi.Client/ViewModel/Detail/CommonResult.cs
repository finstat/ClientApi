using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class CommonResult : AbstractResult
    {
        public string Activity { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Cancelled { get; set; }
        public bool Warning { get; set; }
        public string WarningUrl { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(base.ToString());
            dataString.AppendLine(string.Format("Activity: {0}", Activity));
            dataString.AppendLine(string.Format("Created: {0}", Created));
            dataString.AppendLine(string.Format("Canceled: {0}", Cancelled));
            dataString.AppendLine(string.Format("Warning: {0}", Warning + " " + WarningUrl));

            return dataString.ToString();
        }
    }
}
