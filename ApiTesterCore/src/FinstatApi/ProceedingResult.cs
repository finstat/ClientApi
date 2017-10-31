using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class PersonAddress : FullAddress
    {
        public string Ico { get; set; }
        public string BirthDate { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("Ico: {0}", Ico));
            dataString.AppendLine(string.Format("BirthDate: {0}", BirthDate));
            dataString.AppendLine(base.ToString());
            return dataString.ToString();
        }
    }

    public class ProceedingResult
    {
        public PersonAddress[] Defendant { get; set; }
        public PersonAddress[] Proposer { get; set; }
        public PersonAddress[] Administrator { get; set; }
        public FullAddress Court { get; set; }
        public string ReferenceFileNumber { get; set; }
        public string State { get; set; }
        public string EndReason { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public DateTime PublishDate { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("Defendant Count: {0}", Defendant != null ? Defendant.Length : 0));
            dataString.AppendLine(string.Format("Proposer Count: {0}", Proposer != null ? Proposer.Length : 0));
            dataString.AppendLine(string.Format("Administrator Count: {0}", Administrator != null ? Administrator.Length : 0));
            dataString.AppendLine(string.Format("Court: {0}", Court));
            dataString.AppendLine(string.Format("EndReason: {0}", EndReason));
            dataString.AppendLine(string.Format("Url: {0}", Url));
            dataString.AppendLine(string.Format("Type: {0}", Type));
            dataString.AppendLine(string.Format("PublishDate: {0}", PublishDate));
            return dataString.ToString();
        }
    }
}
