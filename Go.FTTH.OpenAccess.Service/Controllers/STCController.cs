using Go.FTTH.OpenAccess.Service.Data;
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
    public class STCController : ControllerBase
    {

        
        private readonly ILogger _logger;
        private readonly IDataService _dataService;
        public STCController(ILogger<STCController> logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }
        [HttpGet("get-stc-ticket-list")]
        public async Task<IActionResult> GetSTCAllTicket()
        {
            try
            {
                _logger.LogInformation("get all ticket");
                var result = await _dataService.GetAllSTCTicket();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("get-stc-ticket-detail")]
        public async Task<IActionResult> GetSTCTicketDetail(string TicketID)
        {
            try
            {
                _logger.LogInformation("get all ticket");
                var result = await _dataService.GetSTCTicketDetail(TicketID);
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
