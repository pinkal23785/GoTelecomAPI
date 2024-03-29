using Go.LTEWallet.Services.Data;
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
    public class CommissionWalletController : ControllerBase
    {
        private readonly IDataService dataService;
        public CommissionWalletController(IDataService _dataService)
        {
            dataService = _dataService;
        }


        [HttpGet("get-comission-plans")]
        public async Task<IActionResult> GetCommissionPlan()
        {
            try
            {
                var result = await dataService.GetCommissionPlan();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-wallet-balance")]
        public async Task<IActionResult> GetWalletBalance(string MobileNumber)
        {
            try
            {
                var result = await dataService.GetCommissionWalletBalance(MobileNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-plan-voucher")]
        public async Task<IActionResult> GetPlanVoucher(string MobileNumber, string PlanId)
        {
            try
            {
                var result = await dataService.GetPlanVoucher(MobileNumber, PlanId);
                if (result == null)
                {
                    return StatusCode((int)StatusCodes.Status404NotFound, "Voucher not found");
                }
                else
                {
                    return Ok(result.ToString());
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("get-voucher-detail")]
        public async Task<IActionResult> GetVoucherDetail(string SerialNumber)
        {
            try
            {
                var result = await dataService.GetVoucherDetails(SerialNumber);
                if (result == null)
                {
                    return Ok(false);
                }
                else
                {
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("validate-balance")]
        public async Task<IActionResult> ValidateBalance(string MobileNumber, string PlanId)
        {
            try
            {
                var result = await dataService.ValidateWalletBalance(MobileNumber, PlanId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("update-voucher-status-ready")]
        public async Task<IActionResult> UpdateVoucherStatus(string SerialNumber)
        {
            try
            {
                await dataService.UpdateVoucherStatus(SerialNumber);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("redeem-plan-voucher")]
        public async Task<IActionResult> RedeemPlanVoucher(string MobileNumber, string PlanId, string VoucherNo)
        {
            try
            {
                var result = await dataService.RedeemVoucher(MobileNumber, PlanId, VoucherNo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("redeem-plan-voucher-by-merchantID")]
        public async Task<IActionResult> RedeemPlanVoucherByMerchantID(string MerchantID, string PlanId, string VoucherNo)
        {
            try
            {
                var result = await dataService.RedeemVoucherByMerchantID(MerchantID, PlanId, VoucherNo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-activation-orders")]
        public async Task<IActionResult> GetOrder(string MobileNumber)
        {
            try
            {
                var result = await dataService.GetOrder(MobileNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
