using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Hyperpay.Aywa.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.ServiceModel;
using SoapCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Hyperpay.Aywa.Web
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
            //services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddDbContext<CADBContext>(options => options.UseOracle(Configuration.GetConnectionString("CADBConnectionString"),
               opt =>
               {
                   opt.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                   opt.UseOracleSQLCompatibility("11");
               }


           ));
            services.AddSoapCore();
            
            //services.AddMvc().AddRazorPagesOptions(options=>{
            //    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            //});
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();
            services.AddTransient<ILoggerService, LoggerService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IDataService, DataService>();
            services.AddTransient<IOTPService, OTPService>();
            services.AddTransient<IHyperService, HyperService>();
            services.AddTransient<ISADADService, SADADService>();
            services.AddTransient<IAywaCardPaymentService, AywaCardPaymentService>();

            services.AddCors();
            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.  
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                
            });
            //services.ConfigureApplicationCookie(options =>
            //{
            //    // Cookie settings
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            //    options.LoginPath = "/Report/Login";
            //    options.AccessDeniedPath = "/Report/AccessDenied";
            //    options.SlidingExpiration = true;
            //});
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options=>
            {
                options.LoginPath = "/Report/Login";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseEndpoints(endpoints => {
            //    endpoints.UseSoapEndpoint<IAywaCardPaymentService>("/Service.svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);
            //    endpoints.UseSoapEndpoint<IAywaCardPaymentService>("/Service.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
            //});
            loggerFactory.AddFile("Logs/HypePay-{Date}.txt");
            app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowCredentials());
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            //app.UseAuthentication();
            app.UseAuthorization();
            app.UseSoapEndpoint<IAywaCardPaymentService>("/Service.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Aywa}/{action=Home}/{id?}");

                
            });
            
            
        }
    }
}
