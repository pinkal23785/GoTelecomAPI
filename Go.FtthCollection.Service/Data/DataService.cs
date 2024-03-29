
using Go.FtthCollection.Service.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data
{
    public class DataService : IDataService
    {
        private CADBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DataService(CADBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AccountDetail> GetAccountDetail(string AccountID)
        {
            string query = "select acc.* , grp.COLL_GROUP_NAME,acctbl.PRE_POST_FLAG IS_PRE_POST  from " +
            " OA_FTTH_COLLECTION_ACCOUNTS acc, OA_FTTH_COLLECTION_GROUPS grp,ACCOUNT_INFO_TBL acctbl" +
            " where acc.COLL_GROUP_ID = grp.COLL_GROUP_ID and acc.ACCOUNT_ID=acctbl.ACCOUNT_ID and acctbl.PRE_POST_FLAG is not null and acc.ACCOUNT_ID ='" + AccountID + "'";
            var accountDetail = await _context.AccountDetails.FromSqlRaw(query).FirstOrDefaultAsync();
            return accountDetail;
        }

        public async Task UpdateAccount(string AccountID, string SubmittedTo, string ModifyBy)
        {
            string query = @"Update OA_FTTH_COLLECTION_ACCOUNTS SET IS_SUBMITTED=1, SUBMITTED_TO='" + SubmittedTo + 
                "', LAST_MODIFY_BY='" + ModifyBy +"', LAST_MODIFY_DATE='"+ DateTime.UtcNow.ToString("dd-MMM-yyyy") +"'"+ 
                " WHERE ACCOUNT_ID='" + AccountID + "'";
            await _context.Database.ExecuteSqlRawAsync(query);

        }
    }
}
