using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Models
{
    public class PaymentSucessModel
    {
        public string InvoiceId { get; set; }
        public string shortURL { get; set; }
        public string ResultCode { get; set; }
        public string Lang { get; set; }

        public string StatusDesc { get;set; }
        public string CardNumber { get; set; }
    }
}
