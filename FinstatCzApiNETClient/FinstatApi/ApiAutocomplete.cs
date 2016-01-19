using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class ApiAutocomplete
    {
        public Company[] Results { get; set; }
        public string[] Suggestions { get; set; }
        public class Company
        {
            public string Ico { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public bool Cancelled { get; set; }
        }
    }
}
