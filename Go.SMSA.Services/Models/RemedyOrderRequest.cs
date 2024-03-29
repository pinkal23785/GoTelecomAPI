using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Go.SMSA.Services.Models
{
    public class RemedyOrderRequest
    {

        public string ThreePLKey { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public int FacilityID { get; set; }
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber1 { get; set; }
        public string Fax { get; set; }
        public string EmailAddress1 { get; set; }
        public string CustomerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string LTESKU { get; set; }

        public string SIMSKU { get; set; }
        public string LTEQualifier { get; set; }
        public string SIMQualifier { get; set; }
        public decimal? SIMQty { get; set; }
        public decimal? LTEQty { get; set; }
        public string Carrier { get; set; }
        public string Mode { get; set; }
        public string BillingCode { get; set; }
        public string Account { get; set; }
        public string OrderReferenceNumber { get; set; }



    }
}