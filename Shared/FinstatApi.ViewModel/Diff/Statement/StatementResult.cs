using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi.ViewModel.Diff.Statement
{
    public class StatementValue : BaseStatementValue
    {
        public string ReportRow { get; set; }
        public string ReportSection { get; set; }
    }

    public class AssetStatementValue : StatementValue
    {
        public double? ActualBrutto { get; set; }
        public double? ActualCorrection { get; set; }
    }
    public abstract class AbstractStatementResult : BaseStatementResult
    {
        public List<AssetStatementValue> Assets { get; set; } = new List<AssetStatementValue>();
        public List<StatementValue> LiabilitiesAndEquity { get; set; } = new List<StatementValue>();
    }

    public class StatementResult : AbstractStatementResult
    {
        public List<StatementValue> IncomeStatement { get; set; } = new List<StatementValue>();
    }
}
