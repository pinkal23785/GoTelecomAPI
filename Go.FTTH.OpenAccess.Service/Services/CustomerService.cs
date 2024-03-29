using Go.FTTH.OpenAccess.Service.Data;
using Go.FTTH.OpenAccess.Service.Data.Entities;
using Go.FTTH.OpenAccess.Service.Models.Customers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Services
{
    public interface ICustomerService
    {
        Task<Customer> SearchCustomer(SearchCustomerRequest parameter);
    }
    public class CustomerService : ICustomerService
    {
        private CADBContext _context;
        public CustomerService(CADBContext context)
        {
            _context = context;
        }
        public async Task<Customer> SearchCustomer(SearchCustomerRequest parameter)
        {
            string query = " select first_name FIRSTNAME,fourth_name LASTNAME, mobile_no  MOBILE, email_id  EMAIL, 'Low' PRIORITY ";
            query = query + " From CUST_CONTACT_DTL x, account_info_Tbl y, cust_info_Tbl z where x.subscriber_id = y.subscriber_id";
            query = query + " and x.subscriber_id = z.subscriber_id";

            if (!string.IsNullOrEmpty(parameter.customerId))
                query = query + " and identification_value = '" + parameter.customerId + "'";

            if (!string.IsNullOrEmpty(parameter.accountId))
                query = query + " and account_id = '" + parameter.accountId + "'";

            if (!string.IsNullOrEmpty(parameter.subscriberId))
                query = query + " and x.subscriber_id = '" + parameter.subscriberId + "'";

            //and(x.subscriber_id = '610245025' or account_id = '610245025' or identification_value = '610245025')

            var customers = await _context.Customers.FromSqlRaw(query).FirstOrDefaultAsync();
            return customers;
        }
    }
}
