using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace GO.Process.EInvoiceProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            Startup startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>();
            //.AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>();
            logger.AddFile("Logs/E-InvoiceProcess-{Date}.txt");

            var service = serviceProvider.GetService<IProcessInvoice>();
            service.Process().ConfigureAwait(true);
        }
    }
}
