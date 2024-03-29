using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Process.InvoiceURLProducer.Models
{
    public class SADADUploadResponse
    {
        public string BillNumber { get; set; }
        public double Amount { get; set; }

        public string Lang { get; set; }
    }
}
