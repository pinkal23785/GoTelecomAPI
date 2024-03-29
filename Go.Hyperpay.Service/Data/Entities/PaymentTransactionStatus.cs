using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Hyperpay.Service.Data.Entities
{
    public class PaymentTransactionStatus
    {
        public int ID { get; set; }
        public string CHECKOUT { get; set; }
        public string CLIENT_IP { get; set; }
        public string CLIENT_PORT { get; set; }
        public string CLIENT_CONTEXT { get; set; }
        public string CLIENT_USER { get; set; }
        public string CLIENT_SESSION_ID { get; set; }
        public string HYPERPAY_TRANS_ID { get; set; }
        public string PAYMENT_TYPE { get; set; }
        public string PAYMENT_BRAND { get; set; }
        public string PAYMENT_AMOUNT { get; set; }
        public string PAYMENT_CURRENCY { get; set; }
        public string PAYMENT_DESCRIPTOR { get; set; }
        public string MERCHANT_TRANSACTION_ID { get; set; }
        public string RESULT_CODE { get; set; }
        public string RESULT_DESCRIPTION { get; set; }
        public string CONNECTORTXID1 { get; set; }
        public string CARD_BIN { get; set; }
        public string CARD_BIN_COUNTRY { get; set; }
        public string CARD_LAST_4DIGITS { get; set; }
        public string CARD_HOLDER { get; set; }
        public string CARD_EXPIRY_MONTH { get; set; }
        public string CARD_EXPIRY_YEAR { get; set; }
        public string CUST_GIVEN_NAME { get; set; }
        public string CUST_SURNAME { get; set; }
        public string CUST_EMAIL { get; set; }
        public string CUST_IP { get; set; }
        public string CUST_COUNTRY { get; set; }
        public string BILLING_STREET1 { get; set; }
        public string BILLING_CITY { get; set; }
        public string BILLING_STATE { get; set; }
        public string BILLING_POST_CODE { get; set; }
        public string BILLING_COUNTRY { get; set; }
        public string SHOPPER_ENDTOENDIDENTITY { get; set; }
        public string CTPE_DESCRIPTOR_TEMPLATE { get; set; }
        public string SCORE { get; set; }
        public string BUILD_NUMBER { get; set; }
        public string TRANS_TIMESTAMP { get; set; }
        public string NDC { get; set; }
        public string ERROR_PARAM_NAME { get; set; }
        public string ERROR_PARAM_VAL { get; set; }
        public string ERROR_PARAM_MSG { get; set; }
        public string CLIENT_TRANSACTION_ID { get; set; }
        public string TRANSACTION_TIMESTAMP { get; set; }


       // public string MOBILENUMBER { get; set; }
        public string MERCHANTTRANSACTIONID { get; set; }
        public string ENTITYID { get; set; }
        public string CARDTYPE { get; set; }
        public decimal AMOUNT { get; set; }
        public string CURRENCY { get; set; }
        public string STREET { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string COUNTRY { get; set; }
        public string POSTALCODE { get; set; }
        public string GIVENNAME { get; set; }
        public string SURNAME { get; set; }
        public string CHECKOUTDATE { get; set; }

        public string EMAIL_ID { get; set; }

        public string MOBILE_NUMBER { get; set; }

        public string USER_AGENT { get; set; }





    }
}
