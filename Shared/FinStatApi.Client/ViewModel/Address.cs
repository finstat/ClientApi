using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class Address
    {
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string District { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("Street: {0} {1}", Street, StreetNumber));
            dataString.AppendLine(string.Format("ZIP: {0}", ZipCode));
            dataString.AppendLine(string.Format("City: {0}", City));
            dataString.AppendLine(string.Format("Country: {0}", Country));
            dataString.AppendLine(string.Format("District: {0}", District));
            dataString.AppendLine(string.Format("Region: {0}", Region));
            return dataString.ToString();
        }
    }

    public class FullAddress : Address
    {
        public string Name { get; set; }

        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.AppendLine(string.Format("Name: {0}", Name));
            dataString.AppendLine(base.ToString());
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

    public enum CreditScoreStateEnum
    {
        Red,
        Yellow,
        Green
    }
}
