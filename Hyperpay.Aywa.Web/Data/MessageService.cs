using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Hyperpay.Aywa.Web.Data
{
    public interface IMessageService
    {
        Task<string> SendSMS(string MobileNumber, string Message);
        Task<string> SendEmail(string Email, string Message, string Subject);
        Task<int> SendSMTPEmail(string Email, string Message, string Subject);
        Task<string> ShortURL(string strURL);
        Task<int> SendUnifonicSMS(string MobileNumber, string Message);
    }
    public class MessageService : IMessageService
    {
        private readonly IConfiguration _configuration;
        private ILoggerService _loggerService;
        public MessageService(IConfiguration configuration, ILoggerService loggerService)
        {
            _configuration = configuration;
            _loggerService = loggerService;
        }
        public async Task<string> SendSMS(string MobileNumber, string Message)
        {
            string ConversationID = _configuration.GetValue<string>("ConversationID");
            string SenderMobile = _configuration.GetValue<string>("SenderMobile");

            string SMSURL = _configuration.GetValue<string>("SMSURL");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.CreateHttp(SMSURL);
            webRequest.ContentType = "text/xml; charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            string soapstr = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>           
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://172.16.68.23:12000/repository/JCAPS_ROLLUP4/xsd/prjYamamahSMSGW/waleed/JCAPS_ROLLUP410ccf31:12a0d6e7667:-7ffc/XSDDefinition1"">
            <soapenv:Header>
                <wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">
                  <wsse:UsernameToken wsu:Id=""UsernameToken-6207056"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
                    <wsse:Username>EAIWSUser</wsse:Username>
                    <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">wsuser</wsse:Password>
                    <wsse:Nonce>OBN6PpVcjEoxpQGC3FYR7g==</wsse:Nonce>
                    <wsu:Created>{0}</wsu:Created>
                  </wsse:UsernameToken>
                </wsse:Security>
            </soapenv:Header>
            <soapenv:Body>
                <xsd:SMSRequest>
                  <Header>
                    <ServiceID>SendSMS</ServiceID>
                    <VersionNo>1.0</VersionNo>
                    <CallerMessageID>?</CallerMessageID>
                    <!--Optional:-->
                    <ConversationID>{1}</ConversationID>
                    <RequestTimeStamp>{2}</RequestTimeStamp>
                    <CallerSystemID>?</CallerSystemID>
                  </Header>
                  <!--Optional:-->
                  <Sender>{3}</Sender>
                  <Message>{4}</Message>
                  <!--1 or more repetitions:-->
                  <Receiver>{5}</Receiver>
                  <!--English or Arabic:-->
                  <Language></Language>
                </xsd:SMSRequest>
            </soapenv:Body>
        </soapenv:Envelope>", DateTime.Now.ToString("s") + "Z", ConversationID, DateTime.Now.ToString("s") + "Z", SenderMobile, Message, MobileNumber);
            await _loggerService.AddLog(soapstr);
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
                    // _logger.LogInformation(result);
                    XDocument doc = XDocument.Parse(result);

                    //  _logger.LogInformation(result);
                    if (doc.Descendants("Status").FirstOrDefault() != null)
                    {
                        if (doc.Descendants("Status").FirstOrDefault().Value == "0")
                        {
                            await _loggerService.AddLog(result);
                            return "1";
                        }
                    }


                }
            }
            catch (WebException ex)
            {
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

            }
            await _loggerService.AddLog("SMS Error: " + result);
            return "0";
        }

        public async Task<string> SendEmail(string Email, string Message, string Subject)
        {
            string FromEmail = _configuration.GetValue<string>("FromEmail");
            string FromName = _configuration.GetValue<string>("FromName");
            // string Subject = _configuration.GetValue<string>("Subject");


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

            await _loggerService.AddLog(soapstr);
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
                        {
                            await _loggerService.AddLog(result);
                            return "1";
                        }
                    }


                }
            }
            catch (WebException ex)
            {
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

            }
            await _loggerService.AddLog(result);
            return "0";
        }

        public async Task<string> ShortURL(string strURL)
        {
            string URL;
            URL = _configuration.GetValue<string>("TinyURL") + strURL.ToLower();

            System.Net.HttpWebRequest objWebRequest;
            System.Net.HttpWebResponse objWebResponse;

            System.IO.StreamReader srReader;

            string strHTML;

            objWebRequest = (System.Net.HttpWebRequest)System.Net
               .WebRequest.Create(URL);
            objWebRequest.Method = "GET";

            objWebResponse = (System.Net.HttpWebResponse)objWebRequest
               .GetResponse();
            srReader = new System.IO.StreamReader(objWebResponse
               .GetResponseStream());

            strHTML = srReader.ReadToEnd();

            srReader.Close();
            objWebResponse.Close();
            objWebRequest.Abort();

            return (strHTML);
        }


        public async Task<int> SendUnifonicSMS(string MobileNumber, string Message)
        {
            int result = 0;

            try
            {
                SMSRequestModel model = new SMSRequestModel(_configuration);
                model.Recipient = MobileNumber;
                model.Body = Message;
                var client = new RestClient(_configuration.GetValue<string>("UnifonicURL"));
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
                var response = await client.ExecuteAsync<SMSResponseModel>(request);
                await _loggerService.AddLog($"SMS Response {MobileNumber}:  " + response.Content);
                if (response.Data.success)
                {
                    await _loggerService.AddLog(MobileNumber + ": SMS SENT");
                    result = 1;
                }

            }
            catch (Exception ex)
            {
                await _loggerService.AddLog("SMS Error:" + ex.Message + " " + ex.InnerException);
                result = 0;
            }
            return await Task.FromResult<int>(result);
        }

        public async Task<int> SendSMTPEmail(string Email, string Message, string Subject)
        {
            int result = 0;
            try
            {
                //  MailMessage 
                String userName = _configuration.GetValue<string>("SMTPUserName");
                String password = _configuration.GetValue<string>("SMTPPassword");
                MailMessage msg = new MailMessage(userName, Email);
                msg.Subject = Subject;

                msg.Body = HttpUtility.HtmlDecode(Message);
                msg.IsBodyHtml = true;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                SmtpClient SmtpClient = new SmtpClient();
                SmtpClient.Credentials = new System.Net.NetworkCredential(userName, password);
                SmtpClient.Host = "smtp.office365.com";
                SmtpClient.Port = 587;
                SmtpClient.EnableSsl = true;
                SmtpClient.Send(msg);
                result = 1;
            }
            catch (Exception ex)
            {
                await _loggerService.AddLog("Error:" + ex.Message + " " + ex.InnerException);
                result = 0;
            }
            return await Task.FromResult<int>(result);
        }
    }
}
