using System;
using System.Collections.Generic;
using System.Web;

namespace FinstatApi
{
    public class BaseResultCZ
    {
        public string Ico { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Activity { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Cancelled { get; set; }
        public string Url { get; set; }
        public bool Warning { get; set; }
        public string WarningUrl { get; set; }

        public string CZNACE { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string LegalForm { get; set; }
        public string OwnershipType { get; set; }
        public string EmployeeCount { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Ico: {0}, Name: {1} in {5}\n City: {2}\n CZNACE: {6}\n EmployeeCount: {7}\n Created: {3}\n Warning: {4}\n ",
                Ico, Name, City, Created, Warning, Activity, CZNACE, EmployeeCount);
        }
    }
}