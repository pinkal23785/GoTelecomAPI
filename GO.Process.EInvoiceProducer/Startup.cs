using GO.Process.EInvoiceProducer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GO.Process.EInvoiceProducer
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CADBContext>(options => options.UseOracle(Configuration.GetConnectionString("CADBConnectionString"),
                        opt =>
                        {
                            opt.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                            opt.UseOracleSQLCompatibility("11");
                        }
            ));
            services.AddLogging();
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<IProcessInvoice, ProcessInvoice>();
        }
    }
}
