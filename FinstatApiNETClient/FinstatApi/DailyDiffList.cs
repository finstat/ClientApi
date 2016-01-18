using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class DailyDiffList
    {
        public string Version { get; set; }

        public DailyDiff[] Files { get; set; }
    }
}
