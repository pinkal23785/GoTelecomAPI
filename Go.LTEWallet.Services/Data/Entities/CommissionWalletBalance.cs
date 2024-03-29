using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data.Entities
{
    public class CommissionWalletBalance
    {

        public int ID { get; set; }

        //public string USER_ID1 { get; set; }
        public string USER_FULL_NAME_EN { get; set; }
        public string USER_FULL_NAME_AR { get; set; }
        public double WALLET_BALANCE { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string INSERT_BY { get; set; }
        public DateTime? LAST_MODIFY_DATE { get; set; }
        public string LAST_MODIFY_BY { get; set; }
        public int? LAST_TRANS_ID { get; set; }
        public string SALESDEVELOPERCODE { get; set; }
        public string STORENAME { get; set; }
        public string USER_ID { get; set; }
        public string MERCHANTID { get; set; }
    }
}
