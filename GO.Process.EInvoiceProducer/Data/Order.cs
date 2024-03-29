using System;
using System.Collections.Generic;
using System.Text;

namespace GO.Process.EInvoiceProducer.Data
{
    public class Order
    {

        public string ORDER_ID { get; set; }
        public string ORDER_TYPE { get; set; }
        public DateTime ORDER_CREATION_DATE { get; set; }
        public DateTime ORDER_START_DATE { get; set; }
        public DateTime ORDER_EXPECTED_COMPL_DATE { get; set; }
        public DateTime ORDER_ACT_COMPL_DATE { get; set; }
        public DateTime ORDER_RFS_DATE { get; set; }
        public string ORDER_STATUS { get; set; }
        public DateTime INSTALLATION_DATE { get; set; }
        public string SUBSCRIBER_ID { get; set; }
        public string PLAN_ID { get; set; }
        public string AIDED_UNAIDEDN_FLAG { get; set; }
        public string CHANNEL_ID { get; set; }
        public string CHANNEL_TYPE { get; set; }
        public string PYMT_METHOD { get; set; }
        public string PYMT_REF_NO { get; set; }
        public string DEALER_CODE { get; set; }
        public string ACCOUNT_ID { get; set; }
        public double PAYMENT_AMOUNT { get; set; }
        public string INTERACTION_ID { get; set; }
        public string ORDER_SUB_STATUS { get; set; }
        public string ORDER_SUB_TYPE { get; set; }
        public string CREATED_BY { get; set; }
        public string CONTRACT_ID { get; set; }
        public double PURCHASE_FEE { get; set; }
        public double DEVICE_COST { get; set; }
        public double NUMBER_COST { get; set; }
        public double DELIVERY_COST { get; set; }
        public string MASTER_ORDER_ID { get; set; }
        public double ORIGINAL_AMOUNT { get; set; }
        public double DISCOUNT_PERCENTAGE { get; set; }
        public DateTime PYMT_DATE { get; set; }
        public string APPROVER_OVERRIDEN { get; set; }
        public string SECURITY_DEPOSIT { get; set; }
        public string TAX_AMOUNT { get; set; }
    }
}
