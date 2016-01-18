using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class Monitoring
    {
        public string Ident { get; set; }
        public string Ico { get; set; }
        public string Name { get; set; }
        public DateTime? PublishDate { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return string.Format("{0} for {4}[{5}] at {1} more info {2}\nDesc:{3}", Type, PublishDate, Url, Description, Name, Ico);
        }
    }
}
