using Hyperpay.Aywa.Web.Data;
using Hyperpay.Aywa.Web.Models;
using Hyperpay.Aywa.Web.SADAD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Controllers
{
    public class AywaController : Controller
    {
        private readonly IOTPService _oTPService;
        private readonly IHyperService _hyperService;
        private readonly IConfiguration _configuration;
        private readonly ISADADService _SADADService;
        private readonly IDataService _dataService;
        public AywaController(IOTPService oTPService, IHyperService hyperService, IConfiguration configuration,
            ISADADService SADADService, IDataService dataService)
        {
            _oTPService = oTPService;
            _hyperService = hyperService;
            _configuration = configuration;
            _SADADService = SADADService;
            _dataService = dataService;
        }

        public async Task<IActionResult> Home(string Lang)
        {

            if (!string.IsNullOrEmpty(Request.Query["lang"]))
                ViewBag.Lang = Request.Query["lang"];
            else
                ViewBag.Lang = "en";

            if (!string.IsNullOrEmpty(Lang))
                ViewBag.Lang = Lang;
            var result = await _dataService.GetAywaaCardType();
            //ViewBag.Tax = _configuration.GetValue<int>("VATPercent");
            return View(result);
        }

        public async Task<IActionResult> GetCardDetails(int CardType, string Lang)
        {

            var objCardType = await _dataService.GetAywaaCardTypeById(CardType);
            if (objCardType != null)
            {
                ViewBag.NAME = objCardType.NAME;
                
                int VatPercent = _configuration.GetValue<int>("VATPercent");
                decimal VATAmount = Math.Round((objCardType.CREDIT * VatPercent) / 100, 2);
                ViewBag.VAT =  VATAmount;
                ViewBag.CardAmount = objCardType.CREDIT;
                ViewBag.TotalAmount = objCardType.CREDIT + VATAmount;
                ViewBag.CardType = CardType;
                ViewBag.Lang = Lang;
            }
            ViewBag.BuyMore = _configuration.GetValue<string>("BuyMoreURL");
            ViewBag.BuyMore = ViewBag.BuyMore + "&lang=" + Lang;
            return View();
        }
        public IActionResult Index(OTPModel model)
        {

            if (string.IsNullOrEmpty(model.Lang))
                model.Lang = "en";

            ViewBag.BuyMore = _configuration.GetValue<string>("BuyMoreURL");
            ViewBag.BuyMore = ViewBag.BuyMore + "&lang=" + model.Lang;
            ModelState.Clear();
            if (!string.IsNullOrEmpty(Request.Query["lang"]))
                model.Lang = Request.Query["lang"];
            if (!string.IsNullOrEmpty(Request.Query["amt"]))
                model.CardAmount = decimal.Parse(Request.Query["amt"]);
            if (!string.IsNullOrEmpty(Request.Query["cardtype"]))
                model.AwayaCardType = Request.Query["cardtype"];


            return View(model);
        }
        public async Task<IActionResult> SendOTP(OTPModel model)
        {
            if (!ModelState.IsValid)
            {

                return View("Index", model);
            }
            else
            {
                string otp = await _oTPService.SendOTP(model.Mobile, model.Email, model.Lang);
                VerifyOTPModel verifymodel = new VerifyOTPModel();
                verifymodel.Mobile = model.Mobile;
                verifymodel.Email = model.Email;
                verifymodel.AwayaCardType = model.AwayaCardType;
                verifymodel.CardAmount = model.CardAmount;
                verifymodel.Lang = model.Lang;
                ModelState.Clear();
                ViewBag.BuyMore = _configuration.GetValue<string>("BuyMoreURL");
                ViewBag.BuyMore = ViewBag.BuyMore + "&lang=" + model.Lang;
                return View("VerifyOTP", verifymodel);
            }
        }

        public async Task<bool> validateOTP([FromBody] VerifyOTPModel model)
        {
            var result = await _oTPService.ValidateOTP(model.Mobile, model.OTP);
            return (result);

        }
        public async Task<IActionResult> VerifyOTP(VerifyOTPModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var result = await _oTPService.ValidateOTP(model.Mobile, model.OTP);
                if (!result)
                {
                    ModelState.AddModelError("", "Please enter correct OTP");
                    return View(model);
                }
                CheckoutModel checkmodel = new CheckoutModel();
                checkmodel.MobileNumber = model.Mobile;
                checkmodel.Email = model.Email;
                checkmodel.Amount = model.CardAmount;
                checkmodel.AywaaCardID = model.AwayaCardType;
                checkmodel.Lang = model.Lang;
                ViewBag.BuyMore = _configuration.GetValue<string>("BuyMoreURL");
                ViewBag.BuyMore = ViewBag.BuyMore + "&lang=" + model.Lang;
                return View("Checkout", checkmodel);
            }

        }
        public async Task<bool> ResendOTP([FromBody] OTPModel model)
        {
            string otp = await _oTPService.SendOTP(model.Mobile, model.Email, model.Lang);
            if (!string.IsNullOrEmpty(otp))
                return true;
            else
                return false;
        }

        public IActionResult Checkout()
        {

            return View();
        }
        public async Task<IActionResult> CheckoutHyperPay(CheckoutModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Task.Run(() => View("Checkout", model));
            }
            else if (model.CardType == "")
            {
                ModelState.AddModelError("", "Please select payment card");
                return View(model);
            }
            else
            {
                model.Amount = model.Amount;

                if (model.CardType == "sadad")
                {
                    var result = await _SADADService.UploadBillToSADAD(model);
                    return RedirectToAction("SADADResult", new { BillNumber = result.BillNumber, Amount = result.Amount, Lang = model.Lang });
                }
                else
                {
                    var result = await _hyperService.CheckoutPayment(model);
                    string RedirectURL = _configuration.GetValue<string>("PaymentResultURL");
                    PaymentWidgetModel paymentWidget = new PaymentWidgetModel();
                    paymentWidget.CheckoutId = result;
                    paymentWidget.CardType = model.CardType;

                    paymentWidget.PaymentResultURL = RedirectURL + "?AwayaCardID=" + model.AywaaCardID + "&Lang=" + model.Lang;
                    paymentWidget.Lang = model.Lang;
                    paymentWidget.Amount = model.Amount;
                    paymentWidget.AwayaCardId = model.AywaaCardID;
                    ViewBag.BuyMore = _configuration.GetValue<string>("BuyMoreURL");
                    ViewBag.BuyMore = ViewBag.BuyMore + "&lang=" + model.Lang;
                    ViewBag.HyperPayWidgetURL = _configuration.GetValue<string>("HyperPayWidgetURL") + result;
                    return View("RenderHyperPayWodget", paymentWidget);
                }
            }

        }

        public IActionResult RenderHyperPayWodget(PaymentWidgetModel model)
        {
            ViewBag.BuyMore = _configuration.GetValue<string>("BuyMoreURL");

            ViewBag.BuyMore = ViewBag.BuyMore + "&lang=" + model.Lang;
            return View(model);
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> PaymentResult(PaymentWidgetModel model)
        {
            int AwayacardID = int.Parse(Request.Query["AwayaCardID"].ToString());
            var result = await _hyperService.GetPaymentStatus(Request.Query["id"].ToString(), AwayacardID, model.Lang);
            return RedirectToAction("PaymentSuccess", new { shortURL = result.shortURL, ResultCode = result.ResultCode,
                Lang = Request.Query["Lang"], StatusDesc=result.StatusDesc, CardNumber=result.CardNumber });
        }

        public async Task<IActionResult> PaymentSuccess(PaymentSucessModel model)
        {
            ViewBag.BuyMore = _configuration.GetValue<string>("BuyMoreURL");
            ViewBag.BuyMore = ViewBag.BuyMore + "&lang=" + model.Lang;
            return View(model);
        }


        public async Task<IActionResult> SADADResult(SADADUploadResponse model)
        {
            ViewBag.BuyMore = _configuration.GetValue<string>("BuyMoreURL");
            ViewBag.BuyMore = ViewBag.BuyMore + "&lang=" + model.Lang;
            return View(model);
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpPost("aywa-card-payment-confirmation")]
        public async Task<IActionResult> ProcessAywaCardPayment(AywaCardPaymentReqModel model)
        {
            try
            {
                await _SADADService.ApplySADADPayment(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
