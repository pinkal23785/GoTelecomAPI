using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Models.Dawiyat.UpdateCaseStatus
{
    public class Result
    {
        public string case_number { get; set; }
        public string sys_id { get; set; }
    }

    public class UpdateDawiyatCaseStatusRespose
    {
        public Result result { get; set; }
    }
}
