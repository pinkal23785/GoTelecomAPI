using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data.Entities
{
    public class CommissionMerchantUser
    {
        public string MOBILENUMBER { get; set; }
        public string MERCHANTID { get; set; }
        public string FULLNAME { get; set; }
        public string SALESDEVELOPERCODE { get; set; }
        public string CREATED { get; set; }
        public string EMAIL { get; set; }
        public string STATUS { get; set; }
        public int? FAILEDLOGINATTEMPTS { get; set; }
        public string LASTLOGINATTEMPT { get; set; }
        public string ISMOBILEVALID { get; set; }
        public string PREFERREDLANGUAGE { get; set; }
        public string STORENAME { get; set; }
        public string CITY { get; set; }
    }
}
