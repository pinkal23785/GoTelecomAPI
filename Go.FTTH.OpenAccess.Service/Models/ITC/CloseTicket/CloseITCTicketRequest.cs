using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.ITC.CloseTicket
{

    public class CloseRequestContent
    {
        public string seeker { get; set; }
        public string txnNumber { get; set; }
        public string operation { get { return "closett"; } }
        public string seekerServiceNo { get; set; }
        public string seekerTicketNo { get; set; }
        public string providerServiceNo { get; set; }
        public string providerTicketNo { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
    }

    public class CloseITCTicketRequest
    {
        public string access_token { get; set; }
        public string version { get { return "1.0"; } }
        public string method { get { return "closett"; } }
        [JsonPropertyName("content")]
        public CloseRequestContent content { get; set; }
    }
    
}
