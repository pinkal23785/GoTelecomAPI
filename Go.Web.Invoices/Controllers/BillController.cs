using Go.Web.Invoices.Data;
using Go.Web.Invoices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Web.Invoices.Controllers
{
    public class BillController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataService _dataService;
        private readonly IConfiguration _configuration;
        private readonly ReCaptcha _captcha;
        public BillController(ILogger<HomeController> logger, IDataService dataService, IConfiguration configuration, ReCaptcha captcha)
        {
            _logger = logger;
            _dataService = dataService;
            _configuration = configuration;
            _captcha = captcha;

        }

        public async Task<IActionResult> ValidateInvoice(Int64 ID)
        {
            ViewBag.SiteKey = _configuration.GetValue<string>("SiteKey");
            TempData["ID"] = Convert.ToString(ID);
            TempData.Keep();
            return View();
        }
        public async Task<IActionResult> Invoice(Int64? ID)
        {
            try
            {

                //if (Request.HasFormContentType == true && Request.Form.ContainsKey("g-recaptcha-response"))
                //{
                //var captcha = Request.Form["g-recaptcha-response"].ToString();
                //if (!await _captcha.IsValid(captcha, _configuration.GetValue<string>("SecretKey")))
                //{
                //    return RedirectToAction("ValidateInvoice", "Bill", new { ID = TempData["ID"].ToString() });
                //}
                if (ID != null)
                {
                    var result = await _dataService.getCustomerInvoice(ID.Value);
                    //TempData.Keep();
                    if (result != null)
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
                        Bitmap QrBitmap = QrCode.GetGraphic(120);
                        byte[] BitmapArray = QrBitmap.BitmapToByteArray();
                        string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
                        ViewBag.QrCodeUri = QrUri;
                    }
                    return View(result);
                }
                //}

                return Error();
                //return RedirectToAction("ValidateInvoice", "Bill", new { ID = TempData["ID"].ToString() });
            }
            catch (Exception ex)
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }


        }
        public async Task<IActionResult> NewInvoice(Int64? ID)
        {
            try
            {

                //if (Request.HasFormContentType == true && Request.Form.ContainsKey("g-recaptcha-response"))
                //{
                //var captcha = Request.Form["g-recaptcha-response"].ToString();
                //if (!await _captcha.IsValid(captcha, _configuration.GetValue<string>("SecretKey")))
                //{
                //    return RedirectToAction("ValidateInvoice", "Bill", new { ID = TempData["ID"].ToString() });
                //}
                if (ID != null)
                {
                    var result = await _dataService.getCustomerInvoice(ID.Value);
                    //TempData.Keep();
                    if (result != null)
                    {
                        string VATNo = _configuration.GetValue<string>("VATNo");
                        QRCodeGenerator QrGenerator = new QRCodeGenerator();
                        //string QRText = "Customer Name: " + result.CUSTOMER_NAME + System.Environment.NewLine;
                        string QRText = " Merchant Name: GO Telecom" + System.Environment.NewLine;
                        QRText += " VAT Reg.No.: " + VATNo + System.Environment.NewLine;
                        QRText += " Invoice Date: " + result.END_T + System.Environment.NewLine;
                        QRText += " Total VAT: " + result.CYCLE_TAX + System.Environment.NewLine;
                        QRText += " Total Due: " + result.TOTAL_DUE;
                        QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(QRText, QRCodeGenerator.ECCLevel.Q);
                        QRCode QrCode = new QRCode(QrCodeInfo);
                        Bitmap QrBitmap = QrCode.GetGraphic(120);
                        byte[] BitmapArray = QrBitmap.BitmapToByteArray();
                        string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
                        ViewBag.QrCodeUri = QrUri;
                    }
                    return View(result);
                }
                //}

                return Error();
                //return RedirectToAction("ValidateInvoice", "Bill", new { ID = TempData["ID"].ToString() });
            }
            catch (Exception ex)
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> ViewOrderInvoice(string ID)
        {
            var result = await _dataService.GetEInvoice_Audit(ID);
            if (result != null)
            {
                string VATNo = _configuration.GetValue<string>("VATNo");
                QRCodeGenerator QrGenerator = new QRCodeGenerator();
                //string QRText = "Customer Name: " + result.CUSTOMER_NAME + System.Environment.NewLine;
                string QRText = " Merchant Name: Etihad Atheeb Telecom Company" + System.Environment.NewLine;
                QRText += " Order No.: " + result.ORDER_ID + System.Environment.NewLine;
                QRText += " VAT Reg.No.: " + VATNo + System.Environment.NewLine;
                QRText += " Invoice Date: " + result.INVOICEDATE + System.Environment.NewLine;
                QRText += " Total Amount With VAT: " + result.PAYMENT_AMT + System.Environment.NewLine;
                QRText += " Total VAT: " + result.TAX_AMT + System.Environment.NewLine;


                QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(QRText, QRCodeGenerator.ECCLevel.Q);
                QRCode QrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = QrCode.GetGraphic(120);
                byte[] BitmapArray = QrBitmap.BitmapToByteArray();
                string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
                ViewBag.QrCodeUri = QrUri;
            }
            return View(result);
        }


    }
}
