using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi.ViewModel.Diff
{
    public class StatementValue
    {
        public string Key { get; set; }
        public double? Actual { get; set; }
        public double? Previous { get; set; }
    }

    public class StatementResult
    {
        public string ICO { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DatePublished { get; set; }
        public string Format { get; set; }
        public string OriginalFormat { get; set; }
        public string Source { get; set; }

        public List<StatementValue> Assets { get; set; } = new List<StatementValue>();
        public List<StatementValue> LiabilitiesAndEquity { get; set; } = new List<StatementValue>();
        public List<StatementValue> Income { get; set; } = new List<StatementValue>();

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result
                .AppendLine(string.Format("ICO: {0}", ICO))
                .AppendLine(string.Format("Name: {0}", Name))
                .AppendLine(string.Format("Year: {0}", Year))
                .AppendLine(string.Format("Date From: {0:dd.MM.yyyy}", DateFrom))
                .AppendLine(string.Format("Date To: {0:dd.MM.yyyy}", DateTo))
                .AppendLine(string.Format("Date Published: {0:dd.MM.yyyy}", DatePublished))
                .AppendLine(string.Format("Format: {0}", Format))
                .AppendLine(string.Format("Original Format: {0}", OriginalFormat))
                .AppendLine(string.Format("Source: {0}", Source))
                .AppendLine(string.Format("Assets Count: {0}", Assets.Count))
                .AppendLine(string.Format("LiabilitiesAndEquity Count: {0}", LiabilitiesAndEquity.Count))
                .AppendLine(string.Format("Income Count: {0}", Income.Count))
            ;
            return result.ToString();
        }
    }
}
