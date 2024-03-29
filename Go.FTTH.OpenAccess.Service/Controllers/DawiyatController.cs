using Go.FTTH.OpenAccess.Service.Data;
using Go.FTTH.OpenAccess.Service.Data.Entities;
using Go.FTTH.OpenAccess.Service.Models.Dawiyat;
using Go.FTTH.OpenAccess.Service.Models.Dawiyat.CreateCase;
using Go.FTTH.OpenAccess.Service.Models.Dawiyat.UpdateCaseStatus;
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
    public class DawiyatController : ControllerBase
    {
        private readonly IDawiyatService dawiyatService;
        private readonly ILogger _logger;
        private readonly IDataService _dataService;
        public DawiyatController(IDawiyatService _dawiyatService, ILogger<DawiyatController> logger, IDataService dataService)
        {
            dawiyatService = _dawiyatService;
            _logger = logger;
            _dataService = dataService;
        }

        [HttpPost("Auth")]
        public async Task<IActionResult> GetBearerToken()
        {
            try
            {
                _logger.LogInformation("Call Get Bearer Token");
                var result = await dawiyatService.AuthDawiyatClient();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        [HttpGet("user-feasibility")]
        public async Task<IActionResult> getUserFeasibility(string obdId)
        {
            try
            {
                _logger.LogInformation("getUserFeasibility");
                var result = await dawiyatService.CustomerFeasibility(obdId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        [HttpPost("create-case")]
        public async Task<IActionResult> CreateCase(NewCaseRequestModel model)
        {
            try
            {
                _logger.LogInformation("Create Case");
                var result = await dawiyatService.CreateCase(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("create-ticket")]
        public async Task<IActionResult> CreateTicket(TicketMaster model)
        {
            try
            {
                _logger.LogInformation("Create Case");
                await _dataService.CreateNewTicketAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        [HttpPut("update-dawiyat-ticket-status")]
        public async Task<IActionResult> UpdateDawiyatTicketStatus(UpdateDawiyatCaseStatusRequest model)
        {
            try
            {
                _logger.LogInformation("call back case status");
                var result = await dawiyatService.UpdateDawiyatTicketStatus(model);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("get-ticket-list")]
        public async Task<IActionResult> GetTicketList(string UserID)
        {
            try
            {
                _logger.LogInformation("get all ticket");
                var result = await _dataService.GetAllTickets(UserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("get-dawiyat-case-detail")]
        public async Task<IActionResult> GetDawiyatCase(string CaseNumber)
        {
            try
            {
                _logger.LogInformation("get Case");
                var result = await dawiyatService.GetDawiyatCaseDetail(CaseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
