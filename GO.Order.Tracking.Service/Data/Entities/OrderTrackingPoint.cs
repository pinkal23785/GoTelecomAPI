using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GO.Order.Tracking.Service.Data.Entities
{
    public class OrderTrackingPoint
    {
        public string Activity { get; set; }
        public string Activity_Desc { get; set; }
        public string Activity_Status { get; set; }
        public string NotesLog { get; set; }
    }

    public class MileStone
    {
        public string State { get; set; }
        public string State_Reason { get; set; }
        public string Activity { get; set; }
        public string Activity_Desc { get; set; }
    }
    public class ExchangeNote
    {
        public string NotesLog { get; set; }
        public string Work_Info { get; set; }
        public string Work_info_Summary { get; set; }
        public string Operation { get; set; }
    }
}
