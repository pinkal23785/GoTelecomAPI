using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Web.ContactUs.Models
{
    public class ContactModel
    {
        public ContactModel()
        {
            TopicList = new List<Itemlist>() {
                        new Itemlist { Text = "Select Topic", Value = 0 },
                        new Itemlist { Text = "Products & Services", Value = 1 },
                        new Itemlist { Text = "Coverage", Value = 2 },
                        new Itemlist { Text = "Complain", Value = 3 },
                        new Itemlist { Text = "Suggestion", Value = 4 },
                        new Itemlist { Text = "Others", Value = 5 },
                        };
        }
        public string Name { get; set; }
        public int Topic { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Comments { get; set; }
        public List<Itemlist> TopicList { get; set; }
        public string CaptchaCode { get; set; }
    }

    public class Itemlist
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }
}
