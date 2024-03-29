using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Models
{
    public class CardModel
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select card type")]
        public int CardId { get; set; }

        public List<Itemlist> CardList { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select file")]
        public IFormFile file { get; set; }


    }

    public class Itemlist
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }
}
