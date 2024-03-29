using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Mobily.ONT
{
    public class ONTMobilyResponse
    {
        public string Operation { get; set; }
        public string TransactionNo { get; set; }
        public string ServiceAccNum { get; set; }
        public string ErrorCode { get; set; }
        public string status { get; set; }
        public string quality { get; set; }
        public string ONTRx { get; set; }

        public string ONTTx { get; set; }
        public string OLTRx { get; set; }

        public string OLTTx { get; set; }

        public List<ONTRxHistory> ONTRxHistory { get; set; }
        public List<ONTTxHistory> ONTTxHistory { get; set; }
    }
    public class ONTRxHistory
    {
        public string Rx { get; set; }
        public string Time { get; set; }
    }

    public class ONTTxHistory
    {
        public string Tx { get; set; }
        public string Time { get; set; }
    }

}
