using Go.FTTH.OpenAccess.Service.Data;
using Go.FTTH.OpenAccess.Service.Data.Entities;
using Go.FTTH.OpenAccess.Service.Models;
using Go.FTTH.OpenAccess.Service.Models.Mobily.CloseSR;
using Go.FTTH.OpenAccess.Service.Models.Mobily.NewSR;
using Go.FTTH.OpenAccess.Service.Models.Mobily.ONT;
using Go.FTTH.OpenAccess.Service.Models.Mobily.ReOpenSR;
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
    public interface IMobilyService
    {
        Task<NewServiceResponse> CreateNewSR(NewServiceRequestModel parameter);
        Task<ReopenSRServiceResponse> ReopenSR(ReopenSRServiceRequest parameter);
        Task<dynamic> GetMobilyTicketDetails(string caseNumber);
        Task<CloseSRResponse> CloseMobilyTicket(CloseSRRequest parameter);
        Task<ONTMobilyResponse> GetMobilyONTStatus(ONTRequest parameter);
        Task<ONTLatencyMobilyResponse> GetONTLatencyMobily(ONTMobilyRequest parameter);
    }
    public class MobiliyService : IMobilyService
    {
        string BaseURL = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ILogger _logger;
        private readonly IDataService _dataService;
        public MobiliyService(IConfiguration _configuration, ILogger<DawiyatService> logger, IDataService dataService)
        {
            configuration = _configuration;
            _logger = logger;
            BaseURL = configuration.GetValue<string>("MobilyBaseURL");
            _dataService = dataService;
        }
        public async Task<NewServiceResponse> CreateNewSR(NewServiceRequestModel parameter)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                NewServiceResponse responseResult = null;
                parameter.ServiceRequest.TransactionID = "INC" + DateTime.Now.ToString("ddmmyyhhmmss");
                RestClient client = new RestClient(BaseURL);
                RestRequest request = new RestRequest(RestSharp.Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", configuration.GetValue<string>("MobilyBasicToken"));
                //request.AddJsonBody(parameter.ServiceRequest);
                request.AddParameter("application/json", JsonConvert.SerializeObject(parameter.ServiceRequest), ParameterType.RequestBody);
                _logger.LogInformation(JsonConvert.SerializeObject(parameter.ServiceRequest));
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = await client.ExecuteAsync(request);
                if (response == null)
                    _logger.LogInformation("No response");
                _logger.LogInformation(response.ResponseStatus.ToString());
                _logger.LogInformation(response.ErrorMessage);
                if (response.IsSuccessful)
                {

                    responseResult = JsonConvert.DeserializeObject<NewServiceResponse>(response.Content.ToString());
                    if (!string.IsNullOrEmpty(responseResult.SRNumber))
                    {
                        var ticket = new TicketMaster();
                        ticket.TICKET_ID = parameter.ServiceRequest.TransactionID;
                        ticket.USERID = parameter.UserID;
                        ticket.ACCOUNT_ID = parameter.AccountId;
                        ticket.DESCR = parameter.ServiceRequest.Description;
                        ticket.OPERATOR_ID = parameter.Operator;
                        ticket.CREATE_T = DateTime.Now;
                        ticket.STATUS = parameter.ServiceRequest.Status;
                        //ticket.SYS_ID = responseResult.result.sys_id;
                        ticket.MODIFIED_T = DateTime.Now;
                        ticket.NUMB = responseResult.SRNumber;
                        ticket.ORDERID = parameter.OrderID;
                        await _dataService.CreateNewTicketAsync(ticket);

                        var ticketDetail = new TicketMobilyDetail()
                        {
                            TICKET_NO = parameter.ServiceRequest.TransactionID,
                            SERVICEACCNUM = parameter.ServiceRequest.ServiceAccNum,
                            CUSTOMER_TYPE = parameter.ServiceRequest.CustomerType,
                            SRTYPE = parameter.ServiceRequest.SRType,
                            SRAREA = parameter.ServiceRequest.SRArea,
                            SR_SUB_AREA = parameter.ServiceRequest.SRSubArea,
                            CHANNEL = parameter.ServiceRequest.Channel,
                            DESC = parameter.ServiceRequest.Description,
                            SUB_STATUS = parameter.ServiceRequest.SubStatus,
                            SERVICE_OWNER_NAME = parameter.ServiceRequest.ServiceOwnerName,
                            SERVICE_OWNER_NUMBER = parameter.ServiceRequest.ServiceOwnerNumber,
                            FLEX1 = parameter.ServiceRequest.Flex1,
                            FLEX2 = parameter.ServiceRequest.Flex2
                        };
                        await _dataService.AddMobilyTicketDetails(ticketDetail);
                        return responseResult;
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }
                }

                throw new Exception(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
                throw ex;
            }
        }

        public async Task<ReopenSRServiceResponse> ReopenSR(ReopenSRServiceRequest parameter)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ReopenSRServiceResponse responseResult = null;
                //parameter.TransactionID = "INC" + DateTime.Now.ToString("ddmmyyhhmmss");
                RestClient client = new RestClient(BaseURL);
                RestRequest request = new RestRequest(RestSharp.Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", configuration.GetValue<string>("MobilyBasicToken"));
                //request.AddJsonBody(parameter.ServiceRequest);
                request.AddParameter("application/json", JsonConvert.SerializeObject(parameter), ParameterType.RequestBody);
                _logger.LogInformation(JsonConvert.SerializeObject(parameter));
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = await client.ExecuteAsync(request);
                if (response == null)
                    _logger.LogInformation("No response");
                _logger.LogInformation(response.Content.ToString());
                _logger.LogInformation(response.ErrorMessage);
                if (response.IsSuccessful)
                {

                    responseResult = JsonConvert.DeserializeObject<ReopenSRServiceResponse>(response.Content.ToString());
                    if (!string.IsNullOrEmpty(responseResult.ErrorCode) && responseResult.ErrorCode == "0000")
                    {
                        //Update ticket details
                        await _dataService.UpdateReopenMobilyTicket(parameter.SRNumber, "Reopened");
                        return responseResult;
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }
                }

                throw new Exception(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
                throw ex;
            }
        }

        public async Task<CloseSRResponse> CloseMobilyTicket(CloseSRRequest parameter)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                CloseSRResponse responseResult = null;
                parameter.TransactionNo = "INC" + DateTime.Now.ToString("ddmmyyhhmmss");
                RestClient client = new RestClient(BaseURL);
                RestRequest request = new RestRequest(RestSharp.Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", configuration.GetValue<string>("MobilyBasicToken"));
                //request.AddJsonBody(parameter);
                request.AddParameter("application/json", JsonConvert.SerializeObject(parameter), ParameterType.RequestBody);
                _logger.LogInformation(JsonConvert.SerializeObject(parameter));
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = await client.ExecuteAsync(request);
                if (response == null)
                    _logger.LogInformation("No response");
                _logger.LogInformation(response.Content.ToString());

                if (response.IsSuccessful)
                {

                    responseResult = JsonConvert.DeserializeObject<CloseSRResponse>(response.Content.ToString());
                    if (!string.IsNullOrEmpty(responseResult.ErrorMessage) && responseResult.ErrorMessage == "Success")
                    {
                        //Update ticket details
                        await _dataService.UpdateReopenMobilyTicket(parameter.SRNumber, "Closed");
                        return responseResult;
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }
                }

                throw new Exception(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
                throw ex;
            }
        }

        public async Task<dynamic> GetMobilyTicketDetails(string caseNumber)
        {
            var result = await _dataService.GetTicketMobilyDetail(caseNumber);
            return result;
        }
        public async Task<ONTMobilyResponse> GetMobilyONTStatus(ONTRequest parameter)
        {
            try
            {
                var MobilyONTRequest = new ONTMobilyRequest();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ONTMobilyResponse responseResult = null;
                MobilyONTRequest.TransactionNo = "1_" + DateTime.Now.ToString("ddmmyyhhmmss");
                MobilyONTRequest.ServiceAccNum = parameter.ServiceAccountNumber;
                MobilyONTRequest.Flex1 = parameter.Flex1;
                MobilyONTRequest.Flex2 = parameter.Flex2;

                RestClient client = new RestClient(BaseURL);
                RestRequest request = new RestRequest(RestSharp.Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", configuration.GetValue<string>("MobilyBasicToken"));
                //request.AddJsonBody(parameter);
                request.AddParameter("application/json", JsonConvert.SerializeObject(MobilyONTRequest), ParameterType.RequestBody);
                _logger.LogInformation(JsonConvert.SerializeObject(MobilyONTRequest));
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = await client.ExecuteAsync(request);
                if (response == null)
                    _logger.LogInformation("No response");
                _logger.LogInformation(response.Content.ToString());

                if (response.IsSuccessful)
                {

                    responseResult = JsonConvert.DeserializeObject<ONTMobilyResponse>(response.Content.ToString());
                    if (responseResult.ErrorCode == "0")
                    {
                        //Update ticket details
                        int ONTID = await _dataService.InsertONTHealthCheck(parameter.ServiceAccountNumber, parameter.Operator, parameter.UserId, parameter.AccountID);
                        var mobilyONTDetail = new ONTMobilyDetail()
                        {
                            SERVICEACCNUM = responseResult.ServiceAccNum,
                            ONT_ID = ONTID,
                            TRANSACTIONNO = responseResult.TransactionNo,
                            OLTRX = responseResult.OLTRx,
                            OLTTX = responseResult.OLTTx,
                            ONTRX = responseResult.ONTRx,
                            ONTTX = responseResult.ONTTx,
                            QUALITY = responseResult.quality,
                            STATUS = responseResult.status,
                            ONTRxHistory = responseResult.ONTRxHistory != null && responseResult.ONTRxHistory.Count > 0 ? JsonConvert.SerializeObject(responseResult.ONTRxHistory) : "",
                            ONTTxHistory = responseResult.ONTTxHistory != null && responseResult.ONTTxHistory.Count > 0 ? JsonConvert.SerializeObject(responseResult.ONTTxHistory) : ""
                            
                        };
                        _logger.LogInformation("Insert ONT Mobily");
                        try
                        {
                            await _dataService.InsertONTMobilyDetails(mobilyONTDetail);
                            await GetONTLatencyMobily(MobilyONTRequest);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                        }
                        return responseResult;
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }
                }
                else

                    throw new Exception(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
                throw ex;
            }
        }

        public async Task<ONTLatencyMobilyResponse> GetONTLatencyMobily(ONTMobilyRequest parameter)
        {
            try
            {
                var MobilyONTLatencyRequest = new ONTLatencyMobilyRequest();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ONTLatencyMobilyResponse responseResult = null;
                MobilyONTLatencyRequest.TransactionNo = parameter.TransactionNo;
                MobilyONTLatencyRequest.ServiceAccNum = parameter.ServiceAccNum;
                MobilyONTLatencyRequest.Flex1 = parameter.Flex1;
                MobilyONTLatencyRequest.Flex2 = parameter.Flex2;
                RestClient client = new RestClient(BaseURL);
                RestRequest request = new RestRequest(RestSharp.Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", configuration.GetValue<string>("MobilyBasicToken"));
                //request.AddJsonBody(parameter);
                request.AddParameter("application/json", JsonConvert.SerializeObject(MobilyONTLatencyRequest), ParameterType.RequestBody);
                _logger.LogInformation(JsonConvert.SerializeObject(MobilyONTLatencyRequest));
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = await client.ExecuteAsync(request);
                if (response == null)
                    _logger.LogInformation("No response");
                _logger.LogInformation(response.Content.ToString());

                //if (response.IsSuccessful)
                {

                    responseResult = JsonConvert.DeserializeObject<ONTLatencyMobilyResponse>(response.Content.ToString());

                    //Update ticket details
                    //int ONTID = await _dataService.InsertONTHealthCheck(parameter.ServiceAccountNumber, parameter.Operator, parameter.UserId, parameter.AccountID);
                    //var mobilyONTDetail = new ONTMobilyDetail()
                    //{
                    //    SERVICEACCNUM = parameter.ServiceAccountNumber,
                    //    ONT_ID = ONTID,
                    //    TRANSACTIONNO = MobilyONTLatencyRequest.TransactionNo,
                    //    //STATUS = responseResult.Status,
                    //    //ONT_LATENCY = responseResult.ONTLatency,
                    //    //ONT_SPEED = responseResult.ONTSpeed,
                    //    IS_LATENCY = 1
                    //};
                   // await _dataService.InsertONTMobilyDetails(mobilyONTDetail);
                    if (responseResult.Status == "1")
                    {
                        _logger.LogError(response.Content);
                    }
                    else
                    {
                        _logger.LogError(response.Content);
                    }
                }

                return responseResult;
                //throw new Exception(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
                throw ex;
            }
        }
    }
}
