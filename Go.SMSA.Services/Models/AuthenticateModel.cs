using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.SMSA.Services.Models
{
    public class AuthenticateModel
    {
        public string grant_type { get; set; }
        public string tpl { get; set; }
        public string user_login_id { get; set; }
    }
}
