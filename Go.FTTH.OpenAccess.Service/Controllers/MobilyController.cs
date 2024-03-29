using Go.FTTH.OpenAccess.Service.Data;
using Go.FTTH.OpenAccess.Service.Models.Mobily.CloseSR;
using Go.FTTH.OpenAccess.Service.Models.Mobily.NewSR;
using Go.FTTH.OpenAccess.Service.Models.Mobily.ReOpenSR;
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
    public class MobilyController : Controller
    {
        private readonly IMobilyService mobilyService;
        private readonly ILogger _logger;
        private readonly IDataService _dataService;
        public MobilyController(IMobilyService _mobilyService, ILogger<MobilyController> logger, IDataService dataService)
        {
            mobilyService = _mobilyService;
            _logger = logger;
            _dataService = dataService;
        }

        [HttpPost("create-service-request")]
        public async Task<IActionResult> CreateServiceRequest([FromBody]NewServiceRequestModel model)
        {
            try
            {
                _logger.LogInformation("Create Mobily Ticket");
                var result = await mobilyService.CreateNewSR(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("get-mobily-ticket-detail")]
        public async Task<IActionResult> GetMobilyTicketDetail(string Id)
        {
            try
            {
                var result = await mobilyService.GetMobilyTicketDetails(Id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("reopen-mobily-ticket")]
        public async Task<IActionResult> ReopenMobilyTicket(ReopenSRServiceRequest model)
        {
            try
            {
                _logger.LogInformation("Reopen Mobily Ticket");
                var result = await mobilyService.ReopenSR(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost("close-mobily-ticket")]
        public async Task<IActionResult> CloseMobilyTicket(CloseSRRequest model)
        {
            try
            {
                _logger.LogInformation("Close Mobily Ticket");
                var result = await mobilyService.CloseMobilyTicket(model);
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
