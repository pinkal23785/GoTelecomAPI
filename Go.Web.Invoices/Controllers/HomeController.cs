using Go.Web.Invoices.Data;
using Go.Web.Invoices.Models;
using Microsoft.AspNetCore.Mvc;
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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataService _dataService;
        public HomeController(ILogger<HomeController> logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }

        public async Task<IActionResult> Index(int ID)
        {
            try
            {
                var result = await _dataService.getCustomerInvoice(ID);
                if (result != null)
                {
                    QRCodeGenerator QrGenerator = new QRCodeGenerator();
                    string QRText = "Customer Name:" + result.CUSTOMER_NAME + ";";
                    //QRText += "Package Name:" + result.PLAN_NAME + "/n";
                    //QRText += "Package Price:" + result.PLAN_PRICE + "/n";
                    QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(QRText, QRCodeGenerator.ECCLevel.Q);
                    QRCode QrCode = new QRCode(QrCodeInfo);
                    Bitmap QrBitmap = QrCode.GetGraphic(60);
                    byte[] BitmapArray = QrBitmap.BitmapToByteArray();
                    string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
                    ViewBag.QrCodeUri = QrUri;
                }
                return View(result);
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> TestIframe()
        {
            return View();
        }
    }
}
