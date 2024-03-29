using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Dawiyat.CreateCase
{

    public class NewCaseRequestModel
    {
        public string AccountId { get; set; }
        public string Operator { get; set; }
        public string UserID { get; set; }
        public string OrderID { get; set; }
        public NewCaseModel NewCase { get; set; }
    }
    public class NewCaseModel
    {

        public string u_trouble_ticket_number { get; set; }
        public string business_service { get; set; }
        public string u_product { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string subcategory_L2 { get; set; }
        public string state { get; set; }
        public string u_customer_name { get; set; }
        public string u_customer_order_number { get; set; }
        public string u_customer_contact_number { get; set; }
        public string u_odb_id { get; set; }
        public string short_description { get; set; }
        public string description { get; set; }
        public string u_circuit_id { get; set; }
        public string u_region { get; set; }
        public string u_city { get; set; }
        public string u_district { get; set; }
        public string u_dawiyat_building_id { get; set; }
        public string u_saudi_national_address { get; set; }
        public string olo_customer_id { get; set; }
        public string daw_service_id { get; set; }
        public string account { get; set; }
        public string contact { get; set; }
    }
}
