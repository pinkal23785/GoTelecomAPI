using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.SADAD
{
    public class SADADUploadResponse
    {
        public string BillNumber { get; set; }
        public decimal Amount { get; set; }

        public string Lang { get; set; }
    }
}
