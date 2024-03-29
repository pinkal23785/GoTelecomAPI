using Go.FTTH.OpenAccess.Service.Data;
using Go.FTTH.OpenAccess.Service.Models.ITC.CloseTicket;
using Go.FTTH.OpenAccess.Service.Models.ITC.NewTicket;
using Go.FTTH.OpenAccess.Service.Models.ITC.OpenTicket;
using Go.FTTH.OpenAccess.Service.Models.ITC.UpdateTicket;
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
    public class ITCController : Controller
    {
        private readonly IITCService ITCService;
        private readonly ILogger _logger;
        private readonly IDataService _dataService;
        public ITCController(IITCService _ITCService, ILogger<ITCController> logger, IDataService dataService)
        {
            ITCService = _ITCService;
            _logger = logger;
            _dataService = dataService;
        }

        [HttpPost("create-itc-ticket")]
        public async Task<IActionResult> CreateITCCase(NewITCTicketRequest model)
        {
            try
            {
                _logger.LogInformation("Create ITC Ticket");
                var result = await ITCService.CreateITCTicket(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("update-itc-ticket")]
        public async Task<IActionResult> UpdateITCCase(UpdateITCTicketRequest model)
        {
            try
            {
                _logger.LogInformation("Update ITC Ticket");
                var result = await ITCService.UpdateITCTicket(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("repoen-itc-ticket")]
        public async Task<IActionResult> ReopenITCCase(ReOpenITCTicketRequest model)
        {
            try
            {
                _logger.LogInformation("ReOpen ITC Ticket");
                var result = await ITCService.ReopenITCTicket(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("close-itc-ticket")]
        public async Task<IActionResult> CloseITCCase(CloseITCTicketRequest model)
        {
            try
            {
                _logger.LogInformation("Close ITC Ticket");
                var result = await ITCService.CloseITCTicket(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("get-itc-ticket-detail")]
        public async Task<IActionResult> GetTicketITCDetail(string TicketID)
        {
            try
            {
                _logger.LogInformation("get Case");
                var result = await _dataService.GetTicketITCDetail(TicketID);
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
