using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Mobily.ONT
{
    public class ONTMobilyRequest
    {
        public string Operation { get { return "ONTStatus"; } }
        public string ServiceAccNum { get; set; }
        public string TransactionNo { get; set; }
        public string Flex1 { get; set; }
        public string Flex2 { get; set; }


    }
}
