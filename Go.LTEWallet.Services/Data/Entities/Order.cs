using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data.Entities
{
    public class Order
    {
        public List<OrderSummary> OrderSummaries { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
