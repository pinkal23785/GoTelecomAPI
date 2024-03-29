using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Mobily.ONT
{
    public class ONTLatencyMobilyResponse
    {
        public string Operation { get; set; }
        public string TransactionNo { get; set; }
        public string Status { get; set; }
        [JsonProperty("ONT Latency")]
        public string ONTLatency { get; set; }

        [JsonProperty("ONT Speed")]
        public string ONTSpeed { get; set; }
        public string ErrorMessage { get; set; }
    }
}
