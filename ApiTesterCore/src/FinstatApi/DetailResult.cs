using System;

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
            return string.Format(
                "Ico: {0}, Name: {1}{10} in {11}\n Register Number: {8}\n SK Nace: {12}\n City: {2}\n Created: {3}\n Warning: {4}\n Payment order warning: {9}\n OrChange: {5}\n Revenue: {6}\n Profit: {7}",
                Ico, Name, City, Created, Warning, OrChange, Revenue, Profit, RegisterNumberText, PaymentOrderWarning, SuspendedAsPerson ? "[pozastavená]" : null, Activity, SkNaceCode + "  " + SkNaceText);
        }
    }
}