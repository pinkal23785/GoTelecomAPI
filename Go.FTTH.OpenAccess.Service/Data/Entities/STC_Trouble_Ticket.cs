using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class STC_Trouble_Ticket
    {

        public string TROUBLE_TICKETID { get; set; }
        public string PROBLEMID { get; set; }
        public string PROBLEMTYPE { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATEDBY { get; set; }
        public DateTime? CREATEDDATE { get; set; }
        public DateTime? SERVICE_IMPACT_START { get; set; }
        public string STATUS { get; set; }
        public string SEVERITY { get; set; }
        public string SERVICENUMBER { get; set; }
        public string SERVICETYPE { get; set; }
        public string PRODUCTCODE { get; set; }
        public string TICKETCATEGORY { get; set; }
        public string COMMITTIME { get; set; }
        public string AREA { get; set; }
        public string SUBAREA { get; set; }
        public string CHANNELREF { get; set; }
        public string CHANGEONT { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public string CUSTOMER_PRIORITY { get; set; }
        public string FUNCTION_CODE { get; set; }
        public string CAUSE_CODE { get; set; }
        public string SOLUTION_CODE { get; set; }
        public string MEMO_TEXT { get; set; }
        public string SERVICE_ACCOUNT_NUMBER { get; set; }
        public string TYPE { get; set; }
        public string SUBTYPE { get; set; }
        public string ACCOUNTID { get; set; }
        public string SUBSCRIBERID { get; set; }
        public string IDTYPE { get; set; }
        public string IDNUMBER { get; set; }
    }
}
