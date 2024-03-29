using System;
using System.Collections.Generic;
using System.Text;

namespace GO.Process.EInvoiceProducer.Data
{
    public class SearchOrders
    {
        public string Order_ID { get; set; }
        public string Account_ID { get; set; }
        public double Payment_Amount { get; set; }
        public double Tax_Amount { get; set; }
        public string Payment_Method { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Name_AR { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Subscriber_ID { get; set; }
        public string Language { get; set; }
        public string Order_Type { get; set; }


    }
}
