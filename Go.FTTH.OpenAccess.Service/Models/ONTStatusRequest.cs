using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models
{
    public class ONTRequest
    {
        public string ServiceAccountNumber { get; set; }
        public string Operator { get; set; }
        public string Seeker { get; set; }
        public string UserId { get; set; }
        public string AccountID { get; set; }
        public string DawiyatServiceInstanceID { get; set; }

        public string Flex1 { get; set; }
        public string Flex2 { get; set; }
    }
}
