using Go.LTEWallet.Services.Data.Entities;
using Go.LTEWallet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data
{
    public interface IDataService
    {
        Task<List<CommissionPlan>> GetCommissionPlan();
        Task<CustomerWalletBalance> GetCommissionWalletBalance(string MobileNumber);
        Task<bool> ValidateWalletBalance(string MobileNumber, string PlanId);
        Task<dynamic> GetPlanVoucher(string MobileNumber, string PlanId);
        Task<string> RedeemVoucher(string MobileNumber, string PlanId, string SerialNumber);

        Task<string> RedeemVoucherByMerchantID(string MerchantID, string PlanId, string SerialNumber);

        Task<VoucherDetail> GetVoucherDetails(string SerialNumber);

        Task UpdateVoucherStatus(string SerialNumber);

        Task<List<RenewalOrderLogs>> GetRenewalOrderLogs(string MobileNumber);

        Task AddOTP(CustomerOTP oTP);

        Task<string> UpdateMechantWithMobile(string Name, string OldMobile, string NewMobile, string City, string OTP);
        Task<string> UpdateMechantInfo(string Name, string Mobile, string City);

        Task<Order> GetOrder(string MobileNumber);

    }
}
