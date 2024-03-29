using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Dawiyat.ONTStatus
{
    public class Body
    {
        public double latency { get; set; }
        public string throughput { get; set; }
        [JsonProperty("Rx")]
        public string Rx { get; set; }

        [JsonProperty("Tx")]
        public string Tx { get; set; }

        [JsonProperty("ONT Status")]
        public string ONTStatus { get; set; }
        [JsonProperty("Upload Speed")]
        public string UploadSpeed { get; set; }
        [JsonProperty("Download Speed")]
        public string DownloadSpeed { get; set; }
    }

    public class ONTDawiyatStatusResponse
    {
        public Body body { get; set; }
        public int errCode { get; set; }
        public string msg { get; set; }
    }
}
