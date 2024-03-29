using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Process.InvoiceURLProducer.Models
{
    public class SADADUploadRequest
    {
        public string accountId { get; set; }
        public decimal Amount { get; set; }
    }
}
