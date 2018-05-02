using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
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
