using System;
using System.Collections.Generic;
using System.Text;

namespace GO.Process.EInvoiceProducer.Data
{
    public class EInvoice_Audit
    {
        public string ORDER_ID { get; set; }
        public string ACCOUNT_ID { get; set; }
        public double PAYMENT_AMT { get; set; }
        public double TAX_AMT { get; set; }
        public string PAYMENT_METHOD { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_NAME_AR { get; set; }
        public string EMAIL { get; set; }
        public string MOBILE { get; set; }
        public string SUBSCRIBER_ID { get; set; }
        public string LANGUAGE { get; set; }
        public string SMS_LINK { get; set; }
        public DateTime? SMS_TIME { get; set; }
        public int SMS_STATUS { get; set; }

        public string ORDER_TYPE { get; set; }
        public DateTime INVOICEDATE { get; set; }
    }
}
