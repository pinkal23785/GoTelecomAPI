using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class ONTItcDetail
    {
        public int ID { get; set; }
        public int ONT_ID { get; set; }
        public string TXN_NUMBER { get; set; }
        public string RUN_STATUS { get; set; }
        public string LAST_UP_TIME { get; set; }
        public string LAST_DOWN_TIME { get; set; }
        public DateTime MODIFT_DT { get; set; }

        public string ontRx { get; set; }
        public string ontTx { get; set; }
        public string gponRx { get; set; }
        public string gponTx { get; set; }
    }
}
