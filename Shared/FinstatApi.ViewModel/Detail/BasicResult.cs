using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class BasicResult : AbstractResult
    {
        public string Dic { get; set; }
        public string IcDPH { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();

            dataString.AppendLine(base.ToString());
            dataString.AppendLine(string.Format("Dic: {0}", Dic));
            dataString.AppendLine(string.Format("IcDPH: {0}", IcDPH));
            return dataString.ToString();
        }
    }
}
