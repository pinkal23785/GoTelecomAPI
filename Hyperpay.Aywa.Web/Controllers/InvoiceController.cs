using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Hyperpay.Aywa.Web.Data;
using Hyperpay.Aywa.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QRCoder;

namespace Hyperpay.Aywa.Web.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IDataService _dataservice;
        private readonly IConfiguration _configuration;
        public InvoiceController(IDataService dataservice, IConfiguration configuration)
        {
            _dataservice = dataservice;
            _configuration = configuration;
        }
        public async Task<IActionResult> ViewEInvoice(string transactionID)
        {
            if (!string.IsNullOrEmpty(transactionID))
            {
                InvoiceModel result = await _dataservice.GetInvoiceDetails(transactionID);
                int vat = _configuration.GetValue<int>("VATPercent");
                result.VAT_Amount = Math.Round(result.Card_Amount.Value * vat / 100,2);
                result.VatNumber = _configuration.GetValue<string>("VATNo");
                QRCodeGenerator QrGenerator = new QRCodeGenerator();
                string QRText = " Merchant Name: Etihad Atheeb Telecom Company" + System.Environment.NewLine;
                QRText += " VAT Reg.No.: " + result.VatNumber + System.Environment.NewLine;
                QRText += " Invoice Date: " + result.InvoiceDate + System.Environment.NewLine;
                QRText += " Total VAT: " + result.VAT_Amount + System.Environment.NewLine;
                QRText += " Total Amount: " + result.TotalAmount;
                QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(QRText, QRCodeGenerator.ECCLevel.Q);
                QRCode QrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = QrCode.GetGraphic(120);
                byte[] BitmapArray = QrBitmap.BitmapToByteArray();
                string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
                ViewBag.QrCodeUri = QrUri;

                return View(result);
            }
            return View();
        }
    }
}
