using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Go.SMSA.Services.Models
{

    public class Item
    {
        public string batch { get; set; }
        public string expiry { get; set; }
        public string material { get; set; }
        public string quantity { get; set; }
        public string serial { get; set; }
    }

    public class SMSAOrderConfirmationModel
    {
        public List<Item> items { get; set; }
        public string obdnumber { get; set; }
        public string referenceNumber { get; set; }
        public string trackingNumber { get; set; }
        public string transactionNumber { get; set; }
    }

}