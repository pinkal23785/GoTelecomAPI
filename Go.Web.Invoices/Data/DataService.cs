using Go.Web.Invoices.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Web.Invoices.Data
{
    public interface IDataService
    {
        Task<CustomerInvoice> getCustomerInvoice(Int64 ID);
        Task<EInvoice_Audit> GetEInvoice_Audit(string OrderID);
    }
    public class DataService : IDataService
    {
        private BRMDBContext _context;
        private BRMDBContext2 _context2;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DataService(BRMDBContext context, BRMDBContext2 context2, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _context2 = context2;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CustomerInvoice> getCustomerInvoice(Int64 ID)
        {
            var result = await _context.CustomerInvoices.Where(x => x.POID_ID0 == ID).FirstOrDefaultAsync();
            if (result != null)
                return await GetRealtime(result);
            throw new Exception("Record Not Found");
        }
        public async Task<EInvoice_Audit> GetEInvoice_Audit(string OrderID)
        {
            var result =await _context.EInvoiceAudit.Where(x => x.ORDER_ID == OrderID).FirstOrDefaultAsync();
            return result;
        }

        public async Task<CustomerInvoice> GetRealtime(CustomerInvoice invoice)
        {
            try
            {

                OracleParameter p1 = new OracleParameter("piv_bill_no", invoice.BILL_NO);

                OracleParameter p2 = new OracleParameter("Pov_total_due", OracleDbType.Double, 50, null, ParameterDirection.Output);
                OracleParameter p3 = new OracleParameter("Pov_current_Total", OracleDbType.Double, 50, null, ParameterDirection.Output);
                OracleParameter p4 = new OracleParameter("pov_previous_total", OracleDbType.Double, 50, null, ParameterDirection.Output);
                OracleParameter p5 = new OracleParameter("pov_payment", OracleDbType.Double, 50, null, ParameterDirection.Output);

                OracleParameter p6 = new OracleParameter("pov_status", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p7 = new OracleParameter("pov_Cycle_Tax", OracleDbType.Double, 50, null, ParameterDirection.Output);
                OracleParameter p8 = new OracleParameter("pov_Current_WV", OracleDbType.Double, 50, null, ParameterDirection.Output);

                string query = "BEGIN PIN.PRC_INVOICE_BILL(:piv_bill_no, :Pov_total_due, :Pov_current_Total,:pov_previous_total, :pov_payment,:pov_status,:pov_Cycle_Tax,:pov_Current_WV);END;";
                var result = _context2.Database.ExecuteSqlRaw(query, p1, p2, p3, p4, p5, p6, p7, p8);
                invoice.TOTAL_DUE = p2.Value != null ? Double.Parse(p2.Value.ToString()) : 0;
                invoice.CURRENT_TOTAL = p3.Value != null ? Double.Parse(p3.Value.ToString()) : 0;
                invoice.PREVIOUS_TOTAL = p4.Value != null ? Double.Parse(p4.Value.ToString()) : 0;
                invoice.PAID_AMOUNT = p5.Value != null ? Double.Parse(p5.Value.ToString()) : 0;
                invoice.CYCLE_TAX = p7.Value != null ? Double.Parse(p7.Value.ToString()) : 0; ;
                invoice.CURRENT_TOTAL_WV = p8.Value != null ? Double.Parse(p8.Value.ToString()) : 0; ;
                return invoice;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
