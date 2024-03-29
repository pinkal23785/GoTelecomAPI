using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Go.SMSA.Services.ResponseModels
{
    public class SIMDetailsResponseModel
    {
        public string IMSI { get; set; }
        public string MSISDN { get; set; }
        public string ICCID { get; set; }
        public string MACID { get; set; }
        public string CPE_MODEL { get; set; }
        public int STATUS { get; set; }


    }
}