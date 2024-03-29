using Go.FTTH.OpenAccess.Service.Data;
using Go.FTTH.OpenAccess.Service.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Go.FTTH.OpenAccess.Service.Services
{
    public interface ISearchService
    {
        Task<dynamic> SearchTicket(SearchModelRequest request);
    }
    public class SearchService : ISearchService
    {
        private readonly CADBContext _context;
        public SearchService(CADBContext context)
        {
            _context = context;
        }
        public async Task<dynamic> SearchTicket(SearchModelRequest request)
        {
            string sql = @"select distinct tt.TICKET_ID,ord.ORDER_ID,tt.ACCOUNT_ID,pln.plan_name PLANNAME,
                            trim(tt.DESCR) DETAIL_DESC,tt.STATUS TICKETSTATUS,tt.OPERATOR_ID,
                            tt.NUMB ORERATOR_REF,tt.CREATE_T CREATED_DATE,tt.USERID CREATED_BY,
                            tt.MODIFIED_T LAST_MODIFIED,tt.""COMMENT"" COMMENTS 
                            from ticket_master_tbl tt,
                            (select ticket_id, short_desc ShortDesc, 'DESC' DetailedDesc, '' ticket_status 
                            from ticket_dawiyat_details union all
                             select ticket_id, 'DESC' ShortDesc, detail_desc DetailedDesc, ticket_status 
                            from ticket_itc_details union all
                             select ticket_no ticket_id, 'DESC' ShortDesc, 'DESC' DetailedDesc,  sub_status 
                             from ticket_mobily_details
                            )
                            openaccess, ACCOUNT_INFO_TBL acc, order_tbl ord, PLAN_DETAILS_TBL pln, CUST_INFO_TBL cust
                            where tt.ticket_id=openaccess.ticket_id
							and tt.account_id=acc.account_id
                            and tt.orderid = ord.order_id
							and acc.ACCOUNT_ID=ord.account_id
							and acc.PLAN_ID=pln.PLAN_ID
							and acc.SUBSCRIBER_ID=cust.SUBSCRIBER_ID
                            and ord.order_type is null";

            if (!string.IsNullOrEmpty(request.TICKET_ID))
            {
                sql = sql + " and tt.TICKET_ID='" + request.TICKET_ID + "'";
            }

            if (!string.IsNullOrEmpty(request.OPERATOR_ID))
            {
                sql = sql + " and lower(tt.OPERATOR_ID)=lower('" + request.OPERATOR_ID + "')";
            }

            if (!string.IsNullOrEmpty(request.ORDER_ID))
            {
                sql = sql + " and ord.ORDER_ID='" + request.ORDER_ID + "'";
            }

            if (!string.IsNullOrEmpty(request.ACCOUNT_ID))
            {
                sql = sql + " and tt.ACCOUNT_ID='" + request.ACCOUNT_ID + "'";
            }

            if (!string.IsNullOrEmpty(request.NUMB))
            {
                sql = sql + " and tt.NUMB='" + request.NUMB + "'";
            }

            if (!string.IsNullOrEmpty(request.Identification_Value))
            {
                sql = sql + " and cust.Identification_Value='" + request.Identification_Value + "'";
            }

            if (request.StartDate != null && request.EndDate != null)
            {
                sql = sql + " and tt.CREATE_T between TO_DATE('" + request.StartDate.Value.ToString("dd-MMM-yyyy") + "','DD/MM/YYYY') AND TO_DATE('" + request.EndDate.Value.ToString("dd-MMM-yyyy") + "','DD/MM/YYYY') + 1 ";
            }

            if (!string.IsNullOrEmpty(request.UserID))
            {
                sql = sql + " and tt.USERID='" + request.UserID + "'";
            }

            if (!string.IsNullOrEmpty(request.COMMENTS))
            {
                sql = sql + @" and tt.""COMMENT""  like '" + request.COMMENTS + "'";
            }

            var result = await _context.SearchDetails.FromSqlRaw(sql).ToListAsync();
            return result;
        }
    }
}
