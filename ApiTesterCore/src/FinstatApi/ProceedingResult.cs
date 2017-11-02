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
        public PersonAddress[] DebtorsAddress { get; set; }
        public PersonAddress[] ProposersAddress { get; set; }
        public PersonAddress[] AdministratorsAddress { get; set; }
        public FullAddress CourtsAddress { get; set; }
        public string ReferenceFileNumber { get; set; }
        public string Status { get; set; }
        public string Character { get; set; }
        public string EndReason { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public DateTime PublishDate { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("DebtorsAddress Count:{0}", DebtorsAddress != null ? DebtorsAddress.Length : 0));
            dataString.AppendLine(string.Format("ProposersAddress: {0}", ProposersAddress != null ? ProposersAddress.Length : 0));
            dataString.AppendLine(string.Format("AdministratorsAddress Count: {0}", AdministratorsAddress != null ? AdministratorsAddress.Length : 0));
            dataString.AppendLine(string.Format("CourtsAddress: {0}", CourtsAddress));
            dataString.AppendLine(string.Format("EndReason: {0}", EndReason));
            dataString.AppendLine(string.Format("Status: {0}", Status));
            dataString.AppendLine(string.Format("Character: {0}", Character));
            dataString.AppendLine(string.Format("Url: {0}", Url));
            dataString.AppendLine(string.Format("Type: {0}", Type));
            dataString.AppendLine(string.Format("PublishDate: {0}", PublishDate));
            return dataString.ToString();
        }
    }
}
