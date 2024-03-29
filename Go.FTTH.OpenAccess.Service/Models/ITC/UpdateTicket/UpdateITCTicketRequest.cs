using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Go.FTTH.OpenAccess.Service.Models.ITC.UpdateTicket
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class UpdateContent
    {
        public string seeker { get; set; }
        public string txnNumber { get; set; }
        public string operation { get { return "updatett"; } }
        public string seekerServiceNo { get; set; }
        public string seekerTicketNo { get; set; }
        public string providerServiceNo { get; set; }
        public string providerTicketNo { get; set; }
        public string Work_Info { get; set; }
        public string Work_Info_Summary { get; set; }
    }

    public class UpdateITCTicketRequest
    {
        public string access_token { get; set; }
        public string version { get; private set; } = "1.0";
        public string method { get { return "updatett"; } }

        [JsonPropertyName("content")]
        public UpdateContent content { get; set; }
    }
   
}
