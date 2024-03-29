using Go.Process.InvoiceURLProducer.Data;
using Go.Process.InvoiceURLProducer.Data.Entities;
using Go.Process.InvoiceURLProducer.Models;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Font;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using QRCoder;
using RestSharp;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Image = System.Drawing.Image;

namespace Go.Process.InvoiceURLProducer
{
    public interface ICustomerInvoiceService
    {
        Task Process();
        Task RepushToSADAD();

        Task ResendSMS();

        Task GeneratePDF();

        Task DownloadPDF();
        Task DownloadALLPDF();
    }
    public class CustomerInvoiceService : ICustomerInvoiceService
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

        private readonly ILogger<CustomerInvoice> _logger;
        private readonly BRMContext _context;
        private readonly BRMDBContext2 _context2;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfigurationRoot _configuration;
        public CustomerInvoiceService(ILoggerFactory loggerFactory, IConfigurationRoot configuration, BRMContext context, BRMDBContext2 context2,
            IServiceProvider serviceProvider)
        {

            _logger = loggerFactory.CreateLogger<CustomerInvoice>();
            TinyURL = configuration.GetValue<string>("TinyURL");
            InvoiceURL = configuration.GetValue<string>("InvoiceURL");
            SMSURL = configuration.GetValue<string>("SMSURL");
            // generatePdf = _generatePdf;
            ConversationID = configuration.GetValue<string>("ConversationID");
            SenderMobile = configuration.GetValue<string>("SenderMobile");
            MaxSaveRecord = configuration.GetValue<int>("RecordsSaveOn");
            IsSendSMS = configuration.GetValue<bool>("IsSendSMS");
            // EngSMSMessage = configuration.GetValue<string>("EngSMSMessage");
            // ArabicSMSMessage = configuration.GetValue<string>("ArabicSMSMessage");
            _context = context;
            _context2 = context2;
            _serviceProvider = serviceProvider;
            _configuration = configuration;

        }

        public async Task RepushToSADAD()
        {
            //var InvoiceList = await _context.CustomerInvoices.Where(x => (x.SADAD_STATUS == null || x.SADAD_STATUS.Trim() == "0")).ToListAsync();
            //foreach (var invoice in InvoiceList)
            //{
            //    var SADADUpload = await UploadBillToSADAD(invoice);
            //    if (SADADUpload == 1)
            //    {
            //        invoice.SADAD_STATUS = "1";
            //        var IsOrderCreated = await CreateOrder(invoice);
            //        if (IsOrderCreated == 1)
            //            invoice.IS_ORDERED = 1;
            //        else
            //            invoice.IS_ORDERED = 0;

            //    }
            //    else
            //    {
            //        invoice.SADAD_STATUS = "0";
            //    }
            //    await _context.SaveChangesAsync();
            //}
        }


        public async Task Process()
        {
            _logger.LogInformation("Start Process" + DateTime.Now.ToString());
            try
            {
               var InvoiceList = await _context.CustomerInvoices.Where(x => (x.SMS_STATUS == null || x.SMS_STATUS.Trim() == "")).ToListAsync();

               // var InvoiceList = await _context.CustomerInvoices.Where(x => (x.BILL_NO == "B1-109241219")).ToListAsync();

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

                foreach (var invoice in InvoiceList)
                {
                    Console.WriteLine("Start Order for account:" + invoice.ACCOUNT_NO);
                    string shortURL = InvoiceURL + invoice.POID_ID0; // ShortURL(InvoiceURL + invoice.POID_ID0);
                    invoice.SMS_LINK = shortURL;
                    invoice.SMS_TIME = DateTime.Now.ToString();
                    string SMSMsg = "";
                    string Month = DateTime.Parse(invoice.START_T).ToString("MMM-yyyy");
                    double billDays = DateTime.Parse(invoice.DUE_T).Subtract(DateTime.Parse(invoice.END_T)).TotalDays;

                    if (invoice.LANGUAGE == "English")
                    {
                        SMSMsg = string.Format(EngSMSMessage, invoice.CUSTOMER_NAME, invoice.PLAN_NAME, Month, invoice.CURRENT_TOTAL, invoice.TOTAL_DUE,
                            invoice.ACCOUNT_NO, billDays, shortURL);
                    }
                    else
                    {
                        SMSMsg = ArabicSMSMessage;
                        SMSMsg = SMSMsg.Replace("{0}", invoice.CUSTOMER_NAME_AR);
                        SMSMsg = SMSMsg.Replace("{1}", invoice.PLAN_NAME);
                        SMSMsg = SMSMsg.Replace("{2}", Month);
                        SMSMsg = SMSMsg.Replace("{3}", invoice.CURRENT_TOTAL.ToString());
                        SMSMsg = SMSMsg.Replace("{4}", invoice.TOTAL_DUE.ToString());
                        SMSMsg = SMSMsg.Replace("{5}", invoice.ACCOUNT_NO);
                        SMSMsg = SMSMsg.Replace("{6}", billDays.ToString());
                        SMSMsg = SMSMsg.Replace("{7}", shortURL);
                    }
                    // = invoice.LANGUAGE != null ? (invoice.LANGUAGE == "English" ? EngSMSMessage : ArabicSMSMessage) : EngSMSMessage;



                    //invoice.SADAD_STATUS = "1";
                    GetRealtime(invoice);
                    var OrderID = await CreateOrder(invoice);
                    if (OrderID != "0")
                    {
                        invoice.IS_ORDERED = 1;
                        var SADADUpload = await UploadBillToSADAD(invoice, OrderID);
                        if (SADADUpload == 1)
                        {
                            if (IsSendSMS)
                            {
                                string status = SendSMS(invoice.PHONE, SMSMsg, invoice.LANGUAGE);
                                if (status == "1")
                                    invoice.SMS_STATUS = "1";
                                else
                                    invoice.SMS_STATUS = "0";

                               
                            }
                            invoice.SADAD_STATUS = "1";
                            invoice.SADAD_TIME = DateTime.Now.ToString();
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            invoice.SADAD_STATUS = "0";
                            invoice.SADAD_TIME = DateTime.Now.ToString();
                        }
                    }
                    else
                    {
                        invoice.IS_ORDERED = 0;
                        await _context.SaveChangesAsync();
                    }



                    count = count + 1;
                    if (count == MaxSaveRecord)
                    {
                        await _context.SaveChangesAsync();
                        count = 0;

                    }
                    Console.WriteLine("End Order for account:" + invoice.ACCOUNT_NO);
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
                _logger.LogInformation(result);
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


        public async Task<int> UploadBillToSADAD(CustomerInvoice parameter, string OrderId)
        {
            int responseResult = 0;

            var SadadBillReq = new SADADServiceRequestModel();
            //var SadadBillRes = await GenerateSADADServiceID(SadadBillReq);
            if (parameter != null)
            {
                //Upload bill to sadad
                var SadadUploadURL = _configuration.GetValue<string>("SADADUploadURL");

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.CreateHttp(SadadUploadURL);
                webRequest.ContentType = "text/xml; charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                DateTime cuurentTime = DateTime.Now;

                string billCycle = "INV_" + cuurentTime.ToString("yy-MM-dd");
                string billGenTimeStamp = cuurentTime.ToString("yyyy-MM-dd'T'hh:mm:ss.fffZ");

                TimeZoneInfo AST = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");

                string DueDate = DateTime.Parse(parameter.DUE_T).ToString("yyyy-MM-dd'T'HH:mm:sszzz");
                string ExpDate = (DateTime.Parse(parameter.DUE_T).AddDays(30)).ToString("yyyy-MM-dd'T'HH:mm:sszzz");

                //bool IsTrialAmt = _configuration.GetValue<bool>("UseTrialAmount");
                //if (IsTrialAmt)
                //    parameter.TOTAL_DUE = 1;

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
                            </soapenv:Envelope>", "BillNew", "Recurring", billCycle, OrderId,
                            parameter.ACCOUNT_NO, parameter.TOTAL_DUE, billGenTimeStamp, DueDate, ExpDate);

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

                                responseResult = 1;
                                //Insert Record in Order table.

                            }
                            else
                            {
                                responseResult = 0;
                                _logger.LogError(result);

                            }
                        }

                    }
                }
                catch (WebException ex)
                {
                    result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    _logger.LogError(result);
                    throw ex;

                }

            }
            return responseResult;
        }

        private async Task<string> CreateOrder(CustomerInvoice invoice)
        {

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var order = new Order();
                    order.ORDER_ID = GenerateOrderID();
                    order.MASTER_ORDER_ID = order.ORDER_ID;
                    order.ORDER_TYPE = "Invoice";
                    order.ORDER_CREATION_DATE = DateTime.Now;
                    order.ORDER_STATUS = "InProgress";
                    order.SUBSCRIBER_ID = invoice.SUBSCRIBER_ID;
                    order.PLAN_ID = invoice.PLAN_ID;
                    order.PYMT_METHOD = "SADAD";
                    order.ACCOUNT_ID = invoice.ACCOUNT_NO;
                    order.PAYMENT_AMOUNT = invoice.TOTAL_DUE;
                    order.ORIGINAL_AMOUNT = invoice.TOTAL_DUE;
                    order.CREATED_BY = "System";
                    order.CHANNEL_ID = invoice.SUBSCRIBER_ID;
                    order.CHANNEL_TYPE = "SELFCARE";

                    await _context.Orders.AddAsync(order);


                    var objInvoice = new Invoice();
                    //var objMaxID = _context.Invoices.Max(x => x.INVOICE_ID).FirstOrDefault();

                    objInvoice.INVOICE_ID = order.ORDER_ID;
                    objInvoice.ACCOUNT_ID = invoice.ACCOUNT_NO;
                    objInvoice.BILL_AMOUNT = invoice.DUE;
                    objInvoice.MIN_AMOUNT_DUE = invoice.TOTAL_DUE;
                    objInvoice.STATEMENT_DATE = DateTime.Now;
                    objInvoice.DUE_DATE = DateTime.Parse(invoice.DUE_T);
                    objInvoice.CREATED_DATE = DateTime.Now;
                    objInvoice.CREATED_BY = "system";
                    objInvoice.BILL_NO = invoice.BILL_NO;
                    objInvoice.BILL_STATUS = "Completed";

                    await _context.Invoices.AddAsync(objInvoice);
                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return order.ORDER_ID;
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(ex.Message);
                    return "0";
                }
            }

        }
        private string GenerateOrderID()
        {
            //pkg_id_generation_api.prc_invoice_id(pov_invoice_order_id)
            OracleParameter p1 = new OracleParameter("pov_invoice_order_id", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
            string query = "BEGIN pkg_id_generation_api.prc_invoice_id(:pov_invoice_order_id);END;";
            var result = _context.Database.ExecuteSqlRaw(query, p1);
            return p1.Value.ToString();
        }

        public async Task GeneratePDF()
        {
            try
            {
                var invoiceList = await _context.CustomerInvoices.Where(x => (x.BILL_NO == "B1-4366281")).ToListAsync();
                //var invoiceList = (from c in _context.CustomerInvoices
                //                   join inv in _context.Invoices
                //                   on c.BILL_NO equals inv.BILL_NO
                //                   where c.PDF_STATUS == null && inv.INVOICE_DATA_PDF == null
                //                   select c).ToList();
                int record = 1;
                int count = 1;
                foreach (var inv in invoiceList)
                {

                    string data = GenerateInvoiceHTML(inv);


                    HtmlToPdf converter = new HtmlToPdf();
                    converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                    converter.Options.MarginLeft = 5;
                    converter.Options.MarginRight = 5;

                    SelectPdf.PdfDocument doc = converter.ConvertHtmlString(data);
                    // save pdf document
                    doc.Save(inv.BILL_NO + ".pdf");

                    // close pdf document
                    doc.Close();
                    byte[] file = System.IO.File.ReadAllBytes(inv.BILL_NO + ".pdf");
                
                    System.IO.File.Delete(inv.BILL_NO + ".pdf");
                    OracleConnection connection = new OracleConnection();
                    connection.ConnectionString = _configuration.GetConnectionString("BRMDBConnectionString");
                    if (connection.State != ConnectionState.Open) connection.Open();

                    OracleCommand command = new OracleCommand("Update invoice_tbl set INVOICE_DATA_PDF =  :BlobParameter , INVOICE_DATA_PDF_AR = :BlobParameter where BILL_NO ='" + inv.BILL_NO + "'", connection);

                    OracleParameter blobParameter = new OracleParameter();
                    blobParameter.OracleDbType = OracleDbType.Blob;
                    blobParameter.ParameterName = "BlobParameter";
                    blobParameter.Value = file;

                    command.Parameters.Add(blobParameter);
                    command.ExecuteNonQuery();

                    inv.PDF_STATUS = "1";
                    if (count == 10)
                    {
                        _context.SaveChanges();
                        count = 1;
                    }
                    Console.WriteLine(record + " ("+ inv.BILL_NO + ") " + "- Invoice Processed");
                    count += 1;
                    record += 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
        private string GenerateQR(CustomerInvoice result)
        {
            string VATNo = _configuration.GetValue<string>("VATNo");
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            //string QRText = "Customer Name: " + result.CUSTOMER_NAME + System.Environment.NewLine;
            string QRText = " Merchant Name: Etihad Atheeb Telecom Company" + System.Environment.NewLine;
            QRText += " VAT Reg.No.: " + VATNo + System.Environment.NewLine;
            QRText += " Invoice Date: " + result.END_T + System.Environment.NewLine;
            QRText += " Total VAT: " + result.CYCLE_TAX + System.Environment.NewLine;
            QRText += " Total Due: " + result.TOTAL_DUE;
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(QRText, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(10);
            byte[] BitmapArray = BitmapToByteArray(QrBitmap);
            byteArrayToImage(BitmapArray, result.BILL_NO);
            //string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            return result.BILL_NO;
        }
        public void byteArrayToImage(byte[] bytesArr, string InvoiceId)
        {
            string ImagePath = _configuration.GetValue<string>("QRCodePath");
            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                Image img = Image.FromStream(memstr);

                img.Save(ImagePath + "\\" + InvoiceId + ".jpg");
                img.Dispose();
            }
        }
        public async Task DownloadALLPDF()
        {
            DateTime mydate = DateTime.Parse(_configuration.GetValue<string>("FromDate"));
            try
            {
                
                var invoiceList = (from c in _context.CustomerInvoices
                                   join inv in _context.Invoices
                                   on c.BILL_NO equals inv.BILL_NO
                                   where inv.CREATED_DATE >= mydate && c.POID_ID0 > 8286925712
                                   select c).OrderBy(c=>c.POID_ID0).ToList();
                //var testList = (from inv in _context.Invoices
                //                   where inv.CREATED_DATE >= mydate
                //                   select inv).ToList();
                int record = 1;
                foreach (var inv in invoiceList)
                {
                    string data = GenerateInvoiceHTML(inv);
                    HtmlToPdf converter = new HtmlToPdf();
                    converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                    converter.Options.WebPageWidth = 1000;
                    var path = _configuration.GetValue<string>("QRCodePath");
                    SelectPdf.PdfDocument doc = converter.ConvertHtmlString(data, path);
                    // save pdf document
                    doc.Save("Invoices/" + inv.BILL_NO + ".pdf");
                    doc.Close();
                    Console.WriteLine(record + " (" + inv.BILL_NO + ") " + "- Invoice Generated");
                    record += 1;
                }
                //Console.WriteLine(record + " - Invoices Generated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception Occured while Downloading Invoices:" + ex.Message);
                throw ex;
            }

            //try
            //{
            //    OracleConnection connection = new OracleConnection();
            //    connection.ConnectionString = _configuration.GetConnectionString("BRMDBConnectionString");
            //    if (connection.State != ConnectionState.Open) connection.Open();

            //    OracleCommand command = new OracleCommand(@"select BILL_NO,CREATED_DATE,INVOICE_DATA_PDF from invoice_tbl where CREATED_DATE >= to_date('01-AUG-22','DD-MON-YY') and rownum <= 5", connection);
                
            //    OracleDataAdapter da = new OracleDataAdapter(command);
            //    DataTable dt = new DataTable();
            //    da.Fill(dt);
            //    List<byte> list = null;
            //    //foreach (DataRow dr in dt.Rows)
            //    //{
            //    //    list = new List<byte>();
            //    //    for (int i = 1; i <= 15; i++)
            //    //    {
            //    //        list.AddRange((byte[])(dr["Payload" + i]));
            //    //    }
            //    //    System.IO.File.WriteAllBytes(dr["BILL_NO"] + ".pdf", list.ToArray());
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}


        }
        public async Task DownloadPDF()
        {
            try
            {
                //var pdfInvoice = await _context.Invoices.Where(x => x.BILL_NO == "B1-382280").FirstOrDefaultAsync();
                OracleConnection connection = new OracleConnection();
                connection.ConnectionString = _configuration.GetConnectionString("BRMDBConnectionString");
                if (connection.State != ConnectionState.Open) connection.Open();
                OracleCommand command = new OracleCommand("select * from invoice_tbl where Bill_No ='B1-396779'", connection);
                OracleDataAdapter da = new OracleDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                System.IO.File.WriteAllBytes("Test.pdf", (byte[])dt.Rows[0]["INVOICE_DATA_PDF"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateInvoiceHTML(CustomerInvoice inv)
        {

            StreamReader sr = new StreamReader("InvoiceTemplate.html");
            inv = GetRealtime(inv);
            string qr = GenerateQR(inv);
            string s = sr.ReadToEnd();
            string invoicehtml = string.Format(s, inv.CUSTOMER_NAME, inv.ACCOUNT_NO, inv.PLAN_NAME, inv.PLAN_PRICE,
                inv.END_T, inv.START_T.ToString() + " to " + @DateTime.Parse(inv.END_T).AddDays(-1).ToString("dd-MMM-yy"), inv.BILL_NO,
                inv.DUE_T, inv.CREDIT_LIMIT, inv.TOTAL_DUE, inv.PAID_AMOUNT, inv.PREVIOUS_TOTAL, inv.CURRENT_TOTAL, inv.CYCLE_TAX, inv.CURRENT_TOTAL_WV, qr + ".jpg",inv.START_T.ToString(), @DateTime.Parse(inv.END_T).AddDays(-1).ToString("dd-MMM-yy"));

            return invoicehtml;


        }
        private CustomerInvoice GetRealtime(CustomerInvoice invoice)
        {
            try
            {

                OracleParameter p1 = new OracleParameter("piv_bill_no", invoice.BILL_NO);

                OracleParameter p2 = new OracleParameter("Pov_total_due", OracleDbType.Double, 50, null, ParameterDirection.Output);
                OracleParameter p3 = new OracleParameter("Pov_current_Total", OracleDbType.Double, 50, null, ParameterDirection.Output);
                OracleParameter p4 = new OracleParameter("pov_previous_total", OracleDbType.Double, 50, null, ParameterDirection.Output);
                OracleParameter p5 = new OracleParameter("pov_payment", OracleDbType.Double, 50, null, ParameterDirection.Output);

                OracleParameter p6 = new OracleParameter("pov_status", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p7 = new OracleParameter("pov_Cycle_Tax", OracleDbType.Double, 50, null, ParameterDirection.Output);
                OracleParameter p8 = new OracleParameter("pov_Current_WV", OracleDbType.Double, 50, null, ParameterDirection.Output);

                string query = "BEGIN PIN.PRC_INVOICE_BILL(:piv_bill_no, :Pov_total_due, :Pov_current_Total,:pov_previous_total, :pov_payment,:pov_status,:pov_Cycle_Tax,:pov_Current_WV);END;";
                var result = _context2.Database.ExecuteSqlRaw(query, p1, p2, p3, p4, p5, p6, p7, p8);
                invoice.TOTAL_DUE = p2.Value != null ? Double.Parse(p2.Value.ToString()) : 0;
                invoice.CURRENT_TOTAL = p3.Value != null ? Double.Parse(p3.Value.ToString()) : 0;
                invoice.PREVIOUS_TOTAL = p4.Value != null ? Double.Parse(p4.Value.ToString()) : 0;
                invoice.PAID_AMOUNT = p5.Value != null ? Double.Parse(p5.Value.ToString()) : 0;
                invoice.CYCLE_TAX = p7.Value != null ? Double.Parse(p7.Value.ToString()) : 0; ;
                invoice.CURRENT_TOTAL_WV = p8.Value != null ? Double.Parse(p8.Value.ToString()) : 0; ;
                return invoice;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task ResendSMS()
        {
            //var result = _context.
            try
            {
                var InvoiceList = await (from inv in _context.CustomerInvoices
                                         join not_send in _context.NOT_SEND_ACCOUNTS
                                         on inv.ACCOUNT_NO equals not_send.ACCOUNT_NO
                                         where inv.END_T.Contains("01-AUG-22")
                                         select inv).ToListAsync();

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
                foreach (var invoice in InvoiceList)
                {
                    string shortURL = InvoiceURL + invoice.POID_ID0; // ShortURL(InvoiceURL + invoice.POID_ID0);
                    invoice.SMS_LINK = shortURL;
                    invoice.SMS_TIME = DateTime.Now.ToString();
                    string SMSMsg = "";
                    string Month = DateTime.Parse(invoice.START_T).ToString("MMM-yyyy");
                    double billDays = DateTime.Parse(invoice.DUE_T).Subtract(DateTime.Parse(invoice.END_T)).TotalDays;

                    if (invoice.LANGUAGE == "English")
                    {
                        SMSMsg = string.Format(EngSMSMessage, invoice.CUSTOMER_NAME, invoice.PLAN_NAME, Month, invoice.CURRENT_TOTAL, invoice.TOTAL_DUE,
                            invoice.ACCOUNT_NO, billDays, shortURL);
                    }
                    else
                    {
                        SMSMsg = ArabicSMSMessage;
                        SMSMsg = SMSMsg.Replace("{0}", invoice.CUSTOMER_NAME_AR);
                        SMSMsg = SMSMsg.Replace("{1}", invoice.PLAN_NAME);
                        SMSMsg = SMSMsg.Replace("{2}", Month);
                        SMSMsg = SMSMsg.Replace("{3}", invoice.CURRENT_TOTAL.ToString());
                        SMSMsg = SMSMsg.Replace("{4}", invoice.TOTAL_DUE.ToString());
                        SMSMsg = SMSMsg.Replace("{5}", invoice.ACCOUNT_NO);
                        SMSMsg = SMSMsg.Replace("{6}", billDays.ToString());
                        SMSMsg = SMSMsg.Replace("{7}", shortURL);
                    }

                    string status = "0";// SendSMS(invoice.PHONE, SMSMsg, invoice.LANGUAGE);
                    if (status == "1")
                        invoice.SMS_STATUS = "1";
                    else
                        invoice.SMS_STATUS = "0";

                    await _context.SaveChangesAsync();
                    count += 1;
                    Console.WriteLine("Number of records processed: " + count);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
