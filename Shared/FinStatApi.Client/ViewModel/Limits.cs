using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi.ViewModel
{
    public class Limit
    {
        public long Current { get; set; }
        public long Max { get; set; }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Current, Max);
        }
    }

    public class Limits
    {
        public Limit Daily { get; set; }
        public Limit Monthly { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result
                .AppendLine(String.Format("Daily: {0}", Daily))
                .AppendLine(String.Format("Monthly: {0}", Monthly));

            return result.ToString();
        }
    }
}
