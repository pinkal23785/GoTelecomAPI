using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Models
{
    public class VerifyOTPModel
    {
        public string Mobile { get; set; }
        

        public string Email { get; set; }
        [Required(ErrorMessage = "You must provide a OTP")]
        public string OTP { get; set; }

        public string AwayaCardType { get; set; }
        public decimal CardAmount { get; set; }

        public string Lang { get; set; }
    }
}
