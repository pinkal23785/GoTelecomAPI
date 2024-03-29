using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.SMSA.Services.ResponseModels
{
    public class AuthResponseModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public object refresh_token { get; set; }
        public object scope { get; set; }
    }
}
