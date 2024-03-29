using Go.Service.NumberBooking.Data;
using Go.Service.NumberBooking.WebServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SoapCore;
using SoapCore.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Service.NumberBooking
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IFaultExceptionTransformer, DefaultFaultExceptionTransformer>();
            services.AddDbContext<NBSContext>(options =>
                 options.UseSqlServer(
                     Configuration.GetConnectionString("NBSConnection"),
                     b => b.MigrationsAssembly(typeof(NBSContext).Assembly.FullName)));
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddCors();
            services.AddScoped<IDataService, DataService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddCors();
            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddFile("Logs/NBSLog-{Date}.txt");
            app.UseSoapEndpoint<ICustomerService>("/CustomerService.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
            app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowCredentials());
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/NBSService/swagger/v1/swagger.json", "LTE Commission API V1");
               // c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
