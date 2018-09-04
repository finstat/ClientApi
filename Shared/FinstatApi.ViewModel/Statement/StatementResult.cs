using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi.Statement
{
    public enum TemplateTypeEnum
    {
        Template2011v2,
        Template2014,
        Template2014micro,
        TemplateFinancial,
        TemplateROPO,
        TemplateNujPU
    }

    public class StatementItem
    {
        public int Year { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DatePublished { get; set; }
        public TemplateTypeEnum[] Templates { get; set; }
    }

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
        public TemplateTypeEnum Format { get; set; }
        public TemplateTypeEnum OriginalFormat { get; set; }
        public string Source { get; set; }

        public StatementValue[] Assets { get; set; }
        public StatementValue[] LiabilitiesAndEquity { get; set; }
        public StatementValue[] IncomeStatement { get; set; }
    }
}
