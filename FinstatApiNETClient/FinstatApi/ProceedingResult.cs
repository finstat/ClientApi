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

    public class IssuedPerson
    {
        public string Name { get; set; }
        public string Function { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("Name: {0} - {1}", Name, Function));
            return dataString.ToString();
        }
    }

    public class Deadline
    {
        public string Type { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("DeadLine: {0} - {1}", Date, Type));
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
        public string EndStatus { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public DateTime PublishDate { get; set; }
        public Deadline[] DatesOfProceeding { get; set; }
        public string[] FileIdentifierNumber { get; set; }
        public IssuedPerson IssuedBy { get; set; }
        public string PostedBy { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("DebtorsAddress Count:{0}", DebtorsAddress != null ? DebtorsAddress.Length : 0));
            dataString.AppendLine(string.Format("ProposersAddress: {0}", ProposersAddress != null ? ProposersAddress.Length : 0));
            dataString.AppendLine(string.Format("AdministratorsAddress Count: {0}", AdministratorsAddress != null ? AdministratorsAddress.Length : 0));
            dataString.AppendLine(string.Format("CourtsAddress: {0}", CourtsAddress));
            dataString.AppendLine(string.Format("ReferenceFileNumber: {0}", ReferenceFileNumber));
            dataString.AppendLine(string.Format("EndReason: {0}", EndReason));
            dataString.AppendLine(string.Format("EndStatus: {0}", EndStatus));
            dataString.AppendLine(string.Format("Status: {0}", Status));
            dataString.AppendLine(string.Format("Character: {0}", Character));
            dataString.AppendLine(string.Format("Url: {0}", Url));
            dataString.AppendLine(string.Format("Type: {0}", Type));
            dataString.AppendLine(string.Format("PublishDate: {0}", PublishDate));
            dataString.AppendLine(string.Format("DatesOfProceeding: {0}", (DatesOfProceeding != null && DatesOfProceeding.Length > 0) ? DatesOfProceeding.Length : 0));
            dataString.AppendLine(string.Format("FileIdentifierNumber: {0}", (FileIdentifierNumber != null) ? string.Join(", ", FileIdentifierNumber) : null));
            dataString.AppendLine(string.Format("IssuedBy: {0}", IssuedBy));
            dataString.AppendLine(string.Format("PostedBy: {0}", PostedBy));
            return dataString.ToString();
        }
    }
}
