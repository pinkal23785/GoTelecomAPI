using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Data
{

    public class Data
    {
        public long MessageID { get; set; }
        public string CorrelationID { get; set; }
        public string Status { get; set; }
        public int NumberOfUnits { get; set; }
        public int Cost { get; set; }
        public int Balance { get; set; }
        public string Recipient { get; set; }
        public string TimeCreated { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class SMSResponseModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
        public Data data { get; set; }
    }

}
