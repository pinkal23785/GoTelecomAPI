using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.ITC.NewTicket
{
    public class Content
    {
        public string seeker { get; set; }
        public string txnNumber { get; set; }
        public string operation { get { return "creatett"; } }
        public string seekerServiceNo { get; set; }
        public string providerServiceNo { get; set; }
        public string Impact { get; set; }
        public string Urgency { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string Description { get; set; }
        public string Detailed_Decription { get; set; }
        public string Ticket_Status { get; set; }
        public string Contact_Phone { get; set; }
        public string Problem_Code { get; set; }
        public string Severity { get; set; }
        public string Service_Impacted { get; set; }
        public string External_Reference_number { get; set; }
        public DateTime? Actual_Incident_Start_DateTime { get; set; }
    }

    public class ITCTicketRequest
    {
        public string access_token { get; set;}
        public string version { get; private set; } =  "1.0";
        public string method { get { return "creatett"; } }
        public Content content { get; set; }
    }

    public class NewITCTicketRequest
    {
        public string AccountId { get; set; }
        public string Operator { get; set; }
        public string UserID { get; set; }
        public string OrderID { get; set; }
        public ITCTicketRequest NewCase { get; set; }
    }
}


