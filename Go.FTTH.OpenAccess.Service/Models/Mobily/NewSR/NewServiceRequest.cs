using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Mobily.NewSR
{
    public class NewServiceRequestModel
    {
        public string AccountId { get; set; }
        public string Operator { get; set; }
        public string UserID { get; set; }
        public string OrderID { get; set; }
        public NewServiceRequest ServiceRequest { get; set; }
    }
    public class NewServiceRequest
    {

        public string Operation { get { return "OpenSR"; } }
        public string TransactionID { get; set; }
        public string ServiceAccNum { get; set; }
        public string CustomerType { get { return "Consumer"; } }
        public string SRType { get { return "Open Access"; } }
        public string SRArea { get; set; }
        public string SRSubArea { get; set; }
        public string Channel { get; set; }
        public string Description { get; set; }
        public string Status { get { return "Assigned"; } }
        public string SubStatus { get { return "Unassigned"; } }
        public string ServiceOwnerName { get; set; }
        public string ServiceOwnerNumber { get; set; }
        public string Flex1 { get; set; }
        public string Flex2 { get; set; }
    }
}
