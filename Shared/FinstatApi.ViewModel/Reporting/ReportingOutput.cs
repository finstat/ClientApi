using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace FinstatApi.Reporting
{
    public class ReportOutput
    {
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Topic { get; set; }
        public string Group { get; set; }
        public int Count { get; set; }
        public DateTime? Date { get; set; }
    }
}