using Hyperpay.Aywa.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Hyperpay.Aywa.Web.Data
{
    public interface IOTPService
    {
        public Task<string> SendOTP(string Mobile, string Email, string Lang);
        public Task<bool> ValidateOTP(string Mobile, string OTP);
    }
    public class OTPService : IOTPService
    {
        string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        private readonly IDataService _dataservice;
        private readonly IConfiguration _configuration;
        private readonly IMessageService _messageService;
        private readonly ILoggerService _loggerService;
        private readonly CADBContext _context;
        public OTPService(IDataService dataservice, IConfiguration configuration, CADBContext context,
            IMessageService messageService, ILoggerService loggerService)
        {
            _dataservice = dataservice;
            _configuration = configuration;
            _messageService = messageService;
            _loggerService = loggerService;
            _context = context;
        }
        public async Task<string> SendOTP(string Mobile, string Email, string Lang)
        {
            int expireMin = _configuration.GetValue<int>("OPTExpireTime");
            var otp = new CustomerOTP();
            otp.MOBILENUMBER = Mobile;
            otp.EMAIL = Email;
            otp.OTP = GenerateRandomOTP(5);
            otp.DATE_CREATED = DateTime.Now;
            otp.OTP_EXPIRE_DATE = DateTime.Now.AddMinutes(expireMin);
            otp.ISVERIFIED = "0";
            //Send SMS
            bool IsEng = Lang == "en" ? true : false;
            string SMSMessage = ReadTemplate(true, IsEng);
            string EmailMessage = ReadTemplate(false, IsEng);
            SMSMessage = string.Format(SMSMessage, otp.OTP, DateTime.Now.ToString("dd/MM/yyyy hh:mm:sss"));
            EmailMessage = string.Format(EmailMessage, otp.OTP);

            int IsSMSSent = await _messageService.SendUnifonicSMS(Mobile, SMSMessage);
            string Subject = _configuration.GetValue<string>("Subject");
            int IsMailSent = await _messageService.SendSMTPEmail(Email, (EmailMessage), Subject);

            //Send Email
            if (IsSMSSent == 1 || IsMailSent == 1)
            {
                await _dataservice.AddOTP(otp);
                return otp.OTP;
            }
            else
            {
                await _loggerService.AddLog("Failed to send SMS or Mail");
                throw new ApplicationException("Failed to send SMS or Mail");
            }
        }
        public async Task<bool> ValidateOTP(string Mobile, string OTP)
        {
            var result = await _context.CustomerOTPS.Where(x => x.MOBILENUMBER == Mobile && x.OTP == OTP).OrderByDescending(x => x.OTP_EXPIRE_DATE).FirstOrDefaultAsync();
            if (result != null && result.OTP_EXPIRE_DATE >= DateTime.Now)
            {
                result.ISVERIFIED = "1";
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
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

        private string ReadTemplate(bool IsSMS, bool IsEng)
        {
            string Message = string.Empty;
            if (IsSMS)
            {
                XmlDocument doc = new XmlDocument();
                using (XmlReader reader = XmlReader.Create(@"MessageTemplate.xml"))
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
            }
            else
            {
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
            }
            return Message;
        }


    }
}
