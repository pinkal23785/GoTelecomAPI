using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class Customer
    {
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public string PRIORITY { get; set; }
    }
}
