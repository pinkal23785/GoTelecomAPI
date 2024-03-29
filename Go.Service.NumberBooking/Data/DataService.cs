using Go.Service.NumberBooking.Data.Entities;
using Go.Service.NumberBooking.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Go.Service.NumberBooking.Data
{
    public interface IDataService
    {
        Task<List<NumberResponse>> GetSPNNumberByCategory(string category);
        Task ReservedNumber(ReservedCustomerRequest request);

        Task UnReservedNumber(string Numbers, string Category);

        Task<List<NumberResponse>> GetCustomerReservedNumber(string CustomerID);

        Task<Customer> GetCustomerDetail(string MobileNumber);

        Task<Customer> GetCustomerDetail(string From, string To);

        Task<string> UpdateNumberStatus(string From, string To);
    }
    public class DataService : IDataService
    {
        private readonly NBSContext _context;
        public DataService(NBSContext context)
        {
            _context = context;
        }
        public async Task<List<NumberResponse>> GetCustomerReservedNumber(string CustomerID)
        {
            List<NumberResponse> result = null;
            result = (from spn in _context.SPNNumbers
                      join cat in _context.Categories

                      on spn.SPN_Cat_Id equals cat.Cat_Id
                      join cust in _context.Customers
                      on spn.SPN_Customer_Id equals cust.Customer_Id
                      where cust.Customer_Name == CustomerID && (spn.SPN_Status == 3)
                      select new NumberResponse
                      {
                          Number = cat.Cat_Name + "-" + spn.SPN_Number
                      }).ToList();

            List<NumberResponse> result2 = null;

            result2 = (from spn in _context.GTSNumberRanges
                       join cat in _context.GTSCategories
                       on spn.Range_GTS_CATID equals cat.GTS_CAT_ID
                       join cust in _context.Customers
                       on new { ID = spn.Range_GTS_Customer_Id } equals new { ID = cust.Customer_Id.ToString() }
                       where cust.Customer_Name == CustomerID && spn.Range_GTS_Status == 1
                       select new NumberResponse
                       {
                           Number = (cat.GTS_CATEGORY + "-" + spn.Range_GTS_Number_From) + " to " + (cat.GTS_CATEGORY + "-" + spn.Range_GTS_Number_To)
                       }).ToList();

            return result.Union(result2).ToList();
        }

        public async Task<List<NumberResponse>> GetSPNNumberByCategory(string category)
        {
            List<NumberResponse> result = null;

            if (category != "GTS")
            {
                result = (from spn in _context.SPNNumbers
                          join cat in _context.Categories
                          on spn.SPN_Cat_Id equals cat.Cat_Id
                          where cat.Cat_Name.Contains(category) && (spn.SPN_Status == 1 || spn.SPN_Status == 2)
                          select new NumberResponse
                          {
                              Number = cat.Cat_Name + "-" + spn.SPN_Number
                          }).ToList();
            }
            else
            {
                result = (from spn in _context.GTSNumberRanges
                          join cat in _context.GTSCategories
                          on spn.Range_GTS_CATID equals cat.GTS_CAT_ID
                          where spn.Range_GTS_Status == 1
                          select new NumberResponse
                          {
                              Number = (cat.GTS_CATEGORY + "-" + spn.Range_GTS_Number_From) + " to " + (cat.GTS_CATEGORY + "-" + spn.Range_GTS_Number_To)
                          }).ToList();
            }
            return result;
        }

        public async Task ReservedNumber(ReservedCustomerRequest request)
        {
            List<string> NumberSplit = request.Numbers.Split(",").ToList();
            if (request.Category != "GTS")
            {
                var selected = (from spn in _context.SPNNumbers
                                join cat in _context.Categories
                                on spn.SPN_Cat_Id equals cat.Cat_Id
                                where cat.Cat_Name == request.Category && (spn.SPN_Status == 1 || spn.SPN_Status == 2) &&
                                NumberSplit.Contains(cat.Cat_Name + "-" + spn.SPN_Number)
                                select spn).ToList();

                //Check the customer exist

                Customer objCustomer = _context.Customers.Where(x => x.Customer_Name == request.CRMCustomerId).FirstOrDefault();
                if (objCustomer == null)
                {
                    objCustomer = new Customer()
                    {
                        Customer_Name = request.CRMCustomerId
                    };
                    await _context.Customers.AddAsync(objCustomer);
                    await _context.SaveChangesAsync();

                }

                foreach (var n in selected)
                {
                    n.SPN_Status = 3;
                    n.SPN_Date_Booked = DateTime.Now;
                    n.SPN_Account_Manager = request.AccountManagerName;
                    n.SPN_AM_Email = request.AccountManagerEmail;
                    n.SPN_Customer_Id = objCustomer.Customer_Id;


                }
                await _context.SaveChangesAsync();
            }
            else
            {
                var selected = (from spn in _context.GTSNumberRanges
                                join cat in _context.GTSCategories
                                on spn.Range_GTS_CATID equals cat.GTS_CAT_ID
                                where spn.Range_GTS_Status == 1 &&
                               NumberSplit.Contains((cat.GTS_CATEGORY + "-" + spn.Range_GTS_Number_From) + " to " + (cat.GTS_CATEGORY + "-" + spn.Range_GTS_Number_To))
                                select spn).ToList();


                Customer objCustomer = _context.Customers.Where(x => x.Customer_Name == request.CRMCustomerId).FirstOrDefault();
                if (objCustomer == null)
                {
                    objCustomer = new Customer()
                    {
                        Customer_Name = request.CRMCustomerId
                    };
                    await _context.Customers.AddAsync(objCustomer);
                    await _context.SaveChangesAsync();

                }
                foreach (var n in selected)
                {
                    n.Range_GTS_Status = 3;
                    n.Range_GTS_Date_Booked = DateTime.Now;
                    n.Range_GTS_Customer_Id = objCustomer.Customer_Id.ToString();
                    n.Range_GTS_AM_Email = request.AccountManagerEmail;
                    n.Range_GTS_Account_Manager = request.AccountManagerName;
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task UnReservedNumber(string Numbers, string Category)
        {
            List<string> NumberSplit = Numbers.Split(",").ToList();

            if (Category != "GTS")
            {
                var selected = (from spn in _context.SPNNumbers
                                join cat in _context.Categories
                                on spn.SPN_Cat_Id equals cat.Cat_Id
                                where cat.Cat_Name == Category &&
                                NumberSplit.Contains(cat.Cat_Name + "-" + spn.SPN_Number)
                                select spn).ToList();

                foreach (var n in selected)
                {
                    n.SPN_Status = 1;
                    n.SPN_Date_Booked = DateTime.Now;
                    n.SPN_Account_Manager = string.Empty;
                    n.SPN_AM_Email = string.Empty;
                    n.SPN_Customer_Id = null;


                }
                await _context.SaveChangesAsync();
            }
            else
            {
                var selected = (from spn in _context.GTSNumberRanges
                                join cat in _context.GTSCategories
                                on spn.Range_GTS_CATID equals cat.GTS_CAT_ID
                                where
                               NumberSplit.Contains((cat.GTS_CATEGORY + "-" + spn.Range_GTS_Number_From) + " to " + (cat.GTS_CATEGORY + "-" + spn.Range_GTS_Number_To))
                                select spn).ToList();

                foreach (var n in selected)
                {
                    n.Range_GTS_Status = 1;
                    n.Range_GTS_Customer_Id = null;
                    n.Range_GTS_AM_Email = string.Empty;
                    n.Range_GTS_Account_Manager = string.Empty;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Customer> GetCustomerDetail(string MobileNumber)
        {
            var result = await _context.Customers.Where(x => x.Customer_Mobile == MobileNumber).FirstOrDefaultAsync();
            if (result == null)
            {
                throw new Exception("Record not found");
            }
            return result;
        }

        public async Task<Customer> GetCustomerDetail(string From, string To)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();

            paramList.Add(new SqlParameter("FromNumber", From));
            paramList.Add(new SqlParameter("ToNumber", To));
            var result = await _context.Customers.FromSqlRaw($"EXECUTE  GetCustomerDetails {From}, {To} ").ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<string> UpdateNumberStatus(string From, string To)
        {
            var result = await _context.Database.ExecuteSqlRawAsync($"EXECUTE  UpdatePortNumberStatus {From}, {To} ");
            return "success";
        }
    }
}
