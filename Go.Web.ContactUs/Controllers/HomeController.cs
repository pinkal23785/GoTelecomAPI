using Go.Web.ContactUs.Models;
using Go.Web.ContactUs.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Web.ContactUs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService _emailService;
        public HomeController(ILogger<HomeController> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            ContactModel model = new ContactModel();
            return View(model);
        }

        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            int width = 150;
            int height = 50;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }
        public async Task<IActionResult> SendMail(ContactModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!Captcha.ValidateCaptchaCode(model.CaptchaCode, HttpContext))
            {
                // return error
            }
            string Message = await _emailService.ReadTemplate(true);
            string TopicName = model.TopicList.Where(x => x.Value == model.Topic).Select(x=>x.Text).FirstOrDefault();
            Message = string.Format(Message, model.Name, TopicName, model.Phone, model.Email, model.Comments);
            var result = await _emailService.SendEmail(Message);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
