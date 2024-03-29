using Hyperpay.Aywa.Web.Data;
using Hyperpay.Aywa.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageService _messageService;
        public HomeController(ILogger<HomeController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SendMail()
        {
            _messageService.SendUnifonicSMS("0564465193", "Test SMS");
            return View();
        }
    }
}
