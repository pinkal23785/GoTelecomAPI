
using Go.Web.ShareHolders.Data.Entities;
using GO.Web.ShareHolders.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GO.Web.ShareHolders.Data
{
    public interface IDataService
    {
        Task AddOTP(CustomerOTP oTP);
        Task<string> VerifyOTP(ShareHolderDetails details);
        Task<bool> IsIDNumberExist(Int64 idNumber);

        

    }
}
