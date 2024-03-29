using Go.FTTH.OpenAccess.Service.Data;
using Go.FTTH.OpenAccess.Service.Models;
using Go.FTTH.OpenAccess.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ONTController : ControllerBase
    {
        private readonly IITCService ITCService;
        private readonly IDawiyatService DawiyatService;
        private readonly IMobilyService MobilyService;
        private readonly ILogger _logger;
        private readonly IDataService _dataService;
        public ONTController(IITCService _ITCService, ILogger<ONTController> logger, IDataService dataService, IDawiyatService _dawiyatService, IMobilyService _mobilyService)
        {
            ITCService = _ITCService;
            _logger = logger;
            _dataService = dataService;
            DawiyatService = _dawiyatService;
            MobilyService = _mobilyService;
        }

        [HttpPost("create-ONT-request")]
        public async Task<IActionResult> CreateONTRequest(ONTRequest model)
        {
            try
            {
                if (model.Operator.ToLower() == "itc")
                {
                    _logger.LogInformation("Create ITC ONT Request");
                    var result = await ITCService.ITCONTInquiry(model);
                    return Ok(result);
                }
                else if (model.Operator.ToLower() == "dawiyat")
                {
                    _logger.LogInformation("Create Dawiyat ONT Request");
                    var result = await DawiyatService.GetONTInquiry(model);
                    return Ok(result);
                }
                else if (model.Operator.ToLower() == "mobily")
                {
                    _logger.LogInformation("Create Dawiyat ONT Request");
                    var result = await MobilyService.GetMobilyONTStatus(model);
                    return Ok(result);
                }
                else
                    throw new Exception("Please pass valid operator");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status200OK, ex.Message);
            }
        }
        //[HttpPost("create-mobily-ONT-latency-request")]
        //public async Task<IActionResult> CreateMobilyONTLatencyRequest(ONTRequest model)
        //{
        //    try
        //    {
        //        if (model.Operator.ToLower() == "mobily")
        //        {
        //            _logger.LogInformation("Create Dawiyat ONT Request");
        //            var result = await MobilyService.GetONTLatencyMobily(model);
        //            return Ok(result);
        //        }
        //        else
        //            throw new Exception("Please pass valid operator");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(StatusCodes.Status200OK, ex.Message);
        //    }
        //}

        [HttpGet("get-ONT-list")]
        public async Task<IActionResult> GetONTList(string UserID)
        {
            try
            {
                _logger.LogInformation("Create ITC ONT Request");
                var result = await _dataService.GetONTList(UserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message);
            }
        }

        [HttpGet("get-ONT-list-by-account")]
        public async Task<IActionResult> GetONTListByAccount(string AccountID)
        {
            try
            {
                _logger.LogInformation("Create ITC ONT Request");
                var result = await _dataService.GetONTListByAccount(AccountID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message);
            }
        }

    }
}

