using System;
using System.Collections.Generic;

namespace FinstatApi
{
    public class ExtendedResult : BaseResult
    {
        public enum CreditScoreStateEnum
        {
            Red,
            Yellow,
            Green
        }
        public IcDphAdditonalData IcDphAdditional { get; set; }

        public string District { get; set; }
        public string Region { get; set; }

        public string SkNaceCode { get; set; }
        public string SkNaceText { get; set; }
        public string SkNaceDivision { get; set; }
        public string SkNaceGroup { get; set; }
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

        public bool SelfEmployed { get; set; }
        public Office[] Offices { get; set; }
        public Subject[] Subjects { get; set; }

        public class Debt
        {
            public string Source { get; set; }
            public double Value { get; set; }
            public DateTime ValidFrom { get; set; }

            public override string ToString()
            {
                return string.Format("Source: {0}, Value: {1}, ValidFrom: {2}", Source, Value, ValidFrom);
            }

            public static string AsString(Debt[] values)
            {
                var temp = new List<string>(values.Length);
                foreach (var value in values)
                {
                    temp.Add(value.ToString());
                }
                return string.Join("\n ", temp.ToArray());
            }

        }

        public class PaymentOrder
        {
            public DateTime PublishDate { get; set; }
            public double? Value { get; set; }

            public override string ToString()
            {
                return string.Format("PublishDate: {0}, Value: {1}", PublishDate, Value);
            }

            public static string AsString(PaymentOrder[] values)
            {
                var temp = new List<string>(values.Length);
                foreach (var value in values)
                {
                    temp.Add(value.ToString());
                }
                return string.Join("\n ", temp.ToArray());
            }
        }

        public class IcDphAdditonalData
        {
            public string IcDph { get; set; }
            public string Paragraph { get; set; }
            public DateTime? CancelListDetectedDate { get; set; }
            public DateTime? RemoveListDetectedDate { get; set; }
            public override string ToString()
            {
                return string.Format("IcDph {0} {1}{2}{3}", IcDph, Paragraph,
                    CancelListDetectedDate != null ? "[zoznam s dovodom na zrušenie]" : null,
                    RemoveListDetectedDate != null ? "[zoznam vymazaných]" : null
                    );
            }
        }

        public class Office
        {
            public string[] Subjects { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            public string ZipCode { get; set; }
            public string StreetNumber { get; set; }
            public string Country { get; set; }
            public string District { get; set; }
            public string Region { get; set; }
            public OfficeType Type { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1}, {2} {3}, {4}, {5}, {6}, {7} \n {8}", new object[] {
                    Street,
                    StreetNumber,
                    City,
                    ZipCode,
                    District,
                    Region,
                    Country,
                    Type.ToString(),
                    (Subjects != null) ? string.Join(", ", Subjects) : ""
                });
            }
        }

        public enum OfficeType
        {
            Prevadzka
        }

        public override string ToString()
        {
            string officesString = string.Empty;
            string subjectsString = string.Empty;
            if (Offices != null)
            {
                officesString = "\n Offices:";
                foreach (Office o in Offices)
                {
                    officesString += string.Format("\n - {0}", o.ToString());
                }
            }

            if(Subjects != null)
            {
                subjectsString = "\n Subjects:";
                foreach (Subject s in Subjects)
                {
                    subjectsString += string.Format("\n - {0}", s.ToString());
                }
            }

            return string.Format("Ico: {0}, Name: {1}{21} {23}, IcDph: {25}\n City: {2}, District: {3}, Region: {4}\n Created: {5}\n SkNace: {6}\n Phones: {7}\n Emails: {8}\n Warning: {9}, Payment order warning: {22} OrChange: {10}\n EmployeeText: {11}\n ActualYear: {12} with Credit Score {26}\n ProfitActual: {13}, ProfitPrev: {14}\n RevenueActual: {15}, RevenuePrev: {16}\n ForeignResources: {17}, GrossMargin: {18}, ROA: {19}\n Debts:{20}\n Payment orders:{24}\n Self Employed:{27}{28}", Ico, Name, City, District, Region, Created, SkNaceCode + "-" + SkNaceText, string.Join(", ", Phones), string.Join(", ", Emails), Warning, OrChange, EmployeeText, ActualYear, ProfitActual, ProfitPrev, RevenueActual, RevenuePrev, ForeignResources, GrossMargin, ROA, Debts == null ? "no debt" : Debt.AsString(Debts), SuspendedAsPerson ? "[pozastavená]" : null, PaymentOrderWarning, RegisterNumberText, PaymentOrders == null ? "no payment orders" : PaymentOrder.AsString(PaymentOrders),
                IcDphAdditional != null ? IcDphAdditional.ToString() : null,
                CreditScoreValue != null ? CreditScoreValue.Value.ToString("0.00") + " " + CreditScoreState : null,
                SelfEmployed,
                officesString + subjectsString
                );
        }

        public class Subject
        {
            public string Title { get; set; }
            public DateTime ValidFrom { get; set; }
            public DateTime? SuspendedFrom { get; set; }

            public override string ToString()
            {
                return string.Format("{0} - {1} {2}", new object[] {
                    Title,
                    ValidFrom,
                    (SuspendedFrom != null) ? string.Format(" - {0}", SuspendedFrom) : string.Empty,
                });
            }
        }
    }
}