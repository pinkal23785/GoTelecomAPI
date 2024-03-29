using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GO.TestSMTP
{
    class Program
    {
        static void Main(string[] args)
        {
            string SMTP = "smtp.office365.com";
            string user = "contactus@go.com.sa";
            string pass = "8aZiguDlNIfO+5aQASp8";


            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("contactus@go.com.sa");
            message.To.Add(new MailAddress("p.patel@c.go.com.sa"));
            message.Subject = "Test";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = "<h1>Hi this is test mail</h1>";
            smtp.Port = 587;
            smtp.Host = "smtp.office365.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("contactus@go.com.sa", "8aZiguDlNIfO+5aQASp8");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
    }
}
