using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class SearchDetail
    {
        public string TICKET_ID { get; set; }
        public string ORDER_ID { get; set; }
        public string ACCOUNT_ID { get; set; }
        public string PLANNAME { get; set; }
        public string DETAIL_DESC { get; set; }
        public string TICKETSTATUS { get; set; }
        public string OPERATOR_ID { get; set; }
        public string ORERATOR_REF { get; set; }
        public string CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string LAST_MODIFIED { get; set; }
        public string COMMENTS { get; set; }
    }
}
