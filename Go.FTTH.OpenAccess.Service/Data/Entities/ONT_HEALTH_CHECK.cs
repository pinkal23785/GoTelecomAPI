using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class ONT_HEALTH_CHECK
    {
        public int ID { get; set; }
        public string Operator { get; set; }
        public DateTime Created_DT { get; set; }
        public string Service_Account_Number { get; set; }

        public string UserID { get; set; }
        public string AccountID { get; set; }
    }
}
