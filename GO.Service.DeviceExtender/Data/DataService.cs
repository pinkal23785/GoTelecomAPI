using Go.Service.DeviceExtender.Data;
using GO.Service.DeviceExtender.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO.Service.DeviceExtender.Data
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

        public async Task<string> VerifyOTP(string MobileNumber, string OTP)
        {
            var result = await _context.CustomerOTPs.Where(x => x.MOBILENUMBER == MobileNumber && x.OTP == OTP && x.OTP_EXPIRE_DATE > DateTime.Now).FirstOrDefaultAsync();
            if (result != null)
            {
                result.IS_VERIFIED = "1";
                await _context.SaveChangesAsync();
                return "Success";
            }
            return "Failed";
        }
    }
}
