using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class ONTMobilyDetail
    {

        public int ID { get; set; }
        public int ONT_ID { get; set; }
        public string TRANSACTIONNO { get; set; }
        public string SERVICEACCNUM { get; set; }
        public string STATUS { get; set; }
        public string QUALITY { get; set; }
        public string ONTRX { get; set; }
        public string ONTTX { get; set; }
        public string OLTRX { get; set; }
        public string OLTTX { get; set; }
        public string ONT_LATENCY { get; set; }
        public string ONT_SPEED { get; set; }
        public int IS_LATENCY { get; set; }

        public string ONTRxHistory { get; set; }
        public string ONTTxHistory { get; set; }
    }
}
