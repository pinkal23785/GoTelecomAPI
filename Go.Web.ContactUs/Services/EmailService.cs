using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Go.Web.ContactUs.Services
{

    public interface IEmailService
    {
        Task<string> SendEmail(string Message);
        Task<string> ReadTemplate(bool IsEng);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> SendEmail(string Message)
        {
            string FromEmail = _configuration.GetValue<string>("FromEmail");
            string FromName = _configuration.GetValue<string>("FromName");
            string Subject = _configuration.GetValue<string>("Subject");
            string Email = _configuration.GetValue<string>("ToEmail");

            string EmailURL = _configuration.GetValue<string>("EmailURL");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.CreateHttp(EmailURL);
            webRequest.ContentType = "text/xml; charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            string soapstr = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>           
            <soapenv:Envelope xmlns:ema=""http://eai.atheeb.net/emailNotification"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
   <soapenv:Header>
      <wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
         <wsse:UsernameToken wsu:Id=""UsernameToken-B1F0707AA4AE86416516344554233821"">
            <wsse:Username>EAIWSUser</wsse:Username>
            <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">wsuser</wsse:Password>
            <wsse:Nonce EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">+cF7xd28y1I84K7PCe2w9w==</wsse:Nonce>
            <wsu:Created>2021-10-17T07:23:43.375Z</wsu:Created>
        </wsse:UsernameToken>
      </wsse:Security>
   </soapenv:Header>
   <soapenv:Body>
      <ema:sendMail>
         <from>
            <mailId>{0}</mailId>
            <name>{1}</name>
         </from>
         <to>
            <mailId>{2}</mailId>
            <name>{3}</name>
         </to>
         <subject>{4}</subject>
         <message>
            <messageText>{5}</messageText>
            <!--Optional:-->
            <messageHTML>{6}</messageHTML>
        </message>
         <!--Zero or more repetitions:-->
      </ema:sendMail>
   </soapenv:Body>
</soapenv:Envelope>", FromEmail, FromName, Email, Email, Subject, Message, Message);

            string result;
            try
            {
                using (Stream s = webRequest.GetRequestStream())
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(soapstr);
                    }
                }

                using (WebResponse w = webRequest.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(w.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                    }
                    //_logger.LogInformation(result);
                    XDocument doc = XDocument.Parse(result);

                    //_logger.LogInformation(result);
                    if (doc.Descendants() != null)
                    {
                        if (doc.Descendants().SingleOrDefault(x => x.Name.LocalName == "status").Value == "0")
                            return "1";
                    }


                }
            }
            catch (WebException ex)
            {
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

            }
            return "0";
        }

        public async Task<string> ReadTemplate(bool IsEng)
        {
            string Message = string.Empty;

            XmlDocument doc = new XmlDocument();
            using (XmlReader reader = XmlReader.Create(@"EmailTemplate.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (IsEng)
                        {
                            if (reader.Name.ToString() == "English")
                                Message = reader.ReadInnerXml().ToString();
                        }
                        else
                        {
                            if (reader.Name.ToString() == "Arabic")
                                Message = reader.ReadInnerXml().ToString();
                        }
                    }
                }
            }

            return Message;
        }
    }
}
