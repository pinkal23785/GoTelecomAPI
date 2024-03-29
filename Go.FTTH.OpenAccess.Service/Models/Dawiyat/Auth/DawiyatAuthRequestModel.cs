using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Dawiyat
{
    public class DawiyatAuthRequestModel
    {
        public string client_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string grant_type { get; set; }
    }
}
