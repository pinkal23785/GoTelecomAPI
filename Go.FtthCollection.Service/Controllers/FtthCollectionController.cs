using Go.FtthCollection.Service.Models;
using Go.FtthCollection.Service.Services;
using Go.LTEWallet.Services.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FtthCollection.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FtthCollectionController : ControllerBase
    {
        private IDataService dataService;
        private IExternalService externalService;
        public FtthCollectionController(IDataService _dataService, IExternalService _externalService)
        {
            dataService = _dataService;
            externalService = _externalService;
        }

        [HttpGet("search-collection-account-detail")]
        public async Task<IActionResult> SearchCollectionAccountDetail(string AccountID)
        {
            try
            {
                var result = await dataService.GetAccountDetail(AccountID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("installment-payment-order")]
        public async Task<IActionResult> InstallmentPaymentOrder(InstallmentPaymentRequest request)
        {
            try
            {
                var result = await externalService.InstallmentPaymentOrder(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("discount-payment-order")]
        public async Task<IActionResult> DiscountPaymentOrder(DiscountPaymentOrderReq request)
        {
            try
            {
                var result = await externalService.DiscountPaymentOrder(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
