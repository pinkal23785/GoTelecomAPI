using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Dawiyat.UpdateCaseStatus
{
    public class UpdateDawiyatCaseStatusRequest
    {
        public string u_trouble_ticket_number { get; set; }
        public string state { get; set; }
        public string comments { get; set; }
        //public string sys_id { get; set; }
    }
}
