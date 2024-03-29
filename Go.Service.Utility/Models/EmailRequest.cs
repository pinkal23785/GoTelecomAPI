using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Service.Utility.Models
{
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string htmlBody { get; set; }
    }
}
