using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data.Entities
{
    public class OrderDetail
    {
        public string OrderId { get; set; }
        public string OrderDate { get; set; }
        public string PlanName { get; set; }
        public float Credit_Amount { get; set; }
        public float Balance_Before { get; set; }
        public float Balance_After { get; set; }
        public string SIM { get; set; }
        public string DEVICE_SN { get; set; }
    }
}
