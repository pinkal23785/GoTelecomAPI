using Go.Process.InvoiceURLProducer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


namespace Go.Process.InvoiceURLProducer
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
            services.AddDbContext<BRMContext>(options => options.UseOracle(Configuration.GetConnectionString("BRMDBConnectionString"),
                        opt =>
                        {
                            opt.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                            opt.UseOracleSQLCompatibility("11");
                        }
           ));
            services.AddDbContext<BRMDBContext2>(options => options.UseOracle(Configuration.GetConnectionString("BRMDBConnectionString2"),
                      opt =>
                      {
                          opt.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                          opt.UseOracleSQLCompatibility("11");
                      }

         ));

            
            var mvcBuilder = services.AddControllersWithViews();

            services.AddRazorPages();
            
            services.AddLogging();
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<ICustomerInvoiceService, CustomerInvoiceService>();
 



        }
    }
}
