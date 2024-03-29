using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Data.Entities
{
    public class PrisonsAywaaCardType
    {

        public int ID { get; set; }
        public string NAME { get; set; }
        public string NAME_AR { get; set; }
        public decimal CREDIT { get; set; }
        public string DESCRIPTION { get; set; }
    }
}
