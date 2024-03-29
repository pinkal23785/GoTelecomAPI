using Go.FTTH.OpenAccess.Service.Data;
using Go.FTTH.OpenAccess.Service.Models.Customers;
using Go.FTTH.OpenAccess.Service.Models.Dawiyat.CreateCase;
using Go.FTTH.OpenAccess.Service.Models.Search;
using Go.FTTH.OpenAccess.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly IDataService dataService;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISearchService searchService;
        public CustomerController(ICustomerService _customerService, ILogger<CustomerController> logger,
            IDataService _dataService, IConfiguration configuration, ISearchService _searchService)
        {
            customerService = _customerService;
            _logger = logger;
            dataService = _dataService;
            _configuration = configuration;
            searchService = _searchService;
        }

        //[HttpGet("search-customer")]
        //public async Task<IActionResult> SearchCustomer([FromQuery] SearchCustomerRequest request)
        //{
        //    try
        //    {
        //        var result = await customerService.SearchCustomer(request);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpGet("get-operator-details")]
        public async Task<IActionResult> GetOperatorDetails(string AccountId)
        {
            try
            {
                var result = await dataService.GetOperatorDetails(AccountId);
                result.ShowContactAccount = _configuration.GetValue<string>("ShowContactAccount");
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("search-ticket")]
        public async Task<IActionResult> SearchTicket([FromQuery] SearchModelRequest request)
        {
            try
            {
                var result = await searchService.SearchTicket(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
