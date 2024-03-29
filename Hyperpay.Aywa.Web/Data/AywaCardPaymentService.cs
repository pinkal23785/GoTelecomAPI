using Hyperpay.Aywa.Web.SADAD;
using Hyperpay.Aywa.Web.Webservice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hyperpay.Aywa.Web.Data
{
    [ServiceContract]
    public interface IAywaCardPaymentService
    {
        [OperationContract]
        Task<getAywaCardPaymentResponse> getAywaCardPayment(getAywaCardPaymentRequest request);
    }
    public class AywaCardPaymentService : IAywaCardPaymentService
    {
        private readonly ISADADService _sadadService;
        private readonly ILoggerService _loggerService;
        public AywaCardPaymentService(ISADADService sadadService, ILoggerService loggerService)
        {
            _sadadService = sadadService;
            _loggerService = loggerService;
        }
        public async Task<getAywaCardPaymentResponse> getAywaCardPayment(getAywaCardPaymentRequest request)
        {
            getAywaCardPaymentResponse response = null;
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(getAywaCardPaymentRequest));
                StringBuilder sbData = new StringBuilder(); ;
                StringWriter swWriter = new StringWriter(sbData);
                ser.Serialize(swWriter, request);
                await _loggerService.AddLog(sbData.ToString());

                var parameter = new AywaCardPaymentReqModel()
                {
                    accountId = request.billingAccount,
                    subscriberId = request.billNumber,
                    orderID = request.orderID,
                    paymentMethod = request.paymentMethod,
                    paymentRef = request.paymentRef,
                    totalPaidAmount = request.totalPaidAmount,
                    lang=request.mobiledetails.language
                };
                 await _sadadService.ApplySADADPayment(parameter);

                response = new getAywaCardPaymentResponse()
                {
                    responseId = "200",
                    response = "SADAD payment apply successfully",
                    responseDesc = "SADAD payment apply successfully"
                };
                return response;
            }
            catch (Exception ex)
            {
                 await _loggerService.AddLog(ex.Message);
                response = new getAywaCardPaymentResponse()
                {
                    responseId = "500",
                    response = "Internal Server Error",
                    responseDesc = ex.Message
                };
            }
            return response;
        }
    }
}
