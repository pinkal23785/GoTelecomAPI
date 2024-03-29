using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.SADAD
{
    public class AywaCardPaymentReqModel
    {
        public string paymentMethod { get; set; }
        public string paymentRef { get; set; }
        public string accountId { get; set; }
        public string subscriberId { get; set; }
        public string orderID { get; set; }
        public string totalPaidAmount { get; set; }
        public string lang { get; set; }
    }
}
