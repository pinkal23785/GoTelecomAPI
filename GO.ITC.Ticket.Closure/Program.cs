using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace GO.ITC.Ticket.Closure
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            // Startup.cs finally :)
            Startup startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>();
            //.AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>();
            logger.AddFile("Logs/ITCProcess-{Date}.txt");

            //var logger = serviceProvider.GetService<ILoggerFactory>()
            //    .CreateLogger<Program>();

            var service = serviceProvider.GetService<ITicketService>();
            service.UpdateITCStatus().ConfigureAwait(true);
        }
    }
}
