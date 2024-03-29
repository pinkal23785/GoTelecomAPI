using Go.Service.NumberBooking.Data;
using Go.Service.NumberBooking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Service.NumberBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IDataService _dataservice;
        public BookingController(IDataService dataservice)
        {
            _dataservice = dataservice;
        }


        [HttpGet("get-spn-number")]
        public async Task<IActionResult> GetSpnNumber(string categoryName)
        {
            try
            {
                var result = await _dataservice.GetSPNNumberByCategory(categoryName);
                string html = string.Empty;
                html += "<table>";
                foreach (var d in result)
                {
                    html += "<tr><td><input type=\"checkbox\" \\></td><td>" + d.Number + "</td></tr>";
                }
                html += "</table>";
                return Ok(html);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("reserved-number")]
        public async Task<IActionResult> ReservedNumber(ReservedCustomerRequest request)
        {
            try
            {
                await _dataservice.ReservedNumber(request);
                return Ok("success");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("un-reserved-number")]
        public async Task<IActionResult> UnReservedNumber(string Numbers, string Category)
        {
            try
            {
                await _dataservice.UnReservedNumber(Numbers, Category);
                return Ok("success");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("get-reserved-number-by-customer")]
        public async Task<IActionResult> GetReservedNumberByCustomer(string CustomerId)
        {
            try
            {
                var result = await _dataservice.GetCustomerReservedNumber(CustomerId);
                string html = string.Empty;
                html += "<table>";
                foreach (var d in result)
                {
                    html += "<tr><td><input type=\"checkbox\" \\></td><td>" + d.Number + "</td></tr>";
                }
                html += "</table>";
                return Ok(html);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("get-customer-detail")]
        public async Task<IActionResult> GetCustomerDetails(string MobileNUmber)
        {
            try
            {
                var result = await _dataservice.GetCustomerDetail(MobileNUmber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
