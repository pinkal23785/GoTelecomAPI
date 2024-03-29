using Go.FTTH.OpenAccess.Service.Models.Dawiyat;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Go.FTTH.OpenAccess.Service.Models.Dawiyat.CreateCase;
using Go.FTTH.OpenAccess.Service.Models.Dawiyat.ResourceFeasibility;
using Microsoft.Extensions.Logging;
using System.Web;
using Go.FTTH.OpenAccess.Service.Data;
using Go.FTTH.OpenAccess.Service.Data.Entities;
using Go.FTTH.OpenAccess.Service.Models.Dawiyat.UpdateCaseStatus;
using Go.FTTH.OpenAccess.Service.Models.Dawiyat.Get;
using Go.FTTH.OpenAccess.Service.Models.ITC.ONT_Status;
using Go.FTTH.OpenAccess.Service.Models.Dawiyat.ONTStatus;
using Go.FTTH.OpenAccess.Service.Models;
using System.Net;

namespace Go.FTTH.OpenAccess.Service.Services
{

    public interface IDawiyatService
    {
        Task<DawiyatAuthResponseModel> AuthDawiyatClient();
        Task<object> CreateCase(NewCaseRequestModel parameter);
        Task<FeasibilityResponse> CustomerFeasibility(string obdId);
        Task<dynamic> GetDawiyatCaseDetail(string caseNumber);

        Task<dynamic> UpdateDawiyatTicketStatus(UpdateDawiyatCaseStatusRequest parameter);

        Task<DawiyatTicketResponseModel> GetDwaiyatTicketDetail(string caseNumber);

        Task<dynamic> GetONTInquiry(ONTRequest request);
    }
    public class DawiyatService : IDawiyatService
    {
        string BaseAuthURL = string.Empty;
        string BaseURL = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ILogger _logger;
        private readonly IDataService _dataService;
        public DawiyatService(IConfiguration _configuration, ILogger<DawiyatService> logger, IDataService dataService)
        {
            configuration = _configuration;
            _logger = logger;
            BaseAuthURL = configuration.GetValue<string>("DawiyatBaseAuthURL");
            BaseURL = configuration.GetValue<string>("DawiyatBaseURL");
            _dataService = dataService;
        }
        public async Task<DawiyatAuthResponseModel> AuthDawiyatClient()
        {
            DawiyatAuthResponseModel responseResult = null;
            try
            {
                DawiyatAuthRequestModel dawiyatAuthRequestModel = new DawiyatAuthRequestModel();

                string AuthURL = configuration.GetValue<string>("DawiyatAuthURL");
                _logger.LogInformation(BaseAuthURL + AuthURL);
                RestClient client = new RestClient(BaseAuthURL + AuthURL);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                RestRequest request = new RestRequest(RestSharp.Method.POST);
                //request.Resource = AuthURL;

                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                //request.AddParameter("grant_type", "password");
                //request.AddParameter("client_id", configuration.GetValue<string>("DawiyatClientID"));
                //request.AddParameter("username", configuration.GetValue<string>("DawiyatUserName"));
                //request.AddParameter("password", configuration.GetValue<string>("DawiyatPassword"));
                string parameter = string.Format("grant_type=password&client_id={0}&username={1}&password={2}", configuration.GetValue<string>("DawiyatClientID"),
                 configuration.GetValue<string>("DawiyatUserName"), configuration.GetValue<string>("DawiyatPassword"));
                request.AddParameter("application/x-www-form-urlencoded", parameter, ParameterType.RequestBody);
                //request.AddParameter("application/x-www-form-urlencoded", "grant_type", "password", ParameterType.RequestBody);
                //request.AddParameter("application/x-www-form-urlencoded", "client_id", configuration.GetValue<string>("DawiyatClientID"), ParameterType.RequestBody);
                //request.AddParameter("application/x-www-form-urlencoded", "username", configuration.GetValue<string>("DawiyatUserName"), ParameterType.RequestBody);
                //request.AddParameter("application/x-www-form-urlencoded", "password", configuration.GetValue<string>("DawiyatPassword"), ParameterType.RequestBody);


                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                _logger.LogInformation("Dawiyat Auth Executed");
                IRestResponse response = await client.ExecuteAsync(request);
                _logger.LogInformation(response.Content.ToString());
               // _logger.LogInformation(response.ErrorMessage.ToString());
                if (response.IsSuccessful)
                {
                    responseResult = JsonConvert.DeserializeObject<DawiyatAuthResponseModel>(response.Content);
                }

               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return responseResult;
        }

        public async Task<object> CreateCase(NewCaseRequestModel parameter)
        {
            var dawiyatClient = await AuthDawiyatClient();
            NewCaseResponse responseResult = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            if (dawiyatClient != null)
            {
                parameter.NewCase.u_trouble_ticket_number = "INC" + DateTime.Now.ToString("ddmmyyhhmmss");
                string caseURL = "/esb/csm/rest/v2/createcase";
                RestClient client = new RestClient(BaseURL + caseURL);
                RestRequest request = new RestRequest(RestSharp.Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("CLIENTCODE", configuration.GetValue<string>("DawiyatClientID"));
                //request.AddHeader("CLIENTREFERENCE", configuration.GetValue<string>("ClientReference"));
                request.AddHeader("AUTHORIZATION", "Bearer " + dawiyatClient.access_token);
                request.AddJsonBody(parameter.NewCase);
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                _logger.LogInformation(JsonConvert.SerializeObject(parameter.NewCase));
                IRestResponse response = await client.ExecuteAsync(request);
                _logger.LogInformation("After Dawiyet API called");
                _logger.LogInformation(response.Content);
                if (response.IsSuccessful)
                {
                    _logger.LogInformation("Sucess response");
                    responseResult = JsonConvert.DeserializeObject<NewCaseResponse>(response.Content.ToString());

                    if (responseResult.result.number != null)
                    {
                        var ticket = new TicketMaster();
                        ticket.TICKET_ID = responseResult.result.u_trouble_ticket_number;
                        ticket.USERID = parameter.UserID;
                        ticket.ACCOUNT_ID = parameter.AccountId;
                        ticket.DESCR = parameter.NewCase.description;
                        ticket.OPERATOR_ID = parameter.Operator;
                        ticket.CREATE_T = DateTime.Now;
                        ticket.STATUS = "New";
                        ticket.SYS_ID = responseResult.result.sys_id;
                        ticket.MODIFIED_T = DateTime.Now;
                        ticket.NUMB = responseResult.result.number;
                        ticket.ORDERID = parameter.OrderID;
                        var ticket_detail = new TicketDawiyatDetail()
                        {
                            BUSINESS_SERVICE = parameter.NewCase.business_service,
                            PRODUCT = parameter.NewCase.u_product,
                            CATEGORY = parameter.NewCase.category,
                            SUBCATEGORY = parameter.NewCase.subcategory,
                            SUBCATEGORY_L2 = parameter.NewCase.subcategory_L2,
                            CUSTOMER_NAME = parameter.NewCase.u_customer_name,
                            CUSTOMER_ORDER_NUMBER = parameter.NewCase.u_customer_order_number,
                            CUSTOMER_CONTACT_NUMBER = parameter.NewCase.u_customer_contact_number,
                            OBDID = parameter.NewCase.u_odb_id,
                            SHORT_DESC = parameter.NewCase.short_description,
                            DESC = parameter.NewCase.description,
                            CIRCUIT_ID = parameter.NewCase.u_circuit_id,
                            REGION = parameter.NewCase.u_region,
                            CITY = parameter.NewCase.u_city,
                            DISTRICT = parameter.NewCase.u_district,
                            DAWIYAT_BUILDING_ID = parameter.NewCase.u_dawiyat_building_id,
                            SAUDI_NATION_ADDRESS = parameter.NewCase.u_saudi_national_address,
                            OLO_CUSTOMER_ID = parameter.NewCase.olo_customer_id,
                            DAW_SERVICE_ID = parameter.NewCase.daw_service_id,
                            ACCOUNT = parameter.NewCase.account,
                            CONTACT = parameter.NewCase.contact,
                            TICKET_ID = parameter.NewCase.u_trouble_ticket_number
                        };
                        await _dataService.CreateNewTicketAsync(ticket);
                        await _dataService.AddDawiyatTicketDetails(ticket_detail);

                        return responseResult;
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }
                }

                throw new Exception(response.Content);
            }
            throw new Exception("Dwaiyat Authentication Failed");
        }
        public async Task<FeasibilityResponse> CustomerFeasibility(string obdId)
        {
            var dawiyatClient = await AuthDawiyatClient();
            FeasibilityResponse responseResult = null;
            if (dawiyatClient != null)
            {
                string caseURL = "/esb/om/rest/v1/resourcefeasiblity?odbid=" + obdId;
                _logger.LogInformation(BaseURL + caseURL);
                RestClient client = new RestClient(BaseURL + caseURL);
                RestRequest request = new RestRequest(RestSharp.Method.GET);


                request.AddHeader("authorization", "Bearer " + dawiyatClient.access_token);
                request.AddHeader("clientcode", configuration.GetValue<string>("DawiyatClientID"));
                request.AddHeader("content-type", "application/json");
                _logger.LogInformation(configuration.GetValue<string>("DawiyatClientID"));
                IRestResponse response = await client.ExecuteAsync(request);
                _logger.LogInformation(response.Content);
                if (response.IsSuccessful)
                {
                    responseResult = JsonConvert.DeserializeObject<FeasibilityResponse>(response.Content);
                }
            }
            return responseResult;
        }
        public async Task<dynamic> GetDawiyatCaseDetail(string caseNumber)
        {
            //var dawiyatClient = await AuthDawiyatClient();

            //if (dawiyatClient != null)
            //{
            //    string caseURL = "/esb/csm/rest/v1/querycase?number=" + caseNumber;
            //    RestClient client = new RestClient(BaseURL + caseURL);
            //    var request = new RestRequest(Method.GET);
            //    request.AddHeader("CLIENTCODE", configuration.GetValue<string>("DawiyatClientID"));
            //    request.AddHeader("AUTHORIZATION", "Bearer " + dawiyatClient.access_token);
            //    IRestResponse response = client.Execute(request);
            //    return response.Content;
            //}
            //throw new Exception("Dwaiyat Authentication Failed");
            DawiyatTicketResponseModel responseResult = new DawiyatTicketResponseModel();
            responseResult.result = await _dataService.GetTicketDawiyatDetail(caseNumber);
            return responseResult;
        }

        public async Task<dynamic> UpdateDawiyatTicketStatus(UpdateDawiyatCaseStatusRequest parameter)
        {
            var dawiyatClient = await AuthDawiyatClient();
            UpdateDawiyatCaseStatusRespose responseResult = null;
            if (dawiyatClient != null)
            {

                string caseURL = "/esb/csm/rest/v1/updatecase?u_trouble_ticket_number=" + parameter.u_trouble_ticket_number;
                RestClient client = new RestClient(BaseURL + caseURL);
                RestRequest request = new RestRequest(RestSharp.Method.PUT);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("CLIENTCODE", configuration.GetValue<string>("DawiyatClientID"));
                request.AddHeader("AUTHORIZATION", "Bearer " + dawiyatClient.access_token);
                request.AddJsonBody(parameter);
                _logger.LogInformation(JsonConvert.SerializeObject(parameter));
                IRestResponse response = await client.ExecuteAsync(request);
                _logger.LogInformation(response.Content);
                if (response.IsSuccessful)
                {

                    responseResult = JsonConvert.DeserializeObject<UpdateDawiyatCaseStatusRespose>(response.Content.ToString());

                    if (!string.IsNullOrEmpty(responseResult.result.case_number))
                    {
                        await _dataService.UpdateDawiyatTicketStatus(responseResult.result.case_number, parameter.state, parameter.comments);

                        return responseResult;
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }
                }

                throw new Exception(response.Content);
            }
            throw new Exception("Dwaiyat Authentication Failed");
        }

        public Task<DawiyatTicketResponseModel> GetDwaiyatTicketDetail(string caseNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetONTInquiry(ONTRequest parameter)
        {
            var dawiyatClient = await AuthDawiyatClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            if (dawiyatClient != null)
            {
                ONTDawiyatStatusRequest statusReq = new ONTDawiyatStatusRequest();
                if (!string.IsNullOrEmpty(parameter.ServiceAccountNumber))
                    statusReq.serviceInstanceID = parameter.ServiceAccountNumber;
                string ONTURL = "/esb/acs/rest/v2/onttest";
                RestClient client = new RestClient(BaseURL + ONTURL);
                RestRequest request = new RestRequest(RestSharp.Method.POST);



                request.AddHeader("CLIENTCODE", configuration.GetValue<string>("DawiyatClientID"));
                //request.AddHeader("CLIENTREFERENCE", configuration.GetValue<string>("ClientReference"));
                request.AddHeader("AUTHORIZATION", "Bearer " + dawiyatClient.access_token);
                request.AddJsonBody(statusReq);
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                _logger.LogInformation(JsonConvert.SerializeObject(statusReq));
                IRestResponse response = await client.ExecuteAsync(request);
                _logger.LogInformation("Response: "+response.Content);
                ONTDawiyatStatusResponse responseResult = null;
                if (response.IsSuccessful)
                {
                    responseResult = JsonConvert.DeserializeObject<ONTDawiyatStatusResponse>(response.Content.ToString());
                    if (responseResult.errCode == 200)
                    {
                        try
                        {
                            int ONTID = await _dataService.InsertONTHealthCheck(parameter.ServiceAccountNumber, parameter.Operator, parameter.UserId, parameter.AccountID);
                            var ONTDetail = new ONTDawiyatDetail()
                            {
                                ONT_ID = ONTID,
                                THROUGHPUT = responseResult.body.throughput,
                                MODIFY_DT = DateTime.Now,
                                LATENCY = responseResult.body.latency.ToString(),
                                Rx = responseResult.body.Rx,
                                Tx = responseResult.body.Tx,
                                UploadSpeed = responseResult.body.UploadSpeed,
                                DownloadSpeed = responseResult.body.DownloadSpeed,
                                ONTStatus = responseResult.body.ONTStatus
                            };
                            _logger.LogInformation("Inserted all details");
                            await _dataService.InsertONTDawiyatDetails(ONTDetail);
                        }
                        catch(Exception ex)
                        {
                            _logger.LogError(ex.Message + " " + ex.InnerException);
                        }
                        return responseResult;
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }
                }
                throw new Exception(response.Content);
            }
            else
                throw new Exception("Dwaiyat Authentication Failed");
        }
    }
}
