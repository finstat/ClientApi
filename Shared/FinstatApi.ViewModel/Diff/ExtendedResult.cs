using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi.ViewModel.Diff
{
    public class ExtendedResult : BaseResult
    {
        public IcDphAdditonalData IcDphAdditional { get; set; }

        public string SkNaceCode { get; set; }
        public string SkNaceText { get; set; }
        public string SkNaceDivision { get; set; }
        public string SkNaceGroup { get; set; }

        public string District { get; set; }
        public string Region { get; set; }
        public string[] Phones { get; set; }
        public string[] Emails { get; set; }

        public Debt[] Debts { get; set; }
        public PaymentOrder[] PaymentOrders { get; set; }

        public string EmployeeCode { get; set; }
        public string EmployeeText { get; set; }

        public string LegalFormCode { get; set; }
        public string LegalFormText { get; set; }

        public string OwnershipTypeCode { get; set; }
        public string OwnershipTypeText { get; set; }

        public int? ActualYear { get; set; }

        public double? CreditScoreValue { get; set; }
        public CreditScoreStateEnum? CreditScoreState { get; set; }

        public double? ProfitActual { get; set; }
        public double? ProfitPrev { get; set; }

        public double? RevenueActual { get; set; }
        public double? RevenuePrev { get; set; }

        public double? ForeignResources { get; set; }
        public double? GrossMargin { get; set; }
        public double? ROA { get; set; }

        public DateTime? WarningKaR { get; set; }
        public DateTime? WarningLiquidation { get; set; }

        public class Debt
        {
            public string Source { get; set; }
            public double Value { get; set; }
            public DateTime ValidFrom { get; set; }
        }

        public class PaymentOrder
        {
            public DateTime PublishDate { get; set; }
            public double? Value { get; set; }
        }

        public class IcDphAdditonalData
        {
            public string IcDph { get; set; }
            public string Paragraph { get; set; }
            public DateTime? CancelListDetectedDate { get; set; }
            public DateTime? RemoveListDetectedDate { get; set; }
        }
    }
}
