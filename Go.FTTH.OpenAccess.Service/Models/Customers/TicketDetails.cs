using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Customers
{
    public class TicketDetails
    {
        public string PIV_ORDER_ID { get; set; }
        public string PIV_SUB_ID { get; set; }
        public string PIV_OPERATOR_ID { get; set; }
        public string PIV_REGION { get; set; }
        public string PIV_CITY { get; set; }
        public string PIV_DISTRICT { get; set; }
        public string PIV_SERIAL_NUMBER { get; set; }
        public string PIV_MACID { get; set; }
        public string PIV_OPERATOR_REF { get; set; }
        public string PIV_ODB_ID { get; set; }
        public string PIV_CUST_ID { get; set; }
        public string PIV_CONTACT_NO { get; set; }
        public string PIV_CIRCUIT_ID { get; set; }
        public string POV_STATUS { get; set; }
        
        [JsonPropertyName("SERVICE_PROVIDER_ID")]
        public string SERVICE_PROVIDER_ID { get; set; }
        public string ShowContactAccount { get; set; }
    }
}
