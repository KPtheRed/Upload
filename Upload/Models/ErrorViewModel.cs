using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Upload.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class OutPutModel
    {
        public string id { get; set; }
        public string payment { get; set; }
        public string Status { get; set; }
    }

    public class DataViewModel
    {
        public string TransactionIdentificator { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }

    public class PaymentDetail
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Transaction
    {
        public string Transactionid { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
        public PaymentDetail PaymentDetails { get; set; }

    }

    [XmlRoot("Transactions", Namespace = "")]
    public class Transactions : List<Transaction>
    {
    }
}
