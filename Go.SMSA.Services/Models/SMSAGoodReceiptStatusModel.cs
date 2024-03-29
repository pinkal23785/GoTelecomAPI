using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GO.SMSA.Service.Models.GoodReceipts
{
   

    public class Items
    {
        public string batch { get; set; }
        public string itemNumber { get; set; }
        public string mfgDate { get; set; }
        public string packageSize { get; set; }
        public string plant { get; set; }
        public string qty { get; set; }
        public string shelfLifeExpiryDate { get; set; }
        public string sku { get; set; }
        public string storageLocation { get; set; }
        public string uom { get; set; }
    }

    public class GoodReceiptStatusModel
    {
        public string billOfLanding { get; set; }
        public string deliveryNote { get; set; }
        public string documentDate { get; set; }
        public string form1Number { get; set; }
        public List<Items> items { get; set; }
        public string movementType { get; set; }
        public string ponumber { get; set; }
        public string postingDate { get; set; }
        public string vendorCode { get; set; }
    }

}
