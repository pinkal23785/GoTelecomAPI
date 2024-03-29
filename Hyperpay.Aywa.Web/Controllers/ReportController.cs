using Hyperpay.Aywa.Web.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Controllers
{
    public class ReportController : Controller
    {

        private IConfiguration _configuration;
        private IDataService _dataService;
        private IHyperService _hyperService;
        public ReportController(IConfiguration configuration, IDataService dataService, IHyperService hyperService)
        {
            _configuration = configuration;
            _dataService = dataService;
            _hyperService = hyperService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(string MobileNumber,string TransDate)
        {
            var result = await _dataService.SearchTransactionByMobileNumber(MobileNumber, TransDate);
            ViewBag.MobileNumber = MobileNumber;
            ViewBag.TransDate = TransDate;
            return View(result);
            //return View();
        }

        public async Task<IActionResult> ResendSMS([FromQuery]int TransID)
        {
            var result =await _hyperService.ResendNotification(TransID);
            return Ok(result);
        }

        public async Task<IActionResult> SendBulkSMS()
        {
            var result = await _hyperService.BulkSendSMS();
            return Ok(result);
        }
        public IActionResult Login()
        {
            HttpContext.SignOutAsync();
            return View();
        }

        [HttpPost]
        public IActionResult Login(string UserName, string Password)
        {
            if (!string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Password))
            {
                return RedirectToAction("Login");
            }

            //Check the user name and password  
            //Here can be implemented checking logic from the database  
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            if (UserName == _configuration.GetValue<string>("SUsername") && Password == _configuration.GetValue<string>("SPassword"))
            {

                //Create the identity for the user  
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim(ClaimTypes.Role, "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAuthenticated = true;
            }

            //if (UserName == "Mukesh" && Password == "password")
            //{
            //    //Create the identity for the user  
            //    identity = new ClaimsIdentity(new[] {
            //        new Claim(ClaimTypes.Name, UserName),
            //        new Claim(ClaimTypes.Role, "User")
            //    }, CookieAuthenticationDefaults.AuthenticationScheme);

            //    isAuthenticated = true;
            //}

            if (isAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                
                return RedirectToAction("Index", "Report");
            }
            return View();
        }
    }
}
