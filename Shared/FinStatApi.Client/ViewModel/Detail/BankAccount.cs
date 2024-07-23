using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class BankAccount
    {
        public string AccountNumber { get; set; }
        public DateTime PublishedAt { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", AccountNumber, PublishedAt);
        }
    }
}
