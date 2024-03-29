using Go.FTTH.OpenAccess.Service.Data;
using Go.FTTH.OpenAccess.Service.Data.Entities;
using Go.FTTH.OpenAccess.Service.Models;
using Go.FTTH.OpenAccess.Service.Models.ITC.CloseTicket;
using Go.FTTH.OpenAccess.Service.Models.ITC.NewTicket;
using Go.FTTH.OpenAccess.Service.Models.ITC.ONT_Status;
using Go.FTTH.OpenAccess.Service.Models.ITC.OpenTicket;
using Go.FTTH.OpenAccess.Service.Models.ITC.UpdateTicket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Services
{
    public interface IITCService
    {
        Task<dynamic> CreateITCTicket(NewITCTicketRequest ticketRequest);
        Task<dynamic> UpdateITCTicket(UpdateITCTicketRequest ticketRequest);

        Task<dynamic> ReopenITCTicket(ReOpenITCTicketRequest ticketRequest);
        Task<dynamic> CloseITCTicket(CloseITCTicketRequest ticketRequest);

        Task<dynamic> ITCONTInquiry(ONTRequest request);
    }
    public class ITCService : IITCService
    {
        private readonly IConfiguration configuration;
        private readonly string BaseURL;
        private readonly ILogger _logger;
        private readonly IDataService _dataService;
        public ITCService(IConfiguration _configuration, ILogger<ITCService> logger, IDataService dataService)
        {
            configuration = _configuration;
            BaseURL = configuration.GetValue<string>("ITCBaseURL");
            _logger = logger;
            _dataService = dataService;
        }
        public async Task<dynamic> CreateITCTicket(NewITCTicketRequest ticketRequest)
        {
            ITCTicketResponse responseResult = null;
            ticketRequest.NewCase.access_token = configuration.GetValue<string>("ITCBasicToken");
            ticketRequest.NewCase.content.txnNumber = ticketRequest.NewCase.content.seeker.ToCharArray()[0].ToString().ToUpper() + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string access_token = configuration.GetValue<string>("ITCBasicToken");
            RestClient client = new RestClient(BaseURL);
            RestRequest request = new RestRequest(RestSharp.Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(ticketRequest.NewCase), ParameterType.RequestBody);
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _logger.LogInformation(JsonConvert.SerializeObject(ticketRequest.NewCase));
            IRestResponse response = await client.ExecuteAsync(request);
            _logger.LogInformation(response.Content);
            _logger.LogInformation(response.ErrorMessage);
            if (response.IsSuccessful)
            {
                responseResult = JsonConvert.DeserializeObject<ITCTicketResponse>(response.Content.ToString());
                if (responseResult.result != null && responseResult.result.Request_ID != null)
                {
                    var ticket = new TicketMaster();
                    ticket.TICKET_ID = responseResult.result.Request_ID;
                    ticket.USERID = ticketRequest.UserID;
                    ticket.ACCOUNT_ID = ticketRequest.AccountId;
                    ticket.DESCR = ticketRequest.NewCase.content.Description;
                    ticket.OPERATOR_ID = ticketRequest.Operator;
                    ticket.CREATE_T = DateTime.Now;
                    ticket.STATUS = "New";
                    //ticket.SYS_ID = responseResult.result.sys_id;
                    ticket.MODIFIED_T = DateTime.Now;
                    ticket.NUMB = responseResult.result.Request_ID;
                    ticket.ORDERID = ticketRequest.OrderID;
                    var detail = ticketRequest.NewCase.content;
                    var ticket_detail = new TicketITCDetail()
                    {
                        TICKET_ID = responseResult.result.Request_ID,
                        SEEKER = detail.seeker,
                        TXNUMBER = detail.txnNumber,
                        OPERATION = detail.operation,
                        SEEKER_SERVICE_NO = detail.seekerServiceNo,
                        PROVIDER_SERVICE_NO = detail.providerServiceNo,
                        IMPACT = detail.Impact,
                        URGENCY = detail.Urgency,
                        FIRSTNAME = detail.FIRST_NAME,
                        LASTNAME = detail.LAST_NAME,
                        DESC = detail.Description,
                        DETAIL_DESC = detail.Detailed_Decription,
                        TICKET_STATUS = responseResult.result.Status,
                        CONTACT_PHONE = detail.Contact_Phone,
                        PROBLEM_CODE = detail.Problem_Code,
                        SEVERITY = detail.Severity,
                        SERVICE_IMPACTED = detail.Service_Impacted,
                        EXTERNAL_REFERENCE_NO = detail.External_Reference_number,
                        ACTUAL_INCIDENT_START_DATE = detail.Actual_Incident_Start_DateTime != null ? detail.Actual_Incident_Start_DateTime.Value.ToString() : ""
                    };
                    await _dataService.CreateNewTicketAsync(ticket);
                    await _dataService.AddITCTicketDetails(ticket_detail);
                    return responseResult;
                }
                else
                {
                    throw new Exception(response.Content);
                }
            }
            throw new Exception(response.Content);
        }

        public async Task<dynamic> ReopenITCTicket(ReOpenITCTicketRequest ticketRequest)
        {
            UpdateITCTicketResponse responseResult = null;
            ticketRequest.access_token = configuration.GetValue<string>("ITCBasicToken");
            ticketRequest.content.txnNumber = ticketRequest.content.seeker.ToCharArray()[0].ToString().ToUpper() + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            RestClient client = new RestClient(BaseURL);
            RestRequest request = new RestRequest(RestSharp.Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(ticketRequest), ParameterType.RequestBody);
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _logger.LogInformation(JsonConvert.SerializeObject(ticketRequest));
            IRestResponse response = await client.ExecuteAsync(request);
            _logger.LogInformation(response.Content);
            _logger.LogInformation(response.ErrorMessage);
            if (response.IsSuccessful)
            {
                responseResult = JsonConvert.DeserializeObject<UpdateITCTicketResponse>(response.Content.ToString());
                if (responseResult.result != null && responseResult.res_message == "Success")
                {
                    //Update Ticket 
                    await _dataService.UpdateReopenITCTicket(ticketRequest.content.providerTicketNo, ticketRequest.content.txnNumber, ticketRequest.content.seekerServiceNo, ticketRequest.content.providerServiceNo);
                    await _dataService.UpdateTicketMasterStatus(ticketRequest.content.providerTicketNo, "ReOpen");
                    return responseResult;
                }
                else
                {
                    throw new Exception(response.Content);
                }
            }
            throw new Exception(response.Content);
        }

        public async Task<dynamic> UpdateITCTicket(UpdateITCTicketRequest ticketRequest)
        {
            UpdateITCTicketResponse responseResult = null;
            ticketRequest.access_token = configuration.GetValue<string>("ITCBasicToken");
            ticketRequest.content.txnNumber = ticketRequest.content.seeker.ToCharArray()[0].ToString().ToUpper() + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            RestClient client = new RestClient(BaseURL);
            RestRequest request = new RestRequest(RestSharp.Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(ticketRequest), ParameterType.RequestBody);
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _logger.LogInformation(JsonConvert.SerializeObject(ticketRequest));
            IRestResponse response = await client.ExecuteAsync(request);
            _logger.LogInformation(response.Content);
            _logger.LogInformation(response.ErrorMessage);
            if (response.IsSuccessful)
            {
                responseResult = JsonConvert.DeserializeObject<UpdateITCTicketResponse>(response.Content.ToString());
                if (responseResult.result != null && responseResult.res_message == "Success")
                {
                    //Update Ticket 
                    await _dataService.UpdateITCTicket(ticketRequest.content.providerTicketNo, ticketRequest.content.txnNumber, ticketRequest.content.seekerServiceNo, ticketRequest.content.providerServiceNo, ticketRequest.content.Work_Info, ticketRequest.content.Work_Info_Summary);
                    return responseResult;
                }
                else
                {
                    throw new Exception(response.Content);
                }
            }
            throw new Exception(response.Content);
        }

        public async Task<dynamic> CloseITCTicket(CloseITCTicketRequest ticketRequest)
        {
            UpdateITCTicketResponse responseResult = null;
            ticketRequest.access_token = configuration.GetValue<string>("ITCBasicToken");
            ticketRequest.content.txnNumber = ticketRequest.content.seeker.ToCharArray()[0].ToString().ToUpper() + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            RestClient client = new RestClient(BaseURL);
            RestRequest request = new RestRequest(RestSharp.Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(ticketRequest), ParameterType.RequestBody);
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _logger.LogInformation(JsonConvert.SerializeObject(ticketRequest));
            IRestResponse response = await client.ExecuteAsync(request);
            _logger.LogInformation(response.Content);
            _logger.LogInformation(response.ErrorMessage);
            if (response.IsSuccessful)
            {
                responseResult = JsonConvert.DeserializeObject<UpdateITCTicketResponse>(response.Content.ToString());
                if (responseResult.result != null && responseResult.res_message == "Success")
                {
                    //Update Ticket 
                    await _dataService.UpdateReopenITCTicket(ticketRequest.content.providerTicketNo, ticketRequest.content.txnNumber, ticketRequest.content.seekerServiceNo, ticketRequest.content.providerServiceNo);
                    await _dataService.UpdateTicketMasterStatus(ticketRequest.content.providerTicketNo, "Closed");
                    return responseResult;
                }
                else
                {
                    throw new Exception(response.Content);
                }
            }
            throw new Exception(response.Content);
        }

        public async Task<dynamic> ITCONTInquiry(ONTRequest ITCrequest)
        {
            ONTITCStatusResponse responseResult = null;
            var ITCONTRequest = new ONTStatusRequest();
            ITCONTRequest.access_token = configuration.GetValue<string>("ITCBasicToken");
            ITCONTRequest.content = new ONTITCContent();
            ITCONTRequest.content.txnNumber = "A" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            ITCONTRequest.content.seeker = ITCrequest.Seeker;
            ITCONTRequest.content.providerServiceNo = ITCrequest.ServiceAccountNumber;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            RestClient client = new RestClient(BaseURL);
            RestRequest request = new RestRequest(RestSharp.Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(ITCONTRequest), ParameterType.RequestBody);
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _logger.LogInformation(JsonConvert.SerializeObject(ITCONTRequest));
            IRestResponse response = await client.ExecuteAsync(request);
            _logger.LogInformation(response.Content);
            _logger.LogInformation(response.ErrorMessage);
            if (response.IsSuccessful)
            {
                responseResult = JsonConvert.DeserializeObject<ONTITCStatusResponse>(response.Content.ToString());
                if (responseResult.result != null && responseResult.res_message == "Success")
                {
                    //Update Ticket 
                    int ONTID = await _dataService.InsertONTHealthCheck(ITCrequest.ServiceAccountNumber, ITCrequest.Operator, ITCrequest.UserId, ITCrequest.AccountID);
                    var ONTDetail = new ONTItcDetail()
                    {
                        ONT_ID = ONTID,
                        TXN_NUMBER = ITCONTRequest.content.txnNumber,
                        MODIFT_DT = DateTime.Now,
                        RUN_STATUS= responseResult.result.runStatus,
                        LAST_UP_TIME= responseResult.result.lastUptime,
                        LAST_DOWN_TIME= responseResult.result.lastDowntime,
                        gponRx= responseResult.result.gponRx,
                        gponTx= responseResult.result.gponTx,
                        ontTx= responseResult.result.ontTx,
                        ontRx= responseResult.result.ontRx
                    };
                    _logger.LogInformation("Inserted all details");
                    await _dataService.InsertONTITCDetails(ONTDetail);
                    
                    return responseResult;
                }
                else
                {
                    throw new Exception(response.Content);
                }
            }
            throw new Exception(response.Content);

        }
    }
}
