using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.ITC.ONT_Status
{
    public class ONTITCContent
    {
        public string seeker { get; set; }
        public string txnNumber { get; set; }
        public string operation { get { return "ontstatus"; } }
        public string providerServiceNo { get; set; }
    }

    public class ONTStatusRequest
    {
        public string access_token { get; set; }
        public string version { get { return "1.0"; } }
        public string method { get { return "ontstatus"; } }

        [JsonPropertyName("content")]
        public ONTITCContent content { get; set; }
    }

}
