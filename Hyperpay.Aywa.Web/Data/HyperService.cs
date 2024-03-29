using Hyperpay.Aywa.Web.Data.Entities;
using Hyperpay.Aywa.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Hyperpay.Aywa.Web.Data
{
    public interface IHyperService
    {
        Task<string> CheckoutPayment(CheckoutModel parameter);
        Task<PaymentSucessModel> GetPaymentStatus(string CheckoutId, int AwayaCardId, string Lang);
        Task<bool> ResendNotification(int TransID);

        Task<bool> BulkSendSMS();
    }
    public class HyperService : IHyperService
    {
        private IConfiguration _configuration;
        private CADBContext _context;
        private IDataService _dataService;
        private ILoggerService _loggerService;
        private IMessageService _messageService;
        private string InvoiceURL = "";
        private readonly object transLock = new object();
        public HyperService(IConfiguration configuration, CADBContext context, IDataService dataService,
            ILoggerService loggerService, IMessageService messageService)
        {
            _configuration = configuration;
            _context = context;
            _dataService = dataService;
            _loggerService = loggerService;
            _messageService = messageService;
            InvoiceURL = _configuration.GetValue<string>("InvoiceURL");
        }
        public async Task<string> CheckoutPayment(CheckoutModel parameter)
        {
            try
            {
                string responseData;
                await _loggerService.AddLog(JsonConvert.SerializeObject(parameter));

                StringBuilder data = new StringBuilder();
                string MerchantTransactionId = Guid.NewGuid().ToString().Replace("-", "");
                //parameter.amount = decimal.Parse("1.50");
                //data.Append("amount=" + parameter.amount);
                bool IsTrialAmt = _configuration.GetValue<bool>("UseTrialAmount");
                if (IsTrialAmt)
                    parameter.Amount = 1;


                data.Append("amount=" + parameter.Amount.ToString("F"));
                data.Append("&currency=" + "SAR");

                if (_configuration.GetValue<bool>("Sandbox"))
                {
                    data.Append("&merchantTransactionId=" + _configuration.GetValue<string>("MerchantTransId"));
                }
                else
                {
                    data.Append("&merchantTransactionId=" + MerchantTransactionId);
                }
                data.Append("&customer.email=" + parameter.Email);
                data.Append("&billing.street1=" + parameter.StreetAddress);
                data.Append("&billing.city=" + parameter.City);
                data.Append("&billing.state=" + parameter.State);
                data.Append("&billing.country=" + "SA");
                data.Append("&billing.postcode=" + parameter.PinCode);
                data.Append("&customer.givenName=" + parameter.FirstName);
                data.Append("&customer.surname=" + parameter.LastName);
                data.Append("&paymentType=" + "DB");
                string entityId = "";
                if (parameter.CardType.ToLower() == "visa" || parameter.CardType.ToLower() == "master")
                {
                    if (_configuration.GetValue<bool>("Sandbox"))
                    {
                        data.Append("&testMode=EXTERNAL");
                    }
                    data.Append("&entityId=" + _configuration.GetValue<string>("VisaMasterEntityId"));
                    entityId = _configuration.GetValue<string>("VisaMasterEntityId");

                }
                else if (parameter.CardType.ToLower() == "mada")
                {
                    //data.Append("&paymentType=" + parameter.paymentType);
                    data.Append("&entityId=" + _configuration.GetValue<string>("MadaEntityId"));
                    entityId = _configuration.GetValue<string>("MadaEntityId");
                }

                else if (parameter.CardType.ToLower() == "applepay")
                {
                    if (_configuration.GetValue<bool>("Sandbox"))
                    {
                        data.Append("&testMode=EXTERNAL");
                    }
                    data.Append("&entityId=" + _configuration.GetValue<string>("AppleEntityId"));
                    entityId = _configuration.GetValue<string>("AppleEntityId");
                    //data.Append("&merchantTransactionId=" + MerchantTransactionId);
                }

                string url = _configuration.GetValue<string>("HyperPayURL") + "/checkouts";
                await _loggerService.AddLog(data.ToString());
                byte[] buffer = Encoding.ASCII.GetBytes(data.ToString());
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "POST";
                request.Headers["Authorization"] = _configuration.GetValue<string>("HyperPayAuthToken");
                request.ContentType = "application/x-www-form-urlencoded";
                Stream PostData = request.GetRequestStream();
                PostData.Write(buffer, 0, buffer.Length);
                PostData.Close();
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseData = (reader.ReadToEnd());// s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());
                    reader.Close();
                    dataStream.Close();
                }
                var checkoutSuccess = JsonConvert.DeserializeObject<CheckoutSuccessResult>(responseData);
                if (checkoutSuccess != null && checkoutSuccess.result.code == "000.200.100")
                    await _dataService.InsertPaymentTransactionStatus(checkoutSuccess, "",
                        parameter, MerchantTransactionId, entityId);
                await _loggerService.AddLog(responseData.ToString());

                return checkoutSuccess.id;


            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                throw ex;
            }
        }

        public async Task<PaymentSucessModel> GetPaymentStatus(string CheckoutId, int AwayaCardId, string Lang)
        {
            try
            {
                string responseData;
                await _loggerService.AddLog("CheckoutID:" + CheckoutId);
                var payment = await _dataService.GetPaymentCheckout(CheckoutId);

                if (payment != null)
                {
                    string cardType = payment.CARDTYPE;
                    StringBuilder data = new StringBuilder();
                    if (cardType.ToLower() == "visa" || cardType.ToLower() == "master")
                    {
                        data.Append("entityId=" + _configuration.GetValue<string>("VisaMasterEntityId"));
                    }
                    else if (cardType.ToLower() == "mada")
                    {
                        data.Append("entityId=" + _configuration.GetValue<string>("MadaEntityId"));
                    }
                    else if (cardType.ToLower() == "applepay")
                    {
                        data.Append("entityId=" + _configuration.GetValue<string>("AppleEntityId"));
                    }

                    string url = string.Format(_configuration.GetValue<string>("HyperPayURL") + "/checkouts/{0}/payment?", CheckoutId) + data;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    request.Method = "GET";
                    request.Headers["Authorization"] = _configuration.GetValue<string>("HyperPayAuthToken");
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        Stream dataStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);
                        // var s = new JavaScriptSerializer();
                        responseData = reader.ReadToEnd();// s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());
                        reader.Close();
                        dataStream.Close();
                    }
                    await _loggerService.AddLog(responseData.ToString());
                    PaymentStatusResult result = JsonConvert.DeserializeObject<PaymentStatusResult>(responseData);
                    //if (result.result.code == "000.100.112" || result.result.code == "000.100.110" || result.result.code== "000.000.000")
                    {
                        //Insert Status in DB
                        await _dataService.UpdatePaymentStatus(result);
                    }
                    var paymentResult = new PaymentSucessModel();
                    paymentResult.ResultCode = result.result.code;
                    paymentResult.StatusDesc = result.result.description;

                    if (result.result.code == "000.100.112" || result.result.code == "000.100.110" || result.result.code == "000.000.000")
                    {
                        PrisonsAywaCardTran cardTrans = null;
                        PrisonsAywaCardStock cardStock = null;
                        // Process the Card

                        cardTrans = _dataService.ProcessAywaCard(result.ndc, AwayaCardId).Result;
                        cardStock = _dataService.GetPrisonsAywaCardStock(cardTrans.CARD_STOCK_ID.Value).Result;


                        if (cardTrans != null)
                        {
                            bool IsEng = Lang == "en" ? true : false;
                            string SMSMessage = ReadTemplate(true, IsEng);
                            string EmailMessage = ReadTemplate(false, IsEng);

                            string shorturl = InvoiceURL + "?transactionID=" + cardTrans.ID;
                            string tinyURL = await _messageService.ShortURL(shorturl);
                            paymentResult.shortURL = tinyURL;


                            paymentResult.InvoiceId = await _dataService.GetInvoice(cardTrans.ID);
                            paymentResult.CardNumber = cardStock.CARDNUMBER;

                            SMSMessage = string.Format(SMSMessage, tinyURL, cardStock.CARDNUMBER);
                            EmailMessage = string.Format(EmailMessage, tinyURL, cardStock.CARDNUMBER);

                            cardTrans.IS_SMS_SENT = await _messageService.SendUnifonicSMS(cardTrans.MOBILE, SMSMessage);
                            string Subject = _configuration.GetValue<string>("EReceiptSubject");
                            cardTrans.IS_MAIL_SENT = await _messageService.SendSMTPEmail(cardTrans.EMAIL, EmailMessage, Subject);

                            await _context.SaveChangesAsync();
                        }


                    }

                    return paymentResult;
                }

                return null;
            }
            catch (Exception ex)
            {
                await _loggerService.AddLog(ex.Message);
                throw ex;
            }
        }

        public async Task<bool> ResendNotification(int TransID)
        {
            try
            {
                var cardTrans = await _context.PrisonsAywaCardTrans.Where(x => x.ID == TransID).FirstOrDefaultAsync();
                var cardStock = await _context.PrisonsAywaCardStocks.Where(x => x.ID == cardTrans.CARD_STOCK_ID).FirstOrDefaultAsync();

                bool IsEng = true;
                string SMSMessage = ReadTemplate(true, IsEng);
                string EmailMessage = ReadTemplate(false, IsEng);

                string shorturl = InvoiceURL + "?transactionID=" + cardTrans.ID;
                string tinyURL = await _messageService.ShortURL(shorturl);

                SMSMessage = string.Format(SMSMessage, tinyURL, cardStock.CARDNUMBER);
                EmailMessage = string.Format(EmailMessage, tinyURL, cardStock.CARDNUMBER);

                cardTrans.IS_SMS_SENT = await _messageService.SendUnifonicSMS(cardTrans.MOBILE, SMSMessage);
                string Subject = _configuration.GetValue<string>("EReceiptSubject");
                cardTrans.IS_MAIL_SENT = await _messageService.SendSMTPEmail(cardTrans.EMAIL, EmailMessage, Subject);
                await _context.SaveChangesAsync();
                return await Task.FromResult<bool>(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> BulkSendSMS()
        {
            var listOfTrans = await _context.PrisonsAywaCardTrans
                .Where(x => (x.TRANS_TIME.Contains("14-JUL-22") || x.TRANS_TIME.Contains("15-JUL-22")) && x.TRANS_TYPE == "Consumption").ToListAsync();

            foreach (var trans in listOfTrans)
            {
                await ResendNotification(trans.ID);
            }
            return true;
        }
        private string ReadTemplate(bool IsSMS, bool IsEng)
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
