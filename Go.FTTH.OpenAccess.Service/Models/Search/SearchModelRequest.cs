using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Search
{
    public class SearchModelRequest
    {
        public string TICKET_ID { get; set; }
        public string OPERATOR_ID { get; set; }
        public string ORDER_ID { get; set; }
        public string ACCOUNT_ID { get; set; }
        public string NUMB { get; set; }
        public string UserID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Identification_Value { get; set; }

        public string COMMENTS { get; set; }
    }
}
