using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Go.Process.InvoiceURLProducer
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
            logger.AddFile("Logs/InvoiceProcess-{Date}.txt");

            //var logger = serviceProvider.GetService<ILoggerFactory>()
            //    .CreateLogger<Program>();

            var service = serviceProvider.GetService<ICustomerInvoiceService>();


            Console.WriteLine("Please Choose the option below to proceed");
            Console.WriteLine("1. Process new invoice");
            Console.WriteLine("2. Repush order to SADAD");
            Console.WriteLine("3. Resend SMS");
            Console.WriteLine("4. Generate PDF in Invoice table");
            Console.WriteLine("5. Download PDF");
            Console.WriteLine("6. Download Invoices in PDF");
            string input = Console.ReadLine();
            if (input.Trim() == "1")
                service.Process().ConfigureAwait(true);
            else if (input.Trim() == "2")
                service.RepushToSADAD().ConfigureAwait(true);
            else if (input.Trim() == "3")
                service.ResendSMS().ConfigureAwait(true);
            else if (input.Trim() == "4")
                service.GeneratePDF().ConfigureAwait(true);

            else if (input.Trim() == "5")
                service.DownloadPDF().ConfigureAwait(true);
            else if (input.Trim() == "6")
                service.DownloadALLPDF().ConfigureAwait(true);
        }
    }
}
