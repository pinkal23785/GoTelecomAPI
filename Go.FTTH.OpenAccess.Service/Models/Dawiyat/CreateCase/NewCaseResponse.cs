using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Dawiyat.CreateCase
{
    public class Result
    {
        public string number { get; set; }
        public string sys_id { get; set; }
        public string u_trouble_ticket_number { get; set; }
    }

    public class NewCaseResponse
    {
        public Result result { get; set; }
    }
}
