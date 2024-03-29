using Go.SMSA.Services.EmailService;
using Go.SMSA.Services.Models;
//using Go.SMSA.Services.OrderConfirmationBehavior;
using Go.SMSA.Services.ResponseModels;
using GO.SMSA.Service;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Go.SMSA.Services
{
    public class SMSAService
    {

        private readonly string SMSABaseURL;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SMSAService()
        {
            SMSABaseURL = ConfigurationManager.AppSettings["SMSABaseURL"];

        }
        public async Task<int> CreateOrder(SMSAOrderRequestModel requestModel)
        {
            try
            {
                log.Info("Remedy Order Create Request Received ");
                log.Info("Request Payload:" + JsonConvert.SerializeObject(requestModel));

                var auth = await GetSMSAAuthentication();

                log.Info("Auth Response:" + JsonConvert.SerializeObject(auth));

                OrderResponseModel orderResponseModel = new OrderResponseModel();
                if (auth != null)
                {
                    var client = new RestSharp.RestClient(SMSABaseURL);
                    var request = new RestRequest("orders", Method.POST);
                    //request.AddHeader("cache-control", "no-cache");
                    //request.AddHeader("content-type", "application/json");
                    request.AddHeader("authorization", "Bearer " + auth.access_token);
                    request.RequestFormat = DataFormat.Json;
                    request.AddBody(requestModel);
                    //request.AddParameter("application/json", JsonConvert.SerializeObject(requestModel), ParameterType.RequestBody);
                    var response = client.Execute(request);
                    log.Info("Response:" + response.Content);
                    log.Info("Status Code:" + response.StatusCode);
                    log.Info("Status Description:" + response.StatusDescription);
                    File.AppendAllText("D:\\Applications\\SMSA\\SMSALog.txt", response.Content + Environment.NewLine);
                    orderResponseModel = JsonConvert.DeserializeObject<OrderResponseModel>(response.Content);
                    
                    if (response.StatusCode != System.Net.HttpStatusCode.Created && 
                        response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return 0;
                    }

                    return 1;

                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + ex.InnerException);
                throw ex;
            }
        }

        public async Task<AuthResponseModel> GetSMSAAuthentication()
        {
            try
            {
                string basicToken = ConfigurationManager.AppSettings["BasicToken"];
                string grantType = ConfigurationManager.AppSettings["grant_type"];
                string tpl = ConfigurationManager.AppSettings["tpl"];
                string userLoginId = ConfigurationManager.AppSettings["user_login_id"];
                var client = new RestClient(SMSABaseURL);
                var request = new RestRequest("AuthServer/api/Token", Method.POST);

                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Basic " + basicToken);
                var authModel = new AuthenticateModel()
                {
                    grant_type = grantType,
                    tpl = "{" + tpl + "}",
                    user_login_id = userLoginId
                };

                request.RequestFormat = DataFormat.Json;
                request.AddBody(authModel);

                //request.AddParameter("application/json", JsonConvert.SerializeObject(authModel), ParameterType.RequestBody);

                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    AuthResponseModel AuthModel = JsonConvert.DeserializeObject<AuthResponseModel>(response.Content);
                    return AuthModel;
                }
                throw new Exception("An error occured: Status code: " + response.StatusCode, response.ErrorException);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int SendEmail(string body)
        {
            try
            {
                log.Info("Email Request Received");
                EmailService.SendMail sendMail = new EmailService.SendMail();
                sendMail.subject = "SMSA Good Receipt";

                var toEmail = ConfigurationManager.AppSettings["GoodReceiptEmail"].ToString();
                //TO Address
                List<Address> arrayToAddress = new List<Address>();
                Address toAddress = new Address();
                toAddress.mailId = toEmail;
                toAddress.name = "logistic";
                arrayToAddress.Add(toAddress);
                sendMail.to = arrayToAddress.ToArray();

                //From Address
                Address fromAddress = new Address();
                fromAddress.mailId = "care@go.com.sa";
                fromAddress.name = "care@go.co.sa";
                sendMail.from = fromAddress;

                //Body
                EmailService.Message m = new EmailService.Message();


                m.messageHTML = body;

                sendMail.message = m;

                ExecutePortTypeClient client = new ExecutePortTypeClient();

                client.Endpoint.EndpointBehaviors.Add(new WsSecurityEndpointBehavior("EAIWSUser", "wsuser", "UsernameToken-10461947"));

                log.Info("Email Request Payload:" + JsonConvert.SerializeObject(sendMail));
                var output = client.oprEmailNotificationAsync(sendMail).Result;
                log.Info("Email Response:" + JsonConvert.SerializeObject(output));

                return output.EmailAck.status;


            }
            catch (Exception ex)
            {
                log.Error(ex.Message + ex.InnerException);
                throw ex;
            }
        }

        public async Task<OrderConfirmationResponseModel> RemedyOrderConfirmation(SMSAOrderConfirmationModel sMSAOrderConfirmation)
        {
            try
            {

                ATB_Springbooot_WSService client = new ATB_Springbooot_WSService();
                string ErrorMessage;
                string InstallNo;
                AuthenticationInfo info = new AuthenticationInfo()
                {
                    userName = "webuser",
                    password = "atheeb123"
                };
                client.AuthenticationInfoValue = info;
                string serial = "";
                string SIMICCID = "";
                if(sMSAOrderConfirmation.items.Count>0)
                {
                    if(sMSAOrderConfirmation.items[0].material.ToUpper()== "LTE CPE")
                    {
                        serial = sMSAOrderConfirmation.items[0].serial;
                    }
                    else if (sMSAOrderConfirmation.items[0].material.ToUpper() == "LTE_SIM")
                    {
                        SIMICCID = sMSAOrderConfirmation.items[0].serial;
                    }
                }
                if(sMSAOrderConfirmation.items.Count > 1)
                {
                    if (sMSAOrderConfirmation.items[0].material.ToUpper() == "LTE CPE")
                    {
                        serial = sMSAOrderConfirmation.items[0].serial;
                    }
                    else if (sMSAOrderConfirmation.items[0].material.ToUpper() == "LTE_SIM")
                    {
                        SIMICCID = sMSAOrderConfirmation.items[0].serial;
                    }


                    if (sMSAOrderConfirmation.items[1].material.ToUpper() == "LTE CPE")
                    {
                        serial = sMSAOrderConfirmation.items[1].serial;
                    }
                    else if (sMSAOrderConfirmation.items[1].material.ToUpper() == "LTE_SIM")
                    {
                        SIMICCID = sMSAOrderConfirmation.items[1].serial;
                    }
                }

                var result = client.NotifyShipmentOrderConfirmation(sMSAOrderConfirmation.referenceNumber,serial,
                    SIMICCID,
                    sMSAOrderConfirmation.trackingNumber, sMSAOrderConfirmation.transactionNumber, out ErrorMessage, out InstallNo);
                var orderConfirmResponse = new OrderConfirmationResponseModel()
                {
                    ErrorMessage = ErrorMessage,
                    InstallNo = InstallNo
                };
                return orderConfirmResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<SIMDetailsResponseModel> GetSIMDetails(string serialNo)
        {
            CADBEntities entities = new CADBEntities();
            try
            {
                ObjectParameter parameter_v_IMSI = new ObjectParameter("v_IMSI", typeof(string));
                ObjectParameter parameter_v_MSISDN = new ObjectParameter("v_MSISDN", typeof(string));
                ObjectParameter parameter_v_ICCID = new ObjectParameter("v_ICCID", typeof(string));
                ObjectParameter parameter_v_MACID = new ObjectParameter("v_MACID", typeof(string));
                ObjectParameter parameter_v_CPE_MODEL = new ObjectParameter("v_CPE_MODEL", typeof(string));
                ObjectParameter parameter_v_STATUS = new ObjectParameter("v_STATUS", typeof(string));

                int result = entities.EQUIPMENT_SIM_DETAILS(serialNo, "SIM", parameter_v_IMSI, parameter_v_MSISDN, parameter_v_ICCID, parameter_v_MACID, parameter_v_CPE_MODEL,
                    parameter_v_STATUS);
                var resultData = new SIMDetailsResponseModel()
                {
                    IMSI = Convert.ToString(parameter_v_IMSI.Value),
                    MSISDN = Convert.ToString(parameter_v_MSISDN.Value),
                    MACID = Convert.ToString(parameter_v_MACID.Value),
                    CPE_MODEL = Convert.ToString(parameter_v_CPE_MODEL.Value),
                    STATUS = parameter_v_STATUS.Value != null ? (parameter_v_STATUS.Value.ToString().ToLower() == "success" ? 1 : 0) : 0,
                    ICCID = Convert.ToString(parameter_v_ICCID.Value)
                };
                return resultData;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " " + ex.InnerException);
                throw ex;

            }


        }
    }


}