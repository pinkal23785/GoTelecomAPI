using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Customers
{
    public class SearchCustomerRequest
    {
        public string customerId { get; set; }
        public string subscriberId { get; set; }
        public string accountId { get; set; }
    }
}
