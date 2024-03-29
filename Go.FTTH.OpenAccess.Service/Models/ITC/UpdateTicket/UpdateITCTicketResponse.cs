using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.ITC.UpdateTicket
{
    public class result
    {
        public string Result { get; set; }
    }

    public class UpdateITCTicketResponse
    {
        public string res_code { get; set; }
        public string res_message { get; set; }
        public result result { get; set; }
    }


   
}
