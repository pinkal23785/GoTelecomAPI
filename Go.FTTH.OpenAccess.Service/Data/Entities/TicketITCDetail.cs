using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class TicketITCDetail
    {

        public int ID { get; set; }
        public string TICKET_ID { get; set; }
        public string SEEKER { get; set; }
        public string TXNUMBER { get; set; }
        public string OPERATION { get; set; }
        public string SEEKER_SERVICE_NO { get; set; }
        public string PROVIDER_SERVICE_NO { get; set; }
        public string IMPACT { get; set; }
        public string URGENCY { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string DESC { get; set; }
        public string DETAIL_DESC { get; set; }
        public string TICKET_STATUS { get; set; }
        public string CONTACT_PHONE { get; set; }
        public string PROBLEM_CODE { get; set; }
        public string SEVERITY { get; set; }
        public string SERVICE_IMPACTED { get; set; }
        public string EXTERNAL_REFERENCE_NO { get; set; }
        public string ACTUAL_INCIDENT_START_DATE { get; set; }
        public string WORK_INFO { get; set; }
        public string WORK_INFO_SUMMARY { get; set; }

    }
}
