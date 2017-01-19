using System;
using System.Text;

namespace FinstatApi
{
    public class DetailResult : BaseResult
    {
        public enum RevenueTypeEnum
        {
            Unknown,
            Up,
            Down
        }

        public enum ProfitTypeEnum
        {
            Unknown,
            Up,
            Down,
            Loss
        }

        public RevenueTypeEnum Revenue { get; set; }
        public ProfitTypeEnum Profit { get; set; }
        public override string ToString()
        {
            StringBuilder dataString = new StringBuilder();
            dataString.Append(base.ToString());
            dataString.AppendLine(string.Format("Profit: {0}", Profit));
            dataString.AppendLine(string.Format("Revenue: {0}", Revenue));

            return dataString.ToString();
        }
    }
}