using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class DistraintResult
    {
        public int Count { get; set; }
        public DistraintPreview[] Distraints { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(base.ToString());

            result.AppendLine(string.Format("Count: {0}", Count));
            if (Distraints != null && Distraints.Length > 0)
            {
                result.AppendLine("\nDistraints:");
                foreach (var distraint in Distraints)
                {
                    result.AppendLine(distraint.ToString());
                }
            }
            return result.ToString();
        }
    }

    public class DistraintDetailResults
    {
        public DistraintDetailResult[] DistraintDetails { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(base.ToString());

            if (DistraintDetails != null && DistraintDetails.Length > 0)
            {
                result.AppendLine("\nDistraints:");
                foreach (var distraint in DistraintDetails)
                {
                    result.AppendLine(distraint.ToString());
                }
            }
            return result.ToString();
        }
    }

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

    public class DistraintDetailResult : DistraintPreview
    {
        public string Court { get; set; }               // Sud
        public DateTime DateOfAuthorisation { get; set; }      // DatumPoverenia
        public string CourtCode { get; set; }           // SudZn
        public string EnforcementDetails { get; set; }    // PopisNaroku
        public decimal SumOutstanding { get; set; }             // VyskaPlnenia
        public string Currency { get; set; }            // Mena
        public Bailiff Bailiff;                       // Exekutor

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(base.ToString());

            base.ToString();
            result.AppendLine(string.Format("Court: {0} DateOfAuthorisation: {1} CourtCode: {2} SumOutstanding: {3} Currency: {4}", 
                Court, DateOfAuthorisation, CourtCode, SumOutstanding, Currency));
            result.AppendLine(Bailiff.ToString());
            result.AppendLine(string.Format("ClaimDescription: {0}",
                EnforcementDetails));
            return result.ToString();
        }
    }

    public class Debtor
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdentificationNumber { get; set; }    // Rodne cislo
        public string ICO { get; set; }
        public string CompanyName { get; set; }
        public string AddressType { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZIP { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(base.ToString());

            result.AppendLine(string.Format("Type: {0} Name: {1} Surname: {2} DateOfBirth: {3} IdentificationNumber: {4} ICO: {5} CompanyName: {6} AddressType: {7} Street: {8} City: {9} ZIP: {10}",
                Type, Name, Surname, DateOfBirth, IdentificationNumber, ICO, CompanyName, AddressType, Street, City, ZIP));
            return result.ToString();
        }
    }

    public class Bailiff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string ZIP { get; set; }
        public string City { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(base.ToString());

            result.AppendLine(string.Format("Id: {0} Name: {1} Street: {2} ZIP: {3} City: {4}",
                Id, Name, Street, ZIP, City));
            return result.ToString();
        }
    }
}
