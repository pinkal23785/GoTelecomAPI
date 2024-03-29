using GO.Process.EInvoiceProducer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GO.Process.EInvoiceProducer
{
    public interface IProcessInvoice
    {
        Task Process();
    }
    public class ProcessInvoice : IProcessInvoice
    {
        private readonly string InvoiceURL;
        private readonly string TinyURL;
        private readonly string SMSURL;
        private readonly int MaxSaveRecord;
        private readonly bool IsSendSMS;

        private readonly string ConversationID;
        private readonly string SenderMobile;
        private string EngSMSMessage;
        private string ArabicSMSMessage;

        private readonly ILogger<ProcessInvoice> _logger;
        private readonly CADBContext _context;
        private readonly IServiceProvider _serviceProvider;

        public ProcessInvoice(ILoggerFactory loggerFactory, IConfigurationRoot configuration,
            CADBContext context, IServiceProvider serviceProvider)
        {

            _logger = loggerFactory.CreateLogger<ProcessInvoice>();
            TinyURL = configuration.GetValue<string>("TinyURL");
            InvoiceURL = configuration.GetValue<string>("InvoiceURL");
            SMSURL = configuration.GetValue<string>("SMSURL");

            ConversationID = configuration.GetValue<string>("ConversationID");
            SenderMobile = configuration.GetValue<string>("SenderMobile");
            MaxSaveRecord = configuration.GetValue<int>("RecordsSaveOn");
            IsSendSMS = configuration.GetValue<bool>("IsSendSMS");
            // EngSMSMessage = configuration.GetValue<string>("EngSMSMessage");
            // ArabicSMSMessage = configuration.GetValue<string>("ArabicSMSMessage");
            _context = context;
            _serviceProvider = serviceProvider;
        }



        public async Task Process()
        {
            _logger.LogInformation("Start Process" + DateTime.Now.ToString());
            try
            {

                //string sql = $@"SELECT Order_Id, Account_Id, Payment_Amount, Tax_Amount,Pymt_Method Payment_Method,res.first_name Customer_Name,
                //                 res.first_name_arabic Customer_Name_AR,res.email_id Email,res.mobile_no Mobile,cit.Subscriber_Id,
                //                 cit.preferred_language Language,o.Order_Type
                //                 From order_tbl o, cust_info_Tbl cit,cust_residential_tbl res 
                //                 where order_creation_Date >= (SYSDATE - 3950 / 24) and order_Status ='Completed'
                //                 and o.subscriber_id = cit.subscriber_id and o.subscriber_id = res.subscriber_id
                //                 and payment_amount>0 order by order_id desc";

                string sql = $@"SELECT Order_Id, Account_Id, Payment_Amount, Tax_Amount,Pymt_Method Payment_Method,res.first_name Customer_Name,
res.first_name_arabic Customer_Name_AR,res.email_id Email,res.mobile_no Mobile,cit.Subscriber_Id,
cit.preferred_language Language,o.Order_Type
From order_tbl o, cust_info_Tbl cit,cust_residential_tbl res
where order_creation_Date >= (SYSDATE - 1 / 24) and
order_Status ='Completed'
and order_type not in ('Adjustment')
--and account_id ='106298514'
and o.subscriber_id = cit.subscriber_id and o.subscriber_id = res.subscriber_id;";

                var result = await _context.SearchOrders.FromSqlRaw(sql).ToListAsync();


                //   var InvoiceList = await _context.CustomerInvoices.Where(x => (x.SMS_STATUS == null || x.SMS_STATUS.Trim() == "")).ToListAsync();

                XmlDocument doc = new XmlDocument();
                using (XmlReader reader = XmlReader.Create(@"Template.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            //return only when you have START tag  
                            switch (reader.Name.ToString())
                            {
                                case "English":
                                    EngSMSMessage = reader.ReadInnerXml().ToString();
                                    break;
                                case "Arabic":
                                    ArabicSMSMessage = reader.ReadInnerXml().ToString();
                                    break;
                            }
                        }
                    }
                }
                int count = 0;

                foreach (var invoice in result)
                {
                    EInvoice_Audit audit = new EInvoice_Audit();
                    string shortURL = ShortURL(InvoiceURL + invoice.Order_ID);
                    audit.SMS_LINK = shortURL;
                    //audit.SMS_TIME = DateTime.Now.ToString();
                    audit.CUSTOMER_NAME = invoice.Customer_Name;
                    audit.CUSTOMER_NAME_AR = invoice.Customer_Name_AR;
                    audit.EMAIL = invoice.Email;
                    audit.ACCOUNT_ID = invoice.Account_ID;
                    audit.MOBILE = invoice.Mobile;
                    audit.ORDER_ID = invoice.Order_ID;
                    audit.PAYMENT_AMT = invoice.Payment_Amount;
                    audit.PAYMENT_METHOD = invoice.Payment_Method;
                    audit.SUBSCRIBER_ID = invoice.Subscriber_ID;
                    audit.TAX_AMT = invoice.Tax_Amount;
                    audit.LANGUAGE = invoice.Language;
                    audit.INVOICEDATE = DateTime.Now;
                    audit.ORDER_TYPE = invoice.Order_Type;
                    string SMSMsg = "";
                    //string Month = DateTime.Parse(invoice.START_T).ToString("MMM-yyyy");
                    //double billDays = DateTime.Parse(invoice.DUE_T).Subtract(DateTime.Parse(invoice.END_T)).TotalDays;

                    if (invoice.Language == "English")
                    {
                        SMSMsg = string.Format(EngSMSMessage, invoice.Customer_Name, invoice.Order_ID, invoice.Payment_Amount, shortURL);
                    }
                    else
                    {
                        SMSMsg = ArabicSMSMessage;
                        SMSMsg = SMSMsg.Replace("{0}", invoice.Customer_Name_AR);
                        SMSMsg = SMSMsg.Replace("{1}", invoice.Order_ID);
                        SMSMsg = SMSMsg.Replace("{2}", invoice.Payment_Amount.ToString());
                        SMSMsg = SMSMsg.Replace("{7}", shortURL);
                    }
                    // = invoice.LANGUAGE != null ? (invoice.LANGUAGE == "English" ? EngSMSMessage : ArabicSMSMessage) : EngSMSMessage;
                    if (IsSendSMS)
                    {
                        string status = SendSMS(invoice.Mobile, SMSMsg, invoice.Language);
                        if (status == "1")
                            audit.SMS_STATUS = 1;
                        else
                            audit.SMS_STATUS = 0;

                        audit.SMS_TIME = DateTime.Now;
                    }

                    count = count + 1;
                    await _context.EInvoiceAudit.AddAsync(audit);
                    if (count == MaxSaveRecord)
                    {
                        await _context.SaveChangesAsync();
                        count = 0;
                    }
                }
                await _context.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private string SendSMS(string MobileNumber, string Message, string Lang)
        {
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
            _logger.LogInformation(soapstr);
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
                    _logger.LogInformation(result);
                    XDocument doc = XDocument.Parse(result);

                    _logger.LogInformation(result);
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
        private string ShortURL(string strURL)
        {
            string URL;
            URL = TinyURL + strURL.ToLower();

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
    }
}
