using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Service.NumberBooking.Data.Entities
{
    public class GTSNumberRange
    {

        public int Range_GTS_Id { get; set; }
        public int? Range_GTS_CATID { get; set; }
        public string Range_GTS_CityID { get; set; }
        public string Range_GTS_Number_From { get; set; }
        public string Range_GTS_Number_To { get; set; }
        public int? Range_GTS_Status { get; set; }
        public DateTime? Range_GTS_Date_Booked { get; set; }
        public DateTime? Range_GTS_Date_Expiry { get; set; }
        public string Range_Release_By { get; set; }
        public DateTime? Range_Release_On { get; set; }
        public int? Range_Release_Type { get; set; }
        public string Range_GTS_Account_Manager { get; set; }
        public string Range_GTS_AM_Email { get; set; }
        public string Range_GTS_Customer_Id { get; set; }
        public string Range_GTS_Approved_by { get; set; }
        public DateTime? Range_GTS_Approved_On { get; set; }
        public string Range_GTS_Consumed_by { get; set; }
        public DateTime? Range_GTS_Consumed_On { get; set; }
        public int? Range_GTS_Network_Status { get; set; }
        public DateTime? Range_GTS_Cancel_On { get; set; }
        public DateTime? Range_GTS_Prov_NonProv { get; set; }
    }
}
