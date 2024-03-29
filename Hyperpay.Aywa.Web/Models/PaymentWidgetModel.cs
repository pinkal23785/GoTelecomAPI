using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Models
{
    public class PaymentWidgetModel
    {
        public string CheckoutId { get; set; }
        public string CardType { get; set; }

        public string AwayaCardId { get; set; }
        public string PaymentResultURL { get; set; }

        public string Lang { get; set; }

        public decimal Amount { get; set; }
    }
}
