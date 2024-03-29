using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Models
{
    public class MerchantUserModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string OldMobile { get; set; }
        public string NewMobile { get; set; }
        public string OTP { get; set; }
    }
}
