using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class TicketMaster
    {
        public int ID { get; set; }
        public string TICKET_ID { get; set; }
        public string REFERENCE_ID { get; set; }
        public DateTime? CREATE_T { get; set; }
        public string ACCOUNT_ID { get; set; }
        public string DESCR { get; set; }
        public string STATUS { get; set; }
        public string OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_T { get; set; }
        public string NUMB { get; set; }
        public string SYS_ID { get; set; }
        public string USERID { get; set; }
        public string COMMENT { get; set; }
        public string ORDERID { get; set; }
    }
}
