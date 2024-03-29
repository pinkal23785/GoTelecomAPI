using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Hyperpay.Service.Models
{
    public class CheckoutSuccessResult
    {
        public Result result { get; set; }
        public string buildNumber { get; set; }
        public string timestamp { get; set; }
        public string ndc { get; set; }
        public string id { get; set; }
    }
}
