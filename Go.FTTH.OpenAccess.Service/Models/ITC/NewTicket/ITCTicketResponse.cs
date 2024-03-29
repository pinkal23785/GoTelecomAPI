using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.ITC.NewTicket
{
    public class Result
    {
        public string Request_ID { get; set; }
        public string Status { get; set; }
    }

    public class ITCTicketResponse
    {
        public string res_code { get; set; }
        public string res_message { get; set; }
        public Result result { get; set; }
    }
    
}
