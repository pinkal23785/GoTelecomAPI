using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FtthCollection.Service.Models
{
    public class InstallmentPaymentRequest
    {
        public string AccountNumber { get; set; }
        public string ContactNumber { get; set; }
        public string InstallmantPeriod { get; set; }
        public decimal TotalDueAmount { get; set; }
       // public string BillGenTimeStamp { get; set; }
       // public string ExpDate { get; set; }
        public string NotificationLanguage { get; set; }
        public string UserID { get; set; }
    }
}
