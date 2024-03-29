using GO.Service.DeviceExtender.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GO.Service.DeviceExtender.Data
{
    public interface IDataService
    {
        Task AddOTP(CustomerOTP oTP);
        Task<string> VerifyOTP(string MobileNumber, string OTP);
        

    }
}
