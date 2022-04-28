using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class AbstractResult : FullAddress
    {
        public string Ico { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("ICO: {0}", Ico));
            dataString.AppendLine(base.ToString());
            dataString.AppendLine(string.Format("URL: {0}", Url));

            return dataString.ToString();
        }
    }
}
