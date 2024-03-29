
using Go.FTTH.OpenAccess.Service.Data.Entities;
using Go.FTTH.OpenAccess.Service.Models.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data
{
    public class DataService : IDataService
    {
        private CADBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DataService(CADBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateNewTicketAsync(TicketMaster ticket)
        {
            await _context.TicketMasters.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<TicketDetails> GetOperatorDetails(string AccountID)
        {
            try
            {
                OracleParameter p1 = new OracleParameter("piv_account_id", AccountID);
                //p1.Value = AccountID;
                OracleParameter p2 = new OracleParameter("piv_order_id", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p3 = new OracleParameter("piv_sub_id", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p4 = new OracleParameter("piv_operator_id", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p5 = new OracleParameter("piv_region", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p6 = new OracleParameter("piv_city", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p7 = new OracleParameter("piv_district", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p8 = new OracleParameter("piv_serial_number", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p9 = new OracleParameter("piv_macid", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p10 = new OracleParameter("piv_operator_ref", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p11 = new OracleParameter("piv_odb_id", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p12 = new OracleParameter("piv_cust_id", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p13 = new OracleParameter("piv_contact_no", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p14 = new OracleParameter("piv_circuit_id", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p15 = new OracleParameter("piv_service_provider_Id", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p16 = new OracleParameter("pov_status", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                string query = "BEGIN PKG_FTTH.prc_ticket_details(:piv_account_id, :piv_order_id, :piv_sub_id, :piv_operator_id,:piv_region,";
                query = query + ":piv_city,:piv_district,:piv_serial_number,:piv_macid,:piv_operator_ref,:piv_odb_id,:piv_cust_id,:piv_contact_no,:piv_circuit_id,:piv_service_provider_Id,:pov_status);END;";
                var result = _context.Database.ExecuteSqlRaw(query, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16);
                // return p4.Value.ToString();
                var taskDetails = new TicketDetails();
                taskDetails.PIV_ORDER_ID = p2.Value.ToString();
                taskDetails.PIV_SUB_ID = p3.Value.ToString();
                taskDetails.PIV_OPERATOR_ID = p4.Value.ToString();
                taskDetails.PIV_REGION = p5.Value.ToString();
                taskDetails.PIV_CITY = p6.Value.ToString();
                taskDetails.PIV_DISTRICT = p7.Value.ToString();
                taskDetails.PIV_SERIAL_NUMBER = p8.Value.ToString();
                taskDetails.PIV_MACID = p9.Value.ToString();
                taskDetails.PIV_OPERATOR_REF = p10.Value.ToString();
                taskDetails.PIV_ODB_ID = p11.Value.ToString();
                taskDetails.PIV_CUST_ID = p12.Value.ToString();
                taskDetails.PIV_CONTACT_NO = p13.Value.ToString();
                taskDetails.PIV_CIRCUIT_ID = p14.Value.ToString();
                taskDetails.SERVICE_PROVIDER_ID = p15.Value.ToString();
                taskDetails.POV_STATUS = p16.Value.ToString();

                return taskDetails;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateDawiyatTicketStatus(string caseNumber, string state, string Comments)
        {
            var result = _context.TicketMasters.Where(x => x.NUMB == caseNumber).FirstOrDefault();
            if (result != null)
            {
                result.COMMENT = Comments;
                result.STATUS = state;
                await _context.SaveChangesAsync();

                //Add comments
                await AddTIcketComments(new TicketMasterComment()
                {
                    TicketMasterID = result.ID,
                    Comments = result.COMMENT,
                    Created = DateTime.Now
                });

                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<List<TicketMaster>> GetAllTickets(string UserID)
        {
            var result = await _context.TicketMasters.ToListAsync();
            if (!string.IsNullOrEmpty(UserID))
            {
                result = result.Where(x => x.USERID == UserID).OrderByDescending(x => x.CREATE_T).ToList();
            }
            return result;
        }
        public async Task<dynamic> GetAllSTCTicket()
        {
            var result = await _context.STCTroubleTicket.Select(x => new
            {
                x.TROUBLE_TICKETID,
                x.STATUS,
                x.PROBLEMID,
                x.PROBLEMTYPE,
                x.CREATEDDATE,
                x.SERVICENUMBER,
                x.SERVICETYPE,
                x.SERVICE_ACCOUNT_NUMBER,
                x.SERVICE_IMPACT_START,
                x.MOBILE,
                x.EMAIL


            }).ToListAsync();
            return result;
        }

        public async Task<dynamic> GetSTCTicketDetail(string TicketID)
        {
            var result = await _context.STCTroubleTicket.Where(x => x.TROUBLE_TICKETID == TicketID).FirstOrDefaultAsync();
            if (result == null)
                throw new KeyNotFoundException("Record not found");
            return result;
        }
        public async Task AddMobilyTicketDetails(TicketMobilyDetail ticket)
        {
            var result = _context.TicketMobilyDetails.Where(x => x.TICKET_NO == ticket.TICKET_NO).FirstOrDefault();
            if (result == null)
            {
                await _context.AddAsync(ticket);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("Duplicate ticket not allow");
        }

        public async Task AddDawiyatTicketDetails(TicketDawiyatDetail ticket)
        {
            var result = _context.TicketDawiyatDetails.Where(x => x.TICKET_ID == ticket.TICKET_ID).FirstOrDefault();
            if (result == null)
            {
                await _context.AddAsync(ticket);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("Duplicate ticket not allow");
        }

        public async Task AddITCTicketDetails(TicketITCDetail ticket)
        {
            var result = _context.TicketITCDetails.Where(x => x.TICKET_ID == ticket.TICKET_ID).FirstOrDefault();
            if (result == null)
            {
                await _context.AddAsync(ticket);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("Duplicate ticket not allow");
        }

        public async Task<TicketDawiyatDetail> GetTicketDawiyatDetail(string caseNumber)
        {
            var ticket = await _context.TicketMasters.Where(x => x.NUMB == caseNumber).FirstOrDefaultAsync();
            TicketDawiyatDetail dawiyatDetail = null;
            if (ticket != null)
            {
                dawiyatDetail = await _context.TicketDawiyatDetails.Where(x => x.TICKET_ID == ticket.TICKET_ID).FirstOrDefaultAsync();
                if (dawiyatDetail != null)
                {
                    dawiyatDetail.State = ticket.STATUS;
                }
            }
            return dawiyatDetail;
        }
        public async Task<TicketITCDetail> GetTicketITCDetail(string TicketID)
        {
            var ticket = await _context.TicketMasters.Where(x => x.TICKET_ID == TicketID).FirstOrDefaultAsync();
            TicketITCDetail itcDetail = null;
            if (ticket != null)
            {
                itcDetail = await _context.TicketITCDetails.Where(x => x.TICKET_ID == ticket.TICKET_ID).FirstOrDefaultAsync();

            }
            return itcDetail;
        }

        public async Task UpdateTicketMasterStatus(string ticketNumber, string status)
        {
            var ticket = _context.TicketMasters.Where(x => x.TICKET_ID == ticketNumber).FirstOrDefault();
            if (ticket != null)
            {
                ticket.STATUS = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateITCTicket(string TicketID, string txnNumber, string SEEKER_SERVICE_NO, string PROVIDER_SERVICE_NO,
            string WORK_INFO, string WORK_INFO_SUMMARY)
        {
            var ticketMaster = _context.TicketMasters.Where(x => x.TICKET_ID == TicketID).FirstOrDefault();
            var ticketDetail = _context.TicketITCDetails.Where(x => x.TICKET_ID == TicketID).FirstOrDefault();
            if (ticketDetail != null)
            {
                ticketDetail.SEEKER_SERVICE_NO = SEEKER_SERVICE_NO;
                ticketDetail.PROVIDER_SERVICE_NO = PROVIDER_SERVICE_NO;
                ticketDetail.WORK_INFO = WORK_INFO;
                ticketDetail.WORK_INFO_SUMMARY = WORK_INFO_SUMMARY;
                ticketDetail.TXNUMBER = txnNumber;
                await _context.SaveChangesAsync();

                //Add comments
                await AddTIcketComments(new TicketMasterComment()
                {
                    TicketMasterID = ticketMaster.ID,
                    Work_Info = WORK_INFO,
                    Work_Info_Summary = WORK_INFO_SUMMARY,
                    Created = DateTime.Now
                });
            }
        }

        public async Task UpdateReopenITCTicket(string TicketID, string txnNumber, string SEEKER_SERVICE_NO, string PROVIDER_SERVICE_NO)
        {
            var ticketDetail = _context.TicketITCDetails.Where(x => x.TICKET_ID == TicketID).FirstOrDefault();
            if (ticketDetail != null)
            {
                ticketDetail.SEEKER_SERVICE_NO = SEEKER_SERVICE_NO;
                ticketDetail.PROVIDER_SERVICE_NO = PROVIDER_SERVICE_NO;
                ticketDetail.TXNUMBER = txnNumber;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateReopenMobilyTicket(string TicketID, string Status)
        {
            var mobilyTicket = _context.TicketMasters.Where(x => x.NUMB == TicketID).FirstOrDefault();
            if (mobilyTicket != null)
            {
                mobilyTicket.STATUS = Status;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> InsertONTHealthCheck(string ServiceAccountNumber, string Operator, string UserID, string AccountID)
        {
            //var objHealthChecj = await _context.ONT_HEALTH_CHECKS.Where(x => x.Service_Account_Number == ServiceAccountNumber).FirstOrDefaultAsync();
            //if (objHealthChecj == null)
            {
                var newONTService = new ONT_HEALTH_CHECK()
                {
                    Service_Account_Number = ServiceAccountNumber,
                    Created_DT = DateTime.Now,
                    Operator = Operator,
                    UserID = UserID,
                    AccountID = AccountID
                };
                await _context.AddAsync(newONTService);
                await _context.SaveChangesAsync();
                return newONTService.ID;
            }

            //return objHealthChecj.ID;
        }

        public async Task InsertONTITCDetails(ONTItcDetail oNTItc)
        {
            //var obj = await _context.ONTItcDetails.Where(x => x.ONT_ID == oNTItc.ONT_ID).FirstOrDefaultAsync();
            //if (obj = null)
            //{
            await _context.ONTItcDetails.AddAsync(oNTItc);
            await _context.SaveChangesAsync();
            //}
            //else
            //{
            //    obj.LAST_DOWN_TIME = oNTItc.LAST_DOWN_TIME;
            //    obj.LAST_UP_TIME = oNTItc.LAST_UP_TIME;
            //    obj.RUN_STATUS = oNTItc.RUN_STATUS;
            //    obj.TXN_NUMBER = oNTItc.TXN_NUMBER;
            //    obj.gponRx = oNTItc.gponRx;
            //    obj.gponTx = oNTItc.gponTx;
            //    obj.ontTx = oNTItc.ontTx;
            //    obj.ontRx = oNTItc.ontRx;
            //    await _context.SaveChangesAsync();

            //}
        }


        public async Task InsertONTDawiyatDetails(ONTDawiyatDetail oNTDawiyat)
        {
            //var obj = await _context.ONTDawiyatDetails.Where(x => x.ONT_ID == oNTDawiyat.ONT_ID).FirstOrDefaultAsync();
            //if (obj == null)
            //{
            await _context.ONTDawiyatDetails.AddAsync(oNTDawiyat);
            await _context.SaveChangesAsync();
            //}
            //else
            //{
            //    obj.THROUGHPUT = oNTDawiyat.THROUGHPUT;
            //    obj.LATENCY = oNTDawiyat.LATENCY;
            //    obj.MODIFY_DT = DateTime.Now;
            //    await _context.SaveChangesAsync();

            //}
        }

        public async Task InsertONTMobilyDetails(ONTMobilyDetail oNTMobily)
        {
            //var obj = await _context.ONTMobilyDetails.Where(x => x.ONT_ID == oNTMobily.ONT_ID).FirstOrDefaultAsync();
            //if (obj == null)
            //{
            await _context.ONTMobilyDetails.AddAsync(oNTMobily);
            await _context.SaveChangesAsync();
            //}
            //else
            //{
            //    obj.SERVICEACCNUM = oNTMobily.SERVICEACCNUM;

            //    obj.TRANSACTIONNO = oNTMobily.TRANSACTIONNO;
            //    obj.OLTRX = oNTMobily.OLTRX;
            //    obj.OLTTX = oNTMobily.OLTTX;
            //    obj.ONTRX = oNTMobily.ONTRX;
            //    obj.ONTTX = oNTMobily.ONTTX;
            //    obj.QUALITY = oNTMobily.QUALITY;
            //    obj.STATUS = oNTMobily.STATUS;
            //    obj.ONTRxHistory = oNTMobily.ONTRxHistory;
            //    obj.ONTTxHistory = oNTMobily.ONTTxHistory;
            //    await _context.SaveChangesAsync();

            //}
        }

        public async Task<dynamic> GetONTList(string UserId)
        {
            var listofONT =await _context.ONT_HEALTH_CHECKS.Where(x => x.UserID == UserId && x.Created_DT >= DateTime.Now.AddDays(-7)).OrderByDescending(x=>x.Created_DT).ToListAsync();
            List<dynamic> ONTDetails = new List<dynamic>();
            foreach (var ONT in listofONT)
            {
                if (ONT.Operator.ToLower() == "dawiyat")
                {
                    var details = await _context.ONTDawiyatDetails.Where(x => x.ONT_ID == ONT.ID).FirstOrDefaultAsync();
                    ONTDetails.Add(details);
                }
                else if (ONT.Operator.ToLower() == "mobily")
                {
                    var details = await _context.ONTMobilyDetails.Where(x => x.ONT_ID == ONT.ID).FirstOrDefaultAsync();
                    ONTDetails.Add(details);
                }
                else if (ONT.Operator.ToLower() == "itc")
                {
                    var details = await _context.ONTItcDetails.Where(x => x.ONT_ID == ONT.ID).FirstOrDefaultAsync();
                    ONTDetails.Add(details);
                }
            }
            var result = Tuple.Create(listofONT, ONTDetails);
            return result;
        }

        public async Task<dynamic> GetONTListByAccount(string AccountID)
        {

            var listofONT = await _context.ONT_HEALTH_CHECKS.Where(x => x.AccountID == AccountID).OrderByDescending(x=>x.Created_DT).ToListAsync();
            List<dynamic> ONTDetails = new List<dynamic>();
            foreach (var ONT in listofONT)
            {
                if (ONT.Operator.ToLower() == "dawiyat")
                {
                    var details = await _context.ONTDawiyatDetails.Where(x => x.ONT_ID == ONT.ID).FirstOrDefaultAsync();
                    ONTDetails.Add(details);
                }
                else if (ONT.Operator.ToLower() == "mobily")
                {
                    var details = await _context.ONTMobilyDetails.Where(x => x.ONT_ID == ONT.ID).FirstOrDefaultAsync();
                    ONTDetails.Add(details);
                }
                else if (ONT.Operator.ToLower() == "itc")
                {
                    var details = await _context.ONTItcDetails.Where(x => x.ONT_ID == ONT.ID).FirstOrDefaultAsync();
                    ONTDetails.Add(details);
                }
            }
            var result = Tuple.Create(listofONT, ONTDetails);
            return result;
        }


        public async Task<TicketMobilyDetail> GetTicketMobilyDetail(string caseNUmber)
        {
            var result = _context.TicketMobilyDetails.Where(x => x.TICKET_NO == caseNUmber).FirstOrDefault();
            return result;
        }

        public async Task AddTIcketComments(TicketMasterComment comment)
        {
            if (comment != null)
            {
                await _context.TicketMasterComments.AddAsync(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
