using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data.Entities
{
    public class OrderSummary
    {
        public string PlanName { get; set; }
        public int NumberOfOrders { get; set; }
        public float Total_Credit_Amount { get; set; }
    }
}
