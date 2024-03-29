using Go.SMSA.Services.Models;
using GO.SMSA.Service.Models.GoodReceipts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Go.SMSA.Services.Controllers
{
    public class SMSAController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SMSAController()
        {
            log.Info("SMSA API Started");
        }

        [Route("smsa/send-mail")]
        [HttpPost]
        public IHttpActionResult SendMail()
        {
            SMSAService service = new SMSAService();
            service.SendEmail("Test Mail");
            return Ok();
        }

        [Route("smsa/send-order-confirmation")]
        [HttpPost]
        public IHttpActionResult SMSAOrderConfirmation(SMSAOrderConfirmationModel sMSAOrderConfirmation)
        {
            try
            {
                SMSAService service = new SMSAService();
                var result = service.RemedyOrderConfirmation(sMSAOrderConfirmation).GetAwaiter().GetResult();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("smsa/send-goods-receipt")]
        [HttpPost]
        public IHttpActionResult SMSAGoodReceipt(GoodReceiptStatusModel receiptStatusModel)
        {
            try
            {
                SMSAService service = new SMSAService();
                StringBuilder messageBuilder = new StringBuilder();
                //messageBuilder.Append("<p>Dear Member</p>");
                //messageBuilder.Append("<p>Please find below goods receipt from SMSA</p>");
                //messageBuilder.Append("<br/>");
                //messageBuilder.Append("<table>");
                
                //messageBuilder.Append("<tr>");
                //messageBuilder.Append("<td>Bill Of Landing</td>");
                //messageBuilder.Append("<td>" + receiptStatusModel.billOfLanding + "</td>");
                //messageBuilder.Append("</tr>");

                //messageBuilder.Append("<tr>");
                //messageBuilder.Append("<td>Delivery Note</td>");
                //messageBuilder.Append("<td>" + receiptStatusModel.deliveryNote + "</td>");
                //messageBuilder.Append("</tr>");

                //messageBuilder.Append("<tr>");
                //messageBuilder.Append("<td>Document Note</td>");
                //messageBuilder.Append("<td>" + receiptStatusModel.documentDate + "</td>");
                //messageBuilder.Append("</tr>");

                //messageBuilder.Append("</table>");
                var result = service.SendEmail(JsonConvert.SerializeObject(receiptStatusModel));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("smsa/get-sim-details")]
        [HttpGet]
        public IHttpActionResult GetSIMDetails(string serialNo)
        {
            SMSAService service = new SMSAService();
            var result = service.GetSIMDetails(serialNo);
            return Ok(result);
        }
    }
}
