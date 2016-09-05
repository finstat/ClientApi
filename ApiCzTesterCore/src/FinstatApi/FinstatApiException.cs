using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FinstatApi
{
    public class FinstatApiException : Exception
    {
        public enum FailTypeEnum
        {
            UrlNotFound,
            NotFound,
            NotValidCustomerKey,
            OtherCommunicationFail,
            Timeout,
            LimitExceed,
            TooShort,
            Unknown,
            InvalidHash,
            AccessDisabled,
            LicenseExpired,
            InsufficientAccess
        }

        public FailTypeEnum FailType { get; set; }

        public FinstatApiException()
        {
        }

        public FinstatApiException(FailTypeEnum failType, string message)
            : base(message)
        {
            FailType = failType;
        }

        public FinstatApiException(FailTypeEnum failType, string message, Exception inner)
            : base(message, inner)
        {
            FailType = failType;
        }
    }
}
