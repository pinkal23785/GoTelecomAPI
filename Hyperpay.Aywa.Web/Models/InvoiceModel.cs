using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Models
{
    public class InvoiceModel
    {
        public string OrderNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }

        public string AywaCardName { get; set; }
        public string AywaCardNumber { get; set; }
        public string PaymentMethod { get; set; }

        public decimal? Card_Amount { get; set; }
        public decimal? VAT_Amount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string VatNumber { get; set; }
        public string HyperPayTransactionID { get; set; }

    }
}
