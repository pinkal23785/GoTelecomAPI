
using Go.FTTH.OpenAccess.Service.Data.Entities;
using Go.FTTH.OpenAccess.Service.Models.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data
{
    public interface IDataService
    {
        Task CreateNewTicketAsync(TicketMaster ticket);
        Task<TicketDetails> GetOperatorDetails(string AccountID);
        Task<int> UpdateDawiyatTicketStatus(string caseNumber, string state, string Comments);
        Task<List<TicketMaster>> GetAllTickets(string UserID);
        Task AddMobilyTicketDetails(TicketMobilyDetail ticket);
        Task AddDawiyatTicketDetails(TicketDawiyatDetail ticket);
        Task AddITCTicketDetails(TicketITCDetail ticket);
        Task<TicketDawiyatDetail> GetTicketDawiyatDetail(string caseNumber);
        Task<TicketITCDetail> GetTicketITCDetail(string caseNumber);
        Task UpdateTicketMasterStatus(string ticketNumber, string status);
        Task UpdateITCTicket(string TicketID, string txnNumber, string SEEKER_SERVICE_NO, string PROVIDER_SERVICE_NO, string WORK_INFO, string WORK_INFO_SUMMARY);
        Task UpdateReopenITCTicket(string TicketID, string txnNumber, string SEEKER_SERVICE_NO, string PROVIDER_SERVICE_NO);

        Task<int> InsertONTHealthCheck(string ServiceAccountNumber, string Operator, string UserId, string AccountID);

        Task InsertONTITCDetails(ONTItcDetail oNTItc);
        Task InsertONTDawiyatDetails(ONTDawiyatDetail oNTDawiyat);

        Task InsertONTMobilyDetails(ONTMobilyDetail oNTMobily);

        Task<dynamic> GetONTList(string UserId);

        Task<TicketMobilyDetail> GetTicketMobilyDetail(string caseNUmber);

        Task UpdateReopenMobilyTicket(string TicketID, string Status);

        Task AddTIcketComments(TicketMasterComment comment);

        Task<dynamic> GetAllSTCTicket();

        Task<dynamic> GetSTCTicketDetail(string TicketID);


        Task<dynamic> GetONTListByAccount(string AccountID);
    }
}
