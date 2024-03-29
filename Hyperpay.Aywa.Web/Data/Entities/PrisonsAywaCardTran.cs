using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Data.Entities
{
    public class PrisonsAywaCardTran
    {

        public int ID { get; set; }
        public int? CARD_STOCK_ID { get; set; }
        public string TRANS_TIME { get; set; }
        public string TRANS_TYPE { get; set; }
        public string CREATED_BY { get; set; }
        public string EMAIL { get; set; }
        public string MOBILE { get; set; }
        public decimal? TOTAL_AMOUNT_VAT { get; set; }
        public string PAYMENT_REF { get; set; }
        public string PAYMENT_METHOD { get; set; }
        public string PUR_ORD_RESP_STATUS { get; set; }
        public string PUR_ORD_SUBSCRIBER_ID { get; set; }
        public string PUR_ORD_ACCOUNT_ID { get; set; }
        public string PUR_ORD_ORDER_ID { get; set; }
        public string CONTACT_PREF { get; set; }
        public string LANG_PREF { get; set; }
        public int? CARD_TYPE_ID { get; set; }
        public int? QUANTITY { get; set; }
        public string MERCHANT_ID { get; set; }
        public DateTime? TRNAS_TIMESTAMP { get; set; }
        public string INVOICE_ID { get; set; }
        public decimal? CARD_AMOUNT { get; set; }
        public decimal? VAT_AMOUNT { get; set; }
        public int? IS_SMS_SENT { get; set; }
        public int? IS_MAIL_SENT { get; set; }
    }
}
