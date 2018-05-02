using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public enum CreditScoreStateEnum
    {
        Red,
        Yellow,
        Green
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
}
