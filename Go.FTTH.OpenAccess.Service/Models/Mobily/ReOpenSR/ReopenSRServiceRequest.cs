using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Mobily.ReOpenSR
{
    public class ReopenSRServiceRequest
    {
        public string Operation { get; set; }
        public string SRNumber { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Flex1 { get; set; }
        public string Flex2 { get; set; }
    }
}
