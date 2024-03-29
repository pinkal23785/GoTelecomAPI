using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Dawiyat.ResourceFeasibility
{
    public class Body
    {
        public string id { get; set; }
        public string odbid { get; set; }
        public string availablePorts { get; set; }
        public string ONT { get; set; }
    }

    public class FeasibilityResponse
    {
        public Body body { get; set; }
        public string errCode { get; set; }
        public string errDisc { get; set; }
    }
   
}
