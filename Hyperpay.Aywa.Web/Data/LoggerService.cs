using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Data
{
    public interface ILoggerService
    {
        Task AddLog(string Log);
    }
    public class LoggerService : ILoggerService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly CADBContext _context;
        public LoggerService(ILogger<LoggerService> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;

        }

        public async Task AddLog(string Log)
        {
            if (_configuration.GetValue<string>("EnableLog") == "1")
                _logger.LogInformation(Log);
        }
    }
}
