using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class TicketDawiyatDetail
    {
        public int ID { get; set; }
        [JsonProperty("u_trouble_ticket_number")]
        public string TICKET_ID { get; set; }
        [JsonProperty("business_service")]
        public string BUSINESS_SERVICE { get; set; }
        [JsonProperty("u_product")]
        public string PRODUCT { get; set; }
        [JsonProperty("category")]
        public string CATEGORY { get; set; }
        [JsonProperty("subcategory")]
        public string SUBCATEGORY { get; set; }
        [JsonProperty("u_subcategory_l2")]
        public string SUBCATEGORY_L2 { get; set; }
        [JsonProperty("u_customer_name")]
        public string CUSTOMER_NAME { get; set; }
        [JsonProperty("u_customer_order_number")]
        public string CUSTOMER_ORDER_NUMBER { get; set; }
        [JsonProperty("u_customer_contact_number")]
        public string CUSTOMER_CONTACT_NUMBER { get; set; }
        [JsonProperty("u_odb_id")]
        public string OBDID { get; set; }
        [JsonProperty("short_description")]
        public string SHORT_DESC { get; set; }
        [JsonProperty("description")]
        public string DESC { get; set; }
        [JsonProperty("u_circuit_id")]
        public string CIRCUIT_ID { get; set; }
        [JsonProperty("u_region")]
        public string REGION { get; set; }
        [JsonProperty("u_city")]
        public string CITY { get; set; }
        [JsonProperty("u_district")]
        public string DISTRICT { get; set; }
        [JsonProperty("u_dawiyat_building_id")]
        public string DAWIYAT_BUILDING_ID { get; set; }
        [JsonProperty("u_saudi_national_address")]
        public string SAUDI_NATION_ADDRESS { get; set; }
        [JsonProperty("olo_customer_id")]
        public string OLO_CUSTOMER_ID { get; set; }
        [JsonProperty("daw_service_id")]
        public string DAW_SERVICE_ID { get; set; }
        [JsonProperty("account")]
        public string ACCOUNT { get; set; }
        [JsonProperty("contact")]
        public string CONTACT { get; set; }
        [NotMapped]
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
