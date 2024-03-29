using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Models
{
    public class OTPModel
    {
        [Required]
        [RegularExpression(@"^((\+|00)9665|0?5)([013-9][0-9]{7})$", ErrorMessage = "Not a valid number")]
        public string Mobile { get; set; }
        [Required]
        public string Email { get; set; }
        public string Lang { get; set; }
        public string AwayaCardType { get; set; }
        public decimal CardAmount { get; set; }


    }
}
