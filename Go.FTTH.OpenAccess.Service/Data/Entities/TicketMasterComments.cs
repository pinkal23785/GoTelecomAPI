using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class TicketMasterComment
    {
        public int ID { get; set; }
        public int TicketMasterID { get; set; }
        public string Comments { get; set; }
        public DateTime Created { get; set; }

        public string Work_Info { get; set; }
        public string Work_Info_Summary { get; set; }
    }
}
