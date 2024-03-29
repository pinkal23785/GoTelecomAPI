using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Service.NumberBooking.Data.Entities
{
    public class SPNNumber
    {

        public int SPN_Id { get; set; }
        public int SPN_Cat_Id { get; set; }
        public string SPN_Number { get; set; }
        public int SPN_Status { get; set; }
        public DateTime? SPN_Date_Booked { get; set; }
        public DateTime? SPN_Date_Expiry { get; set; }
        public int? SPN_Customer_Id { get; set; }
        public string SPN_AM_Email { get; set; }
        public string SPN_Account_Manager { get; set; }
        public int? SPN_Number_Type { get; set; }
        public byte? SPN_Request_Validity { get; set; }
        public string SPN_Order_number { get; set; }
        public string SPN_Action_Taken { get; set; }
        public string SPN_Modified_By { get; set; }
        public int? SPN_NetworkStatus { get; set; }
        public DateTime? SPN_Cancel_Date { get; set; }
        public DateTime? SPN_Action_Update { get; set; }
    }
}
