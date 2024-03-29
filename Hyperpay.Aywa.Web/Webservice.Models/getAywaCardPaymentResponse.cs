using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Webservice.Models
{
    [DataContract]
    public class getAywaCardPaymentResponse
    {
        public string responseId { get; set; }
        public string responseDesc { get; set; }
        public string response { get; set; }
    }
}
