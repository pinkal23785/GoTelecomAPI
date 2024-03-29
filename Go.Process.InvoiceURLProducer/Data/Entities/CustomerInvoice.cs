using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Process.InvoiceURLProducer.Data.Entities
{
    public class CustomerInvoice
    {
        public Int64 POID_ID0 { get; set; }
        public string ACCOUNT_NO { get; set; }
        public string CUSTOMER_NAME { get; set; }

        public string CUSTOMER_NAME_AR { get; set; }
        public string PHONE { get; set; }
        public string EMAIL_ADDR { get; set; }
        public string BILL_NO { get; set; }
        public string START_T { get; set; }
        public string END_T { get; set; }
        public string DUE_T { get; set; }
        public double PAID_AMOUNT { get; set; }
        public double LAST_BILL_AMT { get; set; }
        public double TOTAL_DUE { get; set; }
        public double DUE { get; set; }
        public double CURRENT_TOTAL { get; set; }
        public double PREVIOUS_TOTAL { get; set; }
        public double CYCLE_TAX { get; set; }
        public double CURRENT_TOTAL_WV { get; set; }
        public Int64 ACCOUNT_OBJ_ID0 { get; set; }
        public string PLAN_NAME { get; set; }
        public double CREDIT_LIMIT { get; set; }
        public double PLAN_PRICE { get; set; }
        public string SMS_LINK { get; set; }
        public string SMS_STATUS { get; set; }
        public string SMS_TIME { get; set; }
        public string EMAIL_STATUS { get; set; }
        public string EMAIL_TIME { get; set; }
        public string PDF_STATUS { get; set; }
        public string PDF_TIME { get; set; }
        public string SADAD_STATUS { get; set; }
        public string SADAD_TIME { get; set; }
        public string LANGUAGE { get; set; }
        public string SUBSCRIBER_ID { get; set; }
        public string PLAN_ID { get; set; }
        public int? IS_ORDERED { get; set; }
    }
}
