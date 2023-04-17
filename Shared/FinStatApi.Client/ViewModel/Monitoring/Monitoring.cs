using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace FinstatApi
{
    public class AbstractMonitoring
    {
        public string Ident { get; set; }
        public string Name { get; set; }
        public DateTime? PublishDate { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string[] Categories { get; set; }

        public override string ToString()
        {
            return string.Format("{0} for {4} at {1} more info {2}\nDesc:{3}\nCategories:{4}", Type, PublishDate, Url, Description, Name, string.Join(",", Categories ?? new string[0]));
        }
    }

    public class Monitoring : AbstractMonitoring
    {
        public string Ico { get; set; }
    }

    public class MonitoringDate : AbstractMonitoring
    {
        public string Date { get; set; }
    }

    public class MonitoringCategory
    {
        public string Category { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Category, Name);
        }
    }
}
