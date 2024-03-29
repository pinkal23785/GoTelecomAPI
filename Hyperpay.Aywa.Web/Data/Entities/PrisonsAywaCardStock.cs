using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Data.Entities
{
    public class PrisonsAywaCardStock
    {
        public int ID { get; set; }
        public int CARD_TYPE_ID { get; set; }
        public string BATCHNO { get; set; }
        public string CONTROLNO { get; set; }
        public string CARDNUMBER { get; set; }
        public string STATUS { get; set; }
        public DateTime EXPIRY { get; set; }
        public decimal BALANCE { get; set; }
        public DateTime? INSERT_DATE { get; set; }
        public string INSERT_BY { get; set; }
        public DateTime? LAST_MODIFY_DATE { get; set; }
        public string LAST_MODIFY_BY { get; set; }
    }
}
