using GO.Service.DeviceExtender.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GO.Service.DeviceExtender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceExtenderController : ControllerBase
    {
        private readonly IOTPService _iOTPService;
        public DeviceExtenderController(IOTPService iOTPService)
        {
            _iOTPService = iOTPService;
        }
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP(string MobileNumber,string Email,string Lang)
        {
            try
            {
                await _iOTPService.SendOTP(MobileNumber,Email,Lang);
                return Ok("success");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP(string MobileNumber, string OTP)
        {
            try
            {
                string result = await _iOTPService.VerifyOTP(MobileNumber, OTP);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
