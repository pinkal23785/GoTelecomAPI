using Go.LTEWallet.Services.Data.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Go.LTEWallet.Services.Data
{
    public interface IOTPService
    {
        Task<string> SendOTP(string MobileNumber, string Lang);
        
    }
    public class OTPService : IOTPService
    {
        private readonly IConfiguration _configuration;
        private readonly IDataService _dataservice;
        string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        public OTPService(IConfiguration configuration, IDataService dataservice)
        {
            _configuration = configuration;
            _dataservice = dataservice;
        }
        public async Task<string> SendOTP(string MobileNumber, string Lang = "en")
        {
            int expireMin = _configuration.GetValue<int>("OPTExpireTime");
            var otp = new CustomerOTP();
            otp.MOBILENUMBER = MobileNumber;
            otp.OTP = GenerateRandomOTP(5);
            otp.DATE_CREATED = DateTime.Now;
            otp.OTP_EXPIRE_DATE = DateTime.Now.AddMinutes(expireMin);
            otp.IS_VERIFIED = "0";

            bool IsEng = Lang == "en" ? true : false;
            string SMSMessage = ReadTemplate(IsEng);
            SMSMessage = string.Format(SMSMessage, otp.OTP);
            string IsSMSSent = "0";
             IsSMSSent = await SendSMS(MobileNumber, SMSMessage);

            string Subject = _configuration.GetValue<string>("Subject");
            
            if (IsSMSSent == "1")
            {
                await _dataservice.AddOTP(otp);
                return "success";
            }
            

            return "failed";

        }
        private string GenerateRandomOTP(int iOTPLength)
        {

            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }
        private string ReadTemplate(bool IsEng)
        {
            string Message = string.Empty;

            XmlDocument doc = new XmlDocument();
            using (XmlReader reader = XmlReader.Create(@"SMSTemplate.xml"))
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
            //_logger.LogInformation(soapstr);
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

                    // _logger.LogInformation(result);
                    if (doc.Descendants("Status").FirstOrDefault() != null)
                    {
                        if (doc.Descendants("Status").FirstOrDefault().Value == "0")
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
    }
}
