using GO.Order.Tracking.Service.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GO.Order.Tracking.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly CADBContext _context;
        public OrderController(CADBContext context)
        {
            _context = context;
        }
        [HttpGet("get-delivery")]
        public async Task<IActionResult> GetDelivery([FromQuery] string OrderID, [FromQuery] string Operator)
        {
            string SQL = $@"select Activity,Activity_Desc,Activity_Status,NotesLog from OA_ORDER_TRACKING_POINT WHERE ORDER_ID='{OrderID}'";

            if (!string.IsNullOrEmpty(Operator))
            {
                SQL = SQL + $@" AND Service_Provider=UPPER('{Operator}')";
            }

            var result = await _context.OrderTrackingPoints.FromSqlRaw(SQL).ToListAsync();
            return Ok(result);

        }
        [HttpGet("get-milestones")]
        public async Task<IActionResult> GetMilestone([FromQuery] string TicketID, [FromQuery] string Operator)
        {
            string SQL = $@"select State,State_Reason,Activity,Activity_Desc from TICKET_ACTIVITY_LOG Where Ticket_ID='{TicketID}' ";
            if (!string.IsNullOrEmpty(Operator))
            {
                SQL = SQL + $@" and Operator=UPPER('{Operator}')";
            }
            var result = await _context.MileStones.FromSqlRaw(SQL).ToListAsync();
            return Ok(result);
        }
        [HttpGet("get-exchange-notes")]
        public async Task<IActionResult> GetExchangeNotes([FromQuery] string TicketID, [FromQuery] string Operator)
        {
            string SQL = $@"select NotesLog,Work_Info,Work_Info_Summary, Operation from ticket_Tracking_Point Where SRNUMBER='{TicketID}'";
            if (!string.IsNullOrEmpty(Operator))
            {
                SQL = SQL + $@" and Operator=UPPER('{Operator}')";
            }
            var result = await _context.ExchangeNotes.FromSqlRaw(SQL).ToListAsync();
            return Ok(result);
        }
    }
}
