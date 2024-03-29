using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Data.Entities
{
    public class CustomerOTP
    {

        public int ID { get; set; }
        public string MOBILENUMBER { get; set; }
        public string EMAIL { get; set; }
        public string OTP { get; set; }
        public string ISVERIFIED { get; set; }
        public DateTime DATE_CREATED { get; set; }

        public DateTime OTP_EXPIRE_DATE { get; set; }
    }
}
