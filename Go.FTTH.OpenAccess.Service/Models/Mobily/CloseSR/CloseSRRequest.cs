using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Mobily.CloseSR
{
    public class CloseSRRequest
    {
        public string Operation { get { return "CloseSR"; } }
        public string TransactionNo { get; set; }
        public string SRNumber { get; set; }
        public string Flex1 { get; set; }
        public string Flex2 { get; set; }
    }
}
