using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Go.Service.NumberBooking.Data.Entities
{
    [DataContract]
    public class Customer
    {
        public int Customer_Id { get; set; }
        public string Customer_Company { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Email { get; set; }
        public string Customer_Address { get; set; }
        public string Customer_City { get; set; }
        public string Customer_Tel { get; set; }
        public string Customer_Mobile { get; set; }
    }
}
