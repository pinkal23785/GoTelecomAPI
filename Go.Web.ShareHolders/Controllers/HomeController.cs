using Go.Web.ShareHolders.Data.Entities;
using Go.Web.ShareHolders.Models;
using GO.Web.ShareHolders.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Web.ShareHolders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IOTPService _oTPService;
        private readonly IDataService _dataService;
        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer, IOTPService oTPService, IDataService dataService)
        {
            _logger = logger;
            _localizer = localizer;
            _oTPService = oTPService;
            _dataService = dataService;
        }
        private void DeleteCookies()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
        }
        public IActionResult Index([FromQuery] string culture)
        {
            var model = new ShareHolderDetails();
            if (culture == null)
            {
                culture = "en-US";
            }
            ModelState.Clear();
            model.Culture = culture;

            DeleteCookies();

            Response.Cookies.Append(
                "culture",
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) });

            return View(model);
        }

        public async Task<IActionResult> SendOTP(ShareHolderDetails model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            if (await _dataService.IsIDNumberExist(model.ID_Number.Value))
            {
                ViewBag.Message =_localizer["ID Number already exists"];
                return View("Index", model);
            }
            else
            {
                await _oTPService.SendCustomerOTP(model.MobileNumber, model.Culture);

                DeleteCookies();

                Response.Cookies.Append(
                    "culture",
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(model.Culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) });
                return View(model);
            }
        }
        public async Task<bool> ResendOTP([FromBody] ResendOTPModel model)
        {
            await _oTPService.SendCustomerOTP(model.Mobile, model.Culture);
            return true;

        }
        public async Task<IActionResult> SaveShareHolderDetails(ShareHolderDetails model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("SendOTP", model);
                }
                if (!string.IsNullOrEmpty(model.OTP))
                {
                    await _oTPService.VerifyOTP(model);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
