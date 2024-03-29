using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FtthCollection.Service.Data.Entities
{
    public class AccountDetail
    {
        public int ID { get; set; }
        public string ACCOUNT_ID { get; set; }
        public string SUBCCRIBER_ID { get; set; }
        public string CUSTOMER_NAME_EN { get; set; }
        public string CUSTOMER_NAME_AR { get; set; }
        public string ID_NO { get; set; }
        public string ID_TYPE { get; set; }
        public string GENDER { get; set; }
        public string NATIONALITY { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMAIL { get; set; }
        public string STATUS { get; set; }
        public string SERVICE_TYPE { get; set; }
        public string PLAN_NAME { get; set; }
        public int? COLL_GROUP_ID { get; set; }
        public string BILL_NO { get; set; }
        public double? DUE_AMOUNT { get; set; }
        public double? AGING { get; set; }
        public DateTime? FIRST_INVOICE_DT { get; set; }
        public string LAST_INVOICE_DT { get; set; }
        public DateTime? LAST_PAYMENT_DT { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string INSERT_BY { get; set; }
        public DateTime? LAST_MODIFY_DATE { get; set; }
        public string LAST_MODIFY_BY { get; set; }
        public string COLL_GROUP_NAME { get; set; }

        public string SUBMITTED_TO { get; set; }
        public int? IS_SUBMITTED { get; set; }

        public string IS_PRE_POST { get; set; }

    }
}
