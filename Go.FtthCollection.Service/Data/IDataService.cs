using Go.FtthCollection.Service.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data
{
    public interface IDataService
    {
        Task<AccountDetail> GetAccountDetail(string AccountID);
        Task UpdateAccount(string AccountID, string SubmittedTo, string ModifyBy);

    }
}
