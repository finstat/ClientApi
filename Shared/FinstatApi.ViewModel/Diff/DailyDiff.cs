using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class DailyDiff
    {
        public string FileName { get; set; }
        public DateTime GeneratedDate { get; set; }
        public int FileSize { get; set; }
        public DateTime? UploadDate { get; set; }


        public override string ToString()
        {
            return string.Format("File {0} generate at {1} with size {2} KB uploaded at {2}.", FileName, GeneratedDate.ToShortDateString(), FileSize / 1024, UploadDate);
        }
    }
}
