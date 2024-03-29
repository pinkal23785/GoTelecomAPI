using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.ITC.ONT_Status
{

    public class ONTITCResult
    {
        public string txnNumber { get; set; }
        public string resultCode { get; set; }
        public string resultDesc { get; set; }
        public string runStatus { get; set; }
        public string lastUptime { get; set; }
        public string lastDowntime { get; set; }

        public string ontRx { get; set; }
        public string ontTx { get; set; }
        public string gponRx { get; set; }
        public string gponTx { get; set; }
    }

    public class ONTITCStatusResponse
    {
        public string res_code { get; set; }
        public string res_message { get; set; }

        public ONTITCResult result { get; set; }
    }
}
