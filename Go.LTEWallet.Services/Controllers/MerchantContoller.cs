using Go.LTEWallet.Services.Data;
using Go.LTEWallet.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantContoller : ControllerBase
    {
        private readonly IDataService dataService;
        private readonly IOTPService OTPService;
        public MerchantContoller(IDataService _dataService, IOTPService _OTPService)
        {
            dataService = _dataService;
            OTPService = _OTPService;
        }

        [HttpGet("get-renew-orders")]
        public async Task<IActionResult> GetRenewOrder(string MobileNumber)
        {
            try
            {
                var result = await dataService.GetRenewalOrderLogs(MobileNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP(string MobileNumber, string Lang)
        {
            try
            {
                var result = await OTPService.SendOTP(MobileNumber, Lang);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-merchant-info-with-new-mobile")]
        public async Task<IActionResult> UpdateMerchantWithMobile(MerchantUserModel model)
        {
            try
            {
                var result = await dataService.UpdateMechantWithMobile(model.Name, model.OldMobile, model.NewMobile, model.City, model.OTP);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-merchant-info-without-mobile")]
        public async Task<IActionResult> UpdateMerchant(MerchantInfoModel model)
        {
            try
            {
                var result = await dataService.UpdateMechantInfo(model.Name, model.Mobile, model.City);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
