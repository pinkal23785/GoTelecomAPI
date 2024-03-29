using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Go.SMSA.Services.Models
{
    
    public class CustomerIdentifier
    {
        public int id { get; set; }
    }

    public class FacilityIdentifier
    {
        public int id { get; set; }
    }

    public class RoutingInfo
    {
        public string carrier { get; set; }
        public string mode { get; set; }
    }

    public class ShipTo
    {
        public string companyName { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
    }

    public class ItemIdentifier
    {
        public string sku { get; set; }
    }

    public class OrderItem
    {
        public ItemIdentifier itemIdentifier { get; set; }
        public int qty { get; set; }
    }

    public class SMSAOrderRequestModel
    {
        public CustomerIdentifier customerIdentifier { get; set; }
        public FacilityIdentifier facilityIdentifier { get; set; }
        public string referenceNum { get; set; }
        public string billingCode { get; set; }
        public RoutingInfo routingInfo { get; set; }
        public ShipTo shipTo { get; set; }
        public List<OrderItem> orderItems { get; set; }
    }

}