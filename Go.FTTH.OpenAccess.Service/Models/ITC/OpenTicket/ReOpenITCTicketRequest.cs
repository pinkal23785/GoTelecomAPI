using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.ITC.OpenTicket
{

    public class OpenRequestContent
    {
        public string seeker { get; set; }
        public string txnNumber { get; set; }
        public string operation { get { return "reopentt"; } }
        public string seekerServiceNo { get; set; }
        public string seekerTicketNo { get; set; }
        public string providerServiceNo { get; set; }
        public string providerTicketNo { get; set; }
    }

    public class ReOpenITCTicketRequest
    {
        public string access_token { get; set; }
        public string version { get { return "1.0"; } }
        public string method { get { return "reopentt"; } }
        [JsonPropertyName("content")]
        public OpenRequestContent content { get; set; }
    }

}
