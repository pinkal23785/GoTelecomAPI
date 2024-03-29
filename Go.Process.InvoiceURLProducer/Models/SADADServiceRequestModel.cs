using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Process.InvoiceURLProducer.Models
{
    public class SADADServiceRequestModel
    {
        public string subscriberSegment { get; set; } = "1";
        public string subscriberType { get; set; } = "Consumer";
    }
}
