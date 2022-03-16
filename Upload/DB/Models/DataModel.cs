using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upload.DB.Models
{
    public class DataModel
    {
        public int DataModelID { get; set; }
        public string TransactionIdentificator { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Status { get; set; }
    }
}
