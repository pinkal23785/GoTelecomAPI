using Hyperpay.Aywa.Web.Models;
using Hyperpay.Aywa.Web.SADAD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Hyperpay.Aywa.Web.Data
{
    public interface ISADADService
    {
        Task<SADADServiceResponse> GenerateSADADServiceID(SADADServiceRequestModel parameter);
        Task<SADADUploadResponse> UploadBillToSADAD(CheckoutModel parameter);

        Task ApplySADADPayment(AywaCardPaymentReqModel parameter);
    }
    public class SADADService : ISADADService
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerService _loggerService;
        private readonly IDataService _dataService;
        private readonly IMessageService _messageService;
        private CADBContext _context;
        private string InvoiceURL = "";
        public SADADService(IConfiguration configuration, ILoggerService loggerService, IDataService dataService,
            IMessageService messageService, CADBContext context)
        {
            _configuration = configuration;
            _loggerService = loggerService;
            _dataService = dataService;
            _messageService = messageService;
            _context = context;
            InvoiceURL = _configuration.GetValue<string>("InvoiceURL");
        }
        public async Task<SADADServiceResponse> GenerateSADADServiceID(SADADServiceRequestModel parameter)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            SADADServiceResponse responseResult = null;
            string URL = _configuration.GetValue<string>("SADADServiceURL");
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("content-type", "application/json");
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            request.AddParameter("application/json", JsonConvert.SerializeObject(parameter), ParameterType.RequestBody);

            IRestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                responseResult = JsonConvert.DeserializeObject<SADADServiceResponse>(response.Content.ToString());
            }
            return responseResult;
        }
        public async Task<SADADUploadResponse> UploadBillToSADAD(CheckoutModel parameter)
        {
            SADADUploadResponse ResponseResult = null;

            var SadadBillReq = new SADADServiceRequestModel();
            var SadadBillRes = await GenerateSADADServiceID(SadadBillReq);
            if (SadadBillRes != null)
            {
                //Upload bill to sadad
                var SadadUploadURL = _configuration.GetValue<string>("SADADUploadURL");

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.CreateHttp(SadadUploadURL);
                webRequest.ContentType = "text/xml; charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                DateTime cuurentTime = DateTime.Now;

                string billCycle = "INITIAL_" + cuurentTime.ToString("yy-MM-dd");
                string billGenTimeStamp = cuurentTime.ToString("yyyy-MM-dd'T'hh:mm:ss.fffZ");

                TimeZoneInfo AST = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                var DueDateTime = TimeZoneInfo.ConvertTime(cuurentTime.AddDays(2), AST);
                var ExpireDateTime = TimeZoneInfo.ConvertTime(cuurentTime.AddDays(3), AST);

                string DueDate = DueDateTime.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
                string ExpDate = ExpireDateTime.ToString("yyyy-MM-dd'T'HH:mm:sszzz");

                bool IsTrialAmt = _configuration.GetValue<bool>("UseTrialAmount");
                if (IsTrialAmt)
                    parameter.Amount = 1;

                string soapstr = string.Format(@"<soapenv:Envelope 
                                xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
                                xmlns:int=""http://internalbillupload.eai.atheeb.com"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <int:BillRec>
                                     <BillStatusCode>{0}</BillStatusCode>
                                     <BillInfo>
                                        <BillCategory>{1}</BillCategory>
                                        <BillCycle>{2}</BillCycle>
                                        <!--Optional:-->
                                        <BillNumber>{3}</BillNumber>
                                        <BillingAcct>{4}</BillingAcct>
                                        <AmountDue>{5}</AmountDue>
                                        <BillGenTimeStamp>{6}</BillGenTimeStamp>
                                        <DueDt>{7}</DueDt>
                                        <!--Optional:-->
                                        <ExpDt>{8}</ExpDt>
                                     </BillInfo>
                                  </int:BillRec>
                               </soapenv:Body>
                            </soapenv:Envelope>", "BillNew", "Recurring", billCycle, SadadBillRes.accountId,
                            SadadBillRes.subscriberId, parameter.Amount, billGenTimeStamp, DueDate, ExpDate);

                string result;
                await _loggerService.AddLog(soapstr);
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
                        await _loggerService.AddLog(result);
                        XDocument doc = XDocument.Parse(result);

                        if (doc.Descendants("Status").FirstOrDefault() != null && doc.Descendants("StatusCode").FirstOrDefault() != null)
                        {
                            if (doc.Descendants("StatusCode").FirstOrDefault().Value == "0")
                            {

                                ResponseResult = new SADADUploadResponse()
                                {
                                    Amount = parameter.Amount,
                                    BillNumber = SadadBillRes.subscriberId
                                };
                                await _dataService.ReserveAywaCardTrans(SadadBillRes, parameter);
                                bool Lang = parameter.Lang == "en" ? true : false;
                                string SMSMessage = ReadTemplate(true, Lang);
                                string EmailMessage = ReadTemplate(false, Lang);

                                SMSMessage = string.Format(SMSMessage, parameter.Amount, SadadBillRes.subscriberId);
                                EmailMessage = string.Format(EmailMessage, parameter.Amount, SadadBillRes.subscriberId);

                                await _messageService.SendUnifonicSMS(parameter.MobileNumber, SMSMessage);
                                string Subject = _configuration.GetValue<string>("SADADUploadSubject");
                                await _messageService.SendSMTPEmail(parameter.Email, EmailMessage, Subject);
                                return ResponseResult;
                            }
                        }
                        throw new Exception("Failed");
                    }
                }
                catch (WebException ex)
                {
                    result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    await _loggerService.AddLog(result);
                    throw ex;

                }

            }
            return ResponseResult;
        }

        public async Task ApplySADADPayment(AywaCardPaymentReqModel parameter)
        {
            DateTime OracleDate;
            DbConnection connection = _context.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }

            var command = connection.CreateCommand();
            command.CommandText = "select sysdate from dual";
            OracleDate = (DateTime)await command.ExecuteScalarAsync();

            var objCardTrans = _context.PrisonsAywaCardTrans.
                Where(x => x.PUR_ORD_ACCOUNT_ID == parameter.subscriberId &&
                x.PUR_ORD_SUBSCRIBER_ID == parameter.accountId && x.PUR_ORD_RESP_STATUS == "Pending").FirstOrDefault();

            if (objCardTrans != null)
            {

                var objCardStock = await _context.PrisonsAywaCardStocks.Where(x => x.STATUS == "Active" &&
                 x.CARD_TYPE_ID == objCardTrans.CARD_TYPE_ID && x.EXPIRY > OracleDate).OrderBy(x => x.EXPIRY).ThenBy(x => x.ID).FirstOrDefaultAsync();

                await _loggerService.AddLog("Selected Card:" + objCardStock.CARDNUMBER + ", Oracle Current Date:" + OracleDate.ToString());

                objCardStock.STATUS = "Consumed";

                objCardStock.LAST_MODIFY_BY = "Public User";
                objCardStock.LAST_MODIFY_DATE = DateTime.Now;
                objCardTrans.PUR_ORD_RESP_STATUS = "Completed";
                objCardTrans.CARD_STOCK_ID = objCardStock.ID;
                objCardTrans.PAYMENT_METHOD = "SADAD";
                objCardTrans.PAYMENT_REF = parameter.paymentRef;
                objCardTrans.PUR_ORD_ORDER_ID = parameter.orderID;
                objCardTrans.TRANS_TYPE = "Consumption";
                objCardTrans.QUANTITY = 1;
                objCardTrans.CARD_TYPE_ID = objCardStock.CARD_TYPE_ID;
                await _context.SaveChangesAsync();

                // Send email and SMS

                bool IsEng = objCardTrans.LANG_PREF == "Arabic" ? false : true;
                string SMSMessage = ReadSADADPaymentTemplate(true, IsEng);
                string EmailMessage = ReadSADADPaymentTemplate(false, IsEng);

                string shorturl = InvoiceURL + "?transactionID=" + objCardTrans.ID;
                string tinyURL = await _messageService.ShortURL(shorturl);


                SMSMessage = string.Format(SMSMessage, tinyURL, objCardStock.CARDNUMBER);
                EmailMessage = string.Format(EmailMessage, tinyURL, objCardStock.CARDNUMBER);

                objCardTrans.IS_SMS_SENT = await _messageService.SendUnifonicSMS(objCardTrans.MOBILE, SMSMessage);
                string Subject = _configuration.GetValue<string>("SADADPaymentSubject");
                objCardTrans.IS_MAIL_SENT = await _messageService.SendSMTPEmail(objCardTrans.EMAIL, EmailMessage, Subject);
            }
        }
        private string ReadTemplate(bool IsSMS, bool IsEng)
        {
            string Message = string.Empty;
            if (IsSMS)
            {
                XmlDocument doc = new XmlDocument();
                using (XmlReader reader = XmlReader.Create(@"SADADSms.xml"))
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
                using (XmlReader reader = XmlReader.Create(@"SADADEmail.xml"))
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


        private string ReadSADADPaymentTemplate(bool IsSMS, bool IsEng)
        {
            string Message = string.Empty;
            if (IsSMS)
            {
                XmlDocument doc = new XmlDocument();
                using (XmlReader reader = XmlReader.Create(@"InvoiceSMS.xml"))
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
                using (XmlReader reader = XmlReader.Create(@"InvoiceEmail.xml"))
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
