using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
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
}
