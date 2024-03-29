using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FtthCollection.Service.Models
{
    public class DiscountPaymentOrderReq
    {
        public string AccountNumber { get; set; }
        public string ContactNumber { get; set; }
        public decimal TotalDueAmount { get; set; }
        public decimal DueAmountAfterDiscount { get; set; }
        public decimal DiscountPercentage { get; set; }
       // public string BillGenTimeStamp { get; set; }
       // public string ExpDate { get; set; }
        public string NotificationLanguage { get; set; }

        public string UserID { get; set; }
    }
}
