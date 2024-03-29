using Go.Service.NumberBooking.Data;
using Go.Service.NumberBooking.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Go.Service.NumberBooking.WebServices
{
    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract]
        Task<Customer> GetCustomerDetail(string MobileNumber);

        [OperationContract]
        Task<Customer> GetCustomerDetailByFromTo(string From, string To);

        [OperationContract]
        Task<string> UpdateNumberStatus(string From, string To);
    }
    public class CustomerService : ICustomerService
    {
        private readonly IDataService _dataService; 
        private readonly ILogger _logger;
        public CustomerService(IDataService dataService, ILogger<CustomerService> logger)
        {
            _dataService = dataService;
            _logger =logger;
        }
        public async Task<Customer> GetCustomerDetail(string MobileNumber)
        {
            var result = await _dataService.GetCustomerDetail(MobileNumber);
            return result;
        }

        public async Task<Customer> GetCustomerDetailByFromTo(string From, string To)
        {
            try
            {
                var result = await _dataService.GetCustomerDetail(From, To);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "" + ex.StackTrace);
                throw ex;
            }
        }

        public async Task<string> UpdateNumberStatus(string From, string To)
        {
            try
            {
                var result = await _dataService.UpdateNumberStatus(From, To);
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message + "" + ex.StackTrace);
                throw ex;
            }
        }
    }
}
