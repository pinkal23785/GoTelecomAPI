using GO.ITC.Ticket.Closure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO.ITC.Ticket.Closure
{
    public interface ITicketService
    {
        Task UpdateITCStatus();
    }
    public class TicketService : ITicketService
    {
        private readonly CADBContext _context;
        private readonly ILogger<TicketService> _logger;
        public TicketService(CADBContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<TicketService>();
        }
        public async Task UpdateITCStatus()
        {
            _logger.LogInformation("Start update the ITC ticket");
            try
            {
                string cuurentDatetimequery = "select SYSDATE from dual";

                var oracleDateTime = await _context.CurrentOracleDateTimes.FromSqlRaw(cuurentDatetimequery).FirstOrDefaultAsync();
                //oracleDateTime.CurrentDate

                var TicketList = await _context.TicketMasters.Where(x => x.OPERATOR_ID.ToLower() == "itc" 
                && x.STATUS.ToLower() == "resolved").ToListAsync();

                foreach(var t in TicketList)
                {
                    if(oracleDateTime.SYSDATE.Date.Subtract(t.MODIFIED_T.Date).TotalHours > 48)
                    {
                        t.STATUS = "Closed";
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message + " Stack Track:" + ex.StackTrace);
                throw ex;
            }
        }
    }
}
