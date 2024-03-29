using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data.Entities
{
    public class HotLTEMerchant
    {

        public string MobileNumber { get; set; }
        public string MerchantId { get; set; }
        public string FullName { get; set; }
        public DateTime? Created { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int? FailedLoginAttempts { get; set; }
        public DateTime? LastLoginAttempt { get; set; }
        public string IsMobileValid { get; set; }
        public string PreferredLanguage { get; set; }
        public string StoreName { get; set; }
        public string City { get; set; }
        public string SalesDeveloperCode { get; set; }


    }
}
