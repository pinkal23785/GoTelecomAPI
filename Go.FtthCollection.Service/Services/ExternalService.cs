using Go.FtthCollection.Service.Models;
using Go.LTEWallet.Services.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Go.FtthCollection.Service.Services
{
    public interface IExternalService
    {
        Task<dynamic> InstallmentPaymentOrder(InstallmentPaymentRequest request);
        Task<dynamic> DiscountPaymentOrder(DiscountPaymentOrderReq request);
    }
    public class ExternalService : IExternalService
    {
        private readonly IConfiguration configuration;
        private readonly IDataService dataService;
        private readonly ILogger _logger;
        public ExternalService(IConfiguration _configuration, ILogger<ExternalService> logger, IDataService _dataService)
        {
            configuration = _configuration;
            _logger = logger;
            dataService = _dataService;
        }
        public async Task<dynamic> InstallmentPaymentOrder(InstallmentPaymentRequest request)
        {
            DateTime BillGenTimeStamp = DateTime.UtcNow;
            int days = int.Parse(configuration.GetValue<string>("ExpDays"));
            string URL = configuration.GetValue<string>("InstallmentPaymentURL");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.CreateHttp(URL);
            webRequest.ContentType = "text/xml; charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            //             <soapenv:Header>
            // <wsse:Security soapenv:mustUnderstand = ""1"" xmlns: wsse = ""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">
            //<wsse:UsernameToken wsu:Id = ""UsernameToken - 6207056"" xmlns: wsu = ""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""><wsse:Username>EAIWSUser</wsse:Username>
            //<wsse:Password Type = ""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">wsuser</wsse:Password>
            //<wsse:Nonce> OBN6PpVcjEoxpQGC3FYR7g ==</wsse:Nonce><wsu:Created>2012-07-05T12:49:07.326Z </wsu:Created></wsse:UsernameToken></wsse:Security>
            //                    </soapenv:Header>
            string soapstr = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
              <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ins=""http://installmentPaymentOrder.eai.atheeb.com"">
                 <soapenv:Header>
             <wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">
            <wsse:UsernameToken wsu:Id = ""UsernameToken-6207056"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
              <wsse:Username>EAIWSUser</wsse:Username>
            <wsse:Password Type = ""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">wsuser</wsse:Password>
            <wsse:Nonce> OBN6PpVcjEoxpQGC3FYR7g ==</wsse:Nonce><wsu:Created>2012-07-05T12:49:07.326Z </wsu:Created></wsse:UsernameToken></wsse:Security>
             </soapenv:Header>

                <soapenv:Body>
                  <ins:installmentPaymentOrderReq>
                     <!--Optional:-->
                     <AccountNumber>{0}</AccountNumber>
                     <!--Optional:-->
                     <ContactNumber>{1}</ContactNumber>
                     <!--Optional:-->
                     <InstallmantPeriod>{2}</InstallmantPeriod>
                     <!--Optional:-->
                     <TotalDueAmount>{3}</TotalDueAmount>
                     <!--Optional:-->
                     <BillGenTimeStamp>{4}</BillGenTimeStamp>
                     <!--Optional:-->
                     <ExpDate>{5}</ExpDate>
                     <!--Optional:-->
                     <NotificationLanguage>{6}</NotificationLanguage>
                  </ins:installmentPaymentOrderReq>
               </soapenv:Body>
              </soapenv:Envelope>", request.AccountNumber, request.ContactNumber, request.InstallmantPeriod, request.TotalDueAmount, BillGenTimeStamp.ToString("s") + "Z",
BillGenTimeStamp.AddDays(days).ToString("s") + "Z", request.NotificationLanguage);

            string result;
            _logger.LogInformation(soapstr);
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
                    _logger.LogInformation(result);
                    XDocument doc = XDocument.Parse(result);
                    if (doc.Descendants("Status").FirstOrDefault() != null && doc.Descendants("StatusCode").FirstOrDefault() != null)
                    {
                        if (doc.Descendants("StatusCode").FirstOrDefault().Value == "0" && doc.Descendants("Status").FirstOrDefault().Value == "Success")
                        {
                            await dataService.UpdateAccount(request.AccountNumber, "Installment-Service", request.UserID);
                            return "success";
                        }
                    }
                    throw new Exception("Failed");

                }
            }
            catch (WebException ex)
            {
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                _logger.LogInformation(result);
                throw ex;
            }
            return result;

        }

        public async Task<dynamic> DiscountPaymentOrder(DiscountPaymentOrderReq request)
        {
            DateTime BillGenTimeStamp = DateTime.UtcNow;
            int days = int.Parse(configuration.GetValue<string>("ExpDays"));
            string URL = configuration.GetValue<string>("DiscountPaymentURL");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.CreateHttp(URL);
            webRequest.ContentType = "text/xml; charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            string soapstr = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
              <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:dis=""http://discountPaymentOrder.eai.atheeb.com"">
              <soapenv:Header>
             <wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">
            <wsse:UsernameToken wsu:Id = ""UsernameToken-6207056"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
              <wsse:Username>EAIWSUser</wsse:Username>
            <wsse:Password Type = ""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">wsuser</wsse:Password>
            <wsse:Nonce> OBN6PpVcjEoxpQGC3FYR7g ==</wsse:Nonce><wsu:Created>2012-07-05T12:49:07.326Z </wsu:Created></wsse:UsernameToken></wsse:Security>
             </soapenv:Header>

              <soapenv:Body>
                  <dis:discountPaymentOrderReq>
                     <!--Optional:-->
                     <AccountNumber>{0}</AccountNumber>
                     <!--Optional:-->
                     <ContactNumber>{1}</ContactNumber>
                     <!--Optional:-->
                     <TotalDueAmount>{2}</TotalDueAmount>
                     <!--Optional:-->
                     <DueAmountAfterDiscount>{3}</DueAmountAfterDiscount>
                     <DiscountPercentage>{4}</DiscountPercentage>
                     <BillGenTimeStamp>{5}</BillGenTimeStamp>
                     <!--Optional:-->
                     <ExpDate>{6}</ExpDate>
                     <!--Optional:-->
                     <NotificationLanguage>{7}</NotificationLanguage>
                  </dis:discountPaymentOrderReq>
               </soapenv:Body>
              </soapenv:Envelope>", request.AccountNumber, request.ContactNumber, request.TotalDueAmount,
  Math.Round(request.DueAmountAfterDiscount, 2), request.DiscountPercentage, BillGenTimeStamp.ToString("s") + "Z",
  BillGenTimeStamp.AddDays(days).ToString("s") + "Z", request.NotificationLanguage);

            string result;
            _logger.LogInformation(soapstr);
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
                    _logger.LogInformation(result);
                    XDocument doc = XDocument.Parse(result);

                    
                    if (doc.Descendants("Status").FirstOrDefault() != null && doc.Descendants("StatusCode").FirstOrDefault() != null)
                    {
                        if (doc.Descendants("StatusCode").FirstOrDefault().Value == "0")
                        {
                            await dataService.UpdateAccount(request.AccountNumber, "Discount-Service", request.UserID);
                            return "success";
                        }
                    }
                    throw new Exception("Failed");

                }
            }
            catch (WebException ex)
            {
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                _logger.LogInformation(result);
                throw ex;

            }
            return result;
        }
    }
}
