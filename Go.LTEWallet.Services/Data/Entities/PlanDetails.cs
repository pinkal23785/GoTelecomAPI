using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data.Entities
{
    public class PlanDetail
    {
        public string PLAN_ID { get; set; }
        public string PROD_ID { get; set; }
        public double? PLAN_PRICE { get; set; }
        public int RN { get; set; }

    }
}
