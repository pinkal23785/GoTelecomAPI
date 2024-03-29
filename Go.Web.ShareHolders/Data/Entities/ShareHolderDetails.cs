using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Web.ShareHolders.Data.Entities
{
    public class ShareHolderDetails
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="ID Number required")]
        public Int64? ID_Number { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage ="Name is required")] 
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string POBOX { get; set; }
        public string ZipCode { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^((\+|00)9665|0?5)([013-9][0-9]{7})$", ErrorMessage = "Not a valid number")]
        public string MobileNumber { get; set; }
        public DateTime CTimeStamp { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string OTP { get; set; }

        [NotMapped]
        public string Culture { get; set; }


    }
}
