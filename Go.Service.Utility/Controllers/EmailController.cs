using Go.Service.Utility.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Go.Service.Utility.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private IConfiguration configuration;
        public EmailController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        [HttpPost("send-mail")]
        public async Task<IActionResult> SendMail(EmailRequest request)
        {
            string SMTPServer = configuration.GetValue<string>("SMTPServer");
            string Username = configuration.GetValue<string>("SMTPUsername");
            string Password = configuration.GetValue<string>("Password");
            int Port = Convert.ToInt32(configuration.GetValue<string>("SMTPPort"));

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(Username);
                message.To.Add(new MailAddress(request.ToEmail));
                message.Subject = request.Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = request.htmlBody;
                smtp.Port = Port;
                smtp.Host = SMTPServer; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Username, Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message + ex.InnerException);
            }
        }
    }
}
