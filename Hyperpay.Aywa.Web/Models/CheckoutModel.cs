using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Models
{
    public class CheckoutModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the mobile number")]
        public string MobileNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the email")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string CardType { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
        
        public string State { get { return "Riyadh"; } }
        [Required(AllowEmptyStrings = false)]
        public string City { get; set; }
        
        public string StreetAddress { get { return "Riyadh"; } }
        
        public string PinCode { get { return "12242"; } }

        public decimal Amount { get; set; }

        public string AywaaCardID { get; set; }

        public string Lang { get; set; }
    }
}
