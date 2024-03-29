using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Hyperpay.Service.Models
{
    public class CheckoutModel
    {
        public string mobileNumber { get; set; }
        public string cardType { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string paymentType { get; set; }
        public string customerEmail { get; set; }
        public string billingStreet1 { get; set; }
        public string billingCity { get; set; }
        public string billingState { get; set; }
        public string billingCountry { get; set; }
        public string billingPostcode { get; set; }
        public string customerGivenName { get; set; }
        public string surname { get; set; }
        public string sessionId { get; set; }
        public string userAgent { get; set; }
    }

}
