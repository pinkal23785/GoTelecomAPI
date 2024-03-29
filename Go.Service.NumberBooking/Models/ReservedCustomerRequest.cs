using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Service.NumberBooking.Models
{
    public class ReservedCustomerRequest
    {
        public string Category { get; set; }
        public string  Numbers { get; set; }
        public string CRMCustomerId { get; set; }
        public string AccountManagerEmail { get; set; }
        public string AccountManagerName { get; set; }
    }
}
