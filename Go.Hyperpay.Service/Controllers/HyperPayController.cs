using Go.Hyperpay.Service.Data;
using Go.Hyperpay.Service.Data.Entities;
using Go.Hyperpay.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Go.Hyperpay.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HyperPayController : ControllerBase
    {
        private IConfiguration _configuration;
        private CADBContext _context;
        private IDataService _dataService;
        private readonly ILogger _logger;
        public HyperPayController(IConfiguration configuration, CADBContext context, IDataService dataService, ILogger<HyperPayController> logger)
        {
            _configuration = configuration;
            _context = context;
            _dataService = dataService;
            _logger = logger;
        }

        [HttpPost("initiate-checkout")]
        public async Task<IActionResult> CreateCheckout(CheckoutModel parameter)
        {
            try
            {
                string responseData;
                AddLogs(JsonConvert.SerializeObject(parameter));

                StringBuilder data = new StringBuilder();
                string MerchantTransactionId = Guid.NewGuid().ToString().Replace("-", "");
                //parameter.amount = decimal.Parse("1.50");
                //data.Append("amount=" + parameter.amount);
                data.Append("amount=" + parameter.amount.ToString("F"));
                data.Append("&currency=" + parameter.currency);
                if (_configuration.GetValue<bool>("Sandbox"))
                {
                    data.Append("&merchantTransactionId=" + _configuration.GetValue<string>("MerchantTransId"));
                }
                else
                {
                    data.Append("&merchantTransactionId=" + MerchantTransactionId);
                }
                data.Append("&customer.email=" + parameter.customerEmail);
                data.Append("&billing.street1=" + parameter.billingStreet1);
                data.Append("&billing.city=" + parameter.billingCity);
                data.Append("&billing.state=" + parameter.billingState);
                data.Append("&billing.country=" + parameter.billingCountry);
                data.Append("&billing.postcode=" + parameter.billingPostcode);
                data.Append("&customer.givenName=" + parameter.customerGivenName);
                data.Append("&customer.surname=" + parameter.surname);
                data.Append("&paymentType=" + parameter.paymentType);
                string entityId = "";
                if (parameter.cardType.ToLower() == "visa" || parameter.cardType.ToLower() == "master")
                {
                    if (_configuration.GetValue<bool>("Sandbox"))
                    {
                        data.Append("&testMode=EXTERNAL");
                    }
                    data.Append("&entityId=" + _configuration.GetValue<string>("VisaMasterEntityId"));
                    entityId = _configuration.GetValue<string>("VisaMasterEntityId");

                }
                else if (parameter.cardType.ToLower() == "mada")
                {
                    //data.Append("&paymentType=" + parameter.paymentType);
                    data.Append("&entityId=" + _configuration.GetValue<string>("MadaEntityId"));
                    entityId = _configuration.GetValue<string>("MadaEntityId");
                }

                else if (parameter.cardType.ToLower() == "applepay")
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
                AddLogs(data.ToString());
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
                    await _dataService.InsertPaymentTransactionStatus(checkoutSuccess, parameter.sessionId,
                        parameter, MerchantTransactionId, entityId);
                AddLogs(responseData.ToString());
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-payment-status")]
        public async Task<IActionResult> GetPaymentStatus(string cardType, string checkoutId)
        {
            try
            {
                string responseData;
                AddLogs("CardType:" + cardType + ";CheckoutID:" + checkoutId);
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

                string url = string.Format(_configuration.GetValue<string>("HyperPayURL") + "/checkouts/{0}/payment?", checkoutId) + data;
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
                AddLogs(responseData.ToString());
                PaymentStatusResult result = JsonConvert.DeserializeObject<PaymentStatusResult>(responseData);
                //if (result.result.code == "000.100.112" || result.result.code == "000.100.110" || result.result.code== "000.000.000")
                {
                    //Insert Status in DB
                    await _dataService.UpdatePaymentStatus(result);
                }
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("check-database-connection")]
        public IActionResult GetCourse()
        {
            try
            {
                var result = _context.Courses.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-transaction-id")]
        public IActionResult GetRadomString()
        {

            return Ok(Guid.NewGuid().ToString().Replace("-", ""));

        }


        [HttpPost("process-hyperpay-notification")]
        public async Task<IActionResult> ProcessNotification()
        {
            try
            {

                foreach (var h in HttpContext.Request.Headers)
                {
                    _logger.LogInformation(h.Key + ":" + h.Value);
                }
                string ivFromHttpHeader = HttpContext.Request.Headers["X-Initialization-Vector"].ToString();
                string authTagFromHttpHeader = HttpContext.Request.Headers["X-Authentication-Tag"].ToString();
                string httpBody = await GetBody(Request.Body);

                var model = JsonConvert.DeserializeObject<WebHookModel>(httpBody);
                _logger.LogInformation("payload: " + httpBody);

                // Convert data to process
                byte[] key = ToByteArray(_configuration.GetValue<string>("WebhookDecryptionKey"));
                byte[] iv = ToByteArray(ivFromHttpHeader);
                byte[] authTag = ToByteArray(authTagFromHttpHeader);
                byte[] encryptedText = ToByteArray(model.encryptedBody);
                byte[] cipherText = encryptedText.Concat(authTag).ToArray();

                // Prepare decryption
                GcmBlockCipher cipher = new GcmBlockCipher(new AesFastEngine());
                AeadParameters parameters = new AeadParameters(new KeyParameter(key), 128, iv);
                cipher.Init(false, parameters);


                // Decrypt
                var plainText = new byte[cipher.GetOutputSize(cipherText.Length)];
                var len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
                cipher.DoFinal(plainText, len);

                _logger.LogInformation("PlainTextBody: " + plainText);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return Ok();
            }

        }
        private byte[] ToByteArray(string HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }
        private async Task<string> GetBody(Stream streamBody)
        {
            // var bodyStream = new StreamReader(body);
            string body = await new StreamReader(streamBody).ReadToEndAsync();
            // _logger.LogInformation($"Body: {body}");
            // bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            // var bodyText = bodyStream.ReadToEnd();
            return body;
        }
        private void AddLogs(string logInfo)
        {
            if (_configuration.GetValue<string>("EnableLog") == "1")
                _logger.LogInformation(logInfo);
        }



    }
}
