using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class DistraintPreview
    {
        public string Code { get; set; }        // Znacka
        public Debtor[] Debtors;              // Povinni
        public int TypeOfAuthorisation { get; set; }   // TypPoverenia
        public DateTime Created { get; set; }
        public int DetailId { get; set; }
        public string DetailToken { get; set; }
        public string StoredDetailId { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(base.ToString());

            result.AppendLine(string.Format("Code: {0}", Code));
            if (Debtors != null && Debtors.Length > 0)
            {
                result.AppendLine("\nDebtors:");
                foreach (var oblig in Debtors)
                {
                    result.AppendLine(oblig.ToString());
                }
            }
            result.AppendLine(string.Format("TypeOfAuthorisation: {0} Created: {1} DetailId {2} DetailToken {3} StoredDetailId {4}",
                TypeOfAuthorisation, Created, DetailId, DetailToken, StoredDetailId));
            return result.ToString();
        }
    }
}
