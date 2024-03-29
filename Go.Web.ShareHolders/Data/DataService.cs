
using Go.Web.ShareHolders.Data;
using Go.Web.ShareHolders.Data.Entities;
using GO.Web.ShareHolders.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO.Web.ShareHolders.Data
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

        public async Task AddOTP(CustomerOTP oTP)
        {
            await _context.CustomerOTPs.AddAsync(oTP);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsIDNumberExist(Int64 idNumber)
        {
            var result = await _context.ShareHolderDetails.Where(x => x.ID_Number == idNumber).ToListAsync();
            if (result.Count() > 0)
                return true;

            return false;
        }
        public async Task<string> VerifyOTP(ShareHolderDetails details)
        {
            var result = await _context.CustomerOTPs.Where(x => x.MOBILENUMBER == details.MobileNumber && x.OTP == details.OTP && x.OTP_EXPIRE_DATE > DateTime.Now).FirstOrDefaultAsync();
            if (result != null)
            {
                result.IS_VERIFIED = "1";
                await _context.SaveChangesAsync();
                var shareHolder = new ShareHolderDetails()
                {
                    Address = details.Address,
                    City = details.City,
                    Email = details.Email,
                    FullName = details.FullName,
                    MobileNumber = details.MobileNumber,
                    POBOX = details.POBOX,
                    Status = details.Status,
                    ZipCode = details.ZipCode,
                    CTimeStamp = DateTime.Now,
                    ID_Number = details.ID_Number
                };
                await _context.AddAsync(shareHolder);
                await _context.SaveChangesAsync();
                return "Success";
            }
            return "Failed";
        }
    }
}
