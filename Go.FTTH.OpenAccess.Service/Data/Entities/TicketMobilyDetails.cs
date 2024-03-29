using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class TicketMobilyDetail
    {
        public int ID { get; set; }
        public string TICKET_NO { get; set; }
        public string SERVICEACCNUM { get; set; }
        public string CUSTOMER_TYPE { get; set; }
        public string SRTYPE { get; set; }
        public string SRAREA { get; set; }
        public string SR_SUB_AREA { get; set; }
        public string CHANNEL { get; set; }
        public string DESC { get; set; }
        public string SUB_STATUS { get; set; }
        public string SERVICE_OWNER_NAME { get; set; }
        public string SERVICE_OWNER_NUMBER { get; set; }
        public string FLEX1 { get; set; }
        public string FLEX2 { get; set; }
    }
}
