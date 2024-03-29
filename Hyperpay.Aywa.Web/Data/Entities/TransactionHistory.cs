using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Data.Entities
{
    public class TransactionHistory
    {
        public int ID { get; set; }
        public string Trans_type { get; set; }
        public string Trans_time { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Payment_Ref { get; set; }
        public string Payment_Method { get; set; }
        public string Invoice_ID { get; set; }
        public string CardNumber { get; set; }

        public string Last_Modify_Date { get; set; }
        public string PUR_ORD_RESP_STATUS { get; set; }
        public string PUR_ORD_SUBSCRIBER_ID { get; set; }
        public string pur_ord_account_id { get; set; }
        public string pur_ord_order_id { get; set; }


    }
}
