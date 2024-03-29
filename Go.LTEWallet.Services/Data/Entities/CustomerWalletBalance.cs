using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data.Entities
{
    public class CustomerWalletBalance
    {
        public int ID { get; set; }
        public string INSERT_BY { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public int? LAST_TRANS_ID { get; set; }
        public string USER_FULL_NAME_AR { get; set; }
        public string USER_FULL_NAME_EN { get; set; }
        public double? WALLET_BALANCE { get; set; }
        public string MOBILENUMBER { get; set; }

        public string MerchantID { get; set; }
    }
}
