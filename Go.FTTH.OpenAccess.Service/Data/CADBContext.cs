using Go.FTTH.OpenAccess.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data
{
    public class CADBContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TicketMaster> TicketMasters { get; set; }
        public DbSet<TicketMobilyDetail> TicketMobilyDetails { get; set; }
        public DbSet<TicketDawiyatDetail> TicketDawiyatDetails { get; set; }

        public DbSet<TicketITCDetail> TicketITCDetails { get; set; }
        public DbSet<ONT_HEALTH_CHECK> ONT_HEALTH_CHECKS { get; set; }
        public DbSet<ONTItcDetail> ONTItcDetails { get; set; }

        public DbSet<ONTDawiyatDetail> ONTDawiyatDetails { get; set; }
        public DbSet<ONTMobilyDetail> ONTMobilyDetails { get; set; }
        public DbSet<SearchDetail> SearchDetails { get; set; }

        public DbSet<TicketMasterComment> TicketMasterComments { get; set; }

        public DbSet<STC_Trouble_Ticket> STCTroubleTicket { get; set; }
        public CADBContext(DbContextOptions<CADBContext> options)
          : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasNoKey();
            modelBuilder.Entity<TicketMaster>(entity =>
            {
                entity.HasKey(x => new { x.ID });
                entity.Property(e => e.ID);
                entity.Property(e => e.TICKET_ID);
                entity.Property(e => e.OPERATOR_ID);
                entity.Property(e => e.STATUS);
                entity.Property(e => e.REFERENCE_ID);
                entity.Property(e => e.CREATE_T);
                entity.Property(e => e.ACCOUNT_ID);
                entity.Property(e => e.DESCR);
                entity.Property(e => e.MODIFIED_T);
                entity.Property(e => e.NUMB);
                entity.Property(e => e.SYS_ID);
                entity.Property(e => e.USERID);
                entity.Property(e => e.COMMENT);
                entity.Property(e => e.ORDERID);
                entity.ToTable("TICKET_MASTER_TBL");
            });
            modelBuilder.Entity<TicketMobilyDetail>(entity =>
            {
                entity.HasKey(x => new { x.ID });
                entity.Property(e => e.ID);
                entity.Property(e => e.TICKET_NO);
                entity.Property(e => e.SERVICEACCNUM);
                entity.Property(e => e.CUSTOMER_TYPE);
                entity.Property(e => e.SRTYPE);
                entity.Property(e => e.SRAREA);
                entity.Property(e => e.SR_SUB_AREA);
                entity.Property(e => e.CHANNEL);
                entity.Property(e => e.DESC);
                entity.Property(e => e.SUB_STATUS);
                entity.Property(e => e.SERVICE_OWNER_NAME);
                entity.Property(e => e.SERVICE_OWNER_NUMBER);
                entity.Property(e => e.FLEX1);
                entity.Property(e => e.FLEX2);
                entity.ToTable("TICKET_MOBILY_DETAILS");
            });

            modelBuilder.Entity<TicketDawiyatDetail>(entity =>
            {
                entity.HasKey(x => new { x.ID });
                entity.Property(e => e.ID);
                entity.Property(e => e.TICKET_ID);
                entity.Property(e => e.BUSINESS_SERVICE);
                entity.Property(e => e.PRODUCT);
                entity.Property(e => e.CATEGORY);
                entity.Property(e => e.SUBCATEGORY);
                entity.Property(e => e.SUBCATEGORY_L2);
                entity.Property(e => e.CUSTOMER_NAME);
                entity.Property(e => e.CUSTOMER_ORDER_NUMBER);
                entity.Property(e => e.CUSTOMER_CONTACT_NUMBER);
                entity.Property(e => e.OBDID);
                entity.Property(e => e.SHORT_DESC);
                entity.Property(e => e.DESC);
                entity.Property(e => e.CIRCUIT_ID);
                entity.Property(e => e.REGION);
                entity.Property(e => e.CITY);
                entity.Property(e => e.DISTRICT);
                entity.Property(e => e.DAWIYAT_BUILDING_ID);
                entity.Property(e => e.SAUDI_NATION_ADDRESS);
                entity.Property(e => e.OLO_CUSTOMER_ID);
                entity.Property(e => e.DAW_SERVICE_ID);
                entity.Property(e => e.ACCOUNT);
                entity.Property(e => e.CONTACT);
                entity.ToTable("TICKET_DAWIYAT_DETAILS");
            });

            modelBuilder.Entity<TicketITCDetail>(entity =>
            {
                entity.HasKey(x => new { x.ID });
                entity.Property(e => e.ID);
                entity.Property(e => e.TICKET_ID);
                entity.Property(e => e.SEEKER);
                entity.Property(e => e.TXNUMBER);
                entity.Property(e => e.OPERATION);
                entity.Property(e => e.SEEKER_SERVICE_NO);
                entity.Property(e => e.PROVIDER_SERVICE_NO);
                entity.Property(e => e.IMPACT);
                entity.Property(e => e.URGENCY);
                entity.Property(e => e.FIRSTNAME);
                entity.Property(e => e.LASTNAME);
                entity.Property(e => e.DESC);
                entity.Property(e => e.DETAIL_DESC);
                entity.Property(e => e.TICKET_STATUS);
                entity.Property(e => e.CONTACT_PHONE);
                entity.Property(e => e.PROBLEM_CODE);
                entity.Property(e => e.SEVERITY);
                entity.Property(e => e.SERVICE_IMPACTED);
                entity.Property(e => e.EXTERNAL_REFERENCE_NO);
                entity.Property(e => e.ACTUAL_INCIDENT_START_DATE);
                entity.Property(e => e.WORK_INFO);
                entity.Property(e => e.WORK_INFO_SUMMARY);
                entity.ToTable("TICKET_ITC_DETAILS");
            });

            modelBuilder.Entity<ONT_HEALTH_CHECK>(entity =>
            {
                entity.HasKey(x => new { x.ID });
                entity.Property(e => e.ID);
                entity.Property(e => e.Created_DT).HasColumnName("CREATED_DT");
                entity.Property(e => e.Operator).HasColumnName("OPERATOR");
                entity.Property(e => e.Service_Account_Number).HasColumnName("SERVICE_ACCOUNT_NO");
                entity.Property(e => e.UserID).HasColumnName("USERID");
                entity.Property(e => e.AccountID).HasColumnName("ACCOUNTID");
                entity.ToTable("ONT_HEALTH_CHECK");
            });

            modelBuilder.Entity<ONTItcDetail>(entity =>
            {
                entity.HasKey(x => new { x.ID });
                entity.Property(e => e.ID);
                entity.Property(e => e.ONT_ID);
                entity.Property(e => e.TXN_NUMBER).HasColumnName("TXN_NUMBER");
                entity.Property(e => e.RUN_STATUS).HasColumnName("RUN_STATUS");
                entity.Property(e => e.LAST_UP_TIME).HasColumnName("LAST_UP_TIME");
                entity.Property(e => e.LAST_DOWN_TIME).HasColumnName("LAST_DOWN_TIME");
                entity.Property(e => e.MODIFT_DT).HasColumnName("MODIFY_DT");

                entity.Property(e => e.ontRx).HasColumnName("ONTRX");
                entity.Property(e => e.ontTx).HasColumnName("ONTTX");
                entity.Property(e => e.gponRx).HasColumnName("GPONRX");
                entity.Property(e => e.gponTx).HasColumnName("GPONTX");
                //entity.Property(e => e.MODIFT_DT).HasColumnName("MODIFY_DT");

                entity.ToTable("ONT_ITC_DETAILS");
            });

            modelBuilder.Entity<ONTDawiyatDetail>(entity =>
            {
                entity.HasKey(x => new { x.ID });
                entity.Property(e => e.ID);
                entity.Property(e => e.ONT_ID);
                entity.Property(e => e.LATENCY).HasColumnName("LATENCY");
                entity.Property(e => e.THROUGHPUT).HasColumnName("THROUGHPUT");
                entity.Property(e => e.MODIFY_DT).HasColumnName("MODIFY_DT");

                entity.Property(e => e.Rx).HasColumnName("RX");
                entity.Property(e => e.Tx).HasColumnName("TX");
                entity.Property(e => e.DownloadSpeed).HasColumnName("DOWNLOADSPEED");
                entity.Property(e => e.UploadSpeed).HasColumnName("UPLOADSPEED");
                entity.Property(e => e.ONTStatus).HasColumnName("ONTSTATUS");
                entity.ToTable("ONT_DAWIYAT_DETAILS");
            });

            modelBuilder.Entity<ONTMobilyDetail>(entity =>
            {
                entity.HasKey(x => new { x.ID });
                entity.Property(e => e.ID);
                entity.Property(e => e.ONT_ID);
                entity.Property(e => e.TRANSACTIONNO).HasColumnName("TRANSACTIONNO");
                entity.Property(e => e.SERVICEACCNUM).HasColumnName("SERVICEACCNUM");
                entity.Property(e => e.STATUS).HasColumnName("STATUS");

                entity.Property(e => e.QUALITY).HasColumnName("QUALITY");
                entity.Property(e => e.ONTRX).HasColumnName("ONTRX");
                entity.Property(e => e.ONTTX).HasColumnName("ONTTX");
                entity.Property(e => e.OLTRX).HasColumnName("OLTRX");
                entity.Property(e => e.OLTTX).HasColumnName("OLTTX");

                entity.Property(e => e.ONT_LATENCY).HasColumnName("ONT_LATENCY");
                entity.Property(e => e.ONT_SPEED).HasColumnName("ONT_SPEED");
                entity.Property(e => e.IS_LATENCY).HasColumnName("IS_LATENCY");

                entity.Property(e => e.ONTRxHistory).HasColumnName("ONTRXHISTORY");
                entity.Property(e => e.ONTTxHistory).HasColumnName("ONTTXHISTORY");

                entity.ToTable("ONT_MOBILY_DETAIL");
            });
            modelBuilder.Entity<TicketMasterComment>(entity =>
            {
                entity.HasKey(x => new { x.ID });
                entity.Property(e => e.ID);
                entity.Property(e => e.TicketMasterID).HasColumnName("TICKET_MASTER_ID");
                entity.Property(e => e.Comments).HasColumnName("COMMENTS");
                entity.Property(e => e.Created).HasColumnName("CREATED");
                entity.Property(e => e.Work_Info).HasColumnName("WORK_INFO");
                entity.Property(e => e.Work_Info_Summary).HasColumnName("WORK_INFO_SUMMARY");

                entity.ToTable("TICKET_MASTER_COMMENTS");
            });
            modelBuilder.Entity<SearchDetail>().HasNoKey();

            modelBuilder.Entity<STC_Trouble_Ticket>(entity =>
            {
                entity.HasKey(x => new { x.TROUBLE_TICKETID });
                entity.Property(e => e.TROUBLE_TICKETID);
                entity.Property(e => e.PROBLEMID);
                entity.Property(e => e.PROBLEMTYPE);
                entity.Property(e => e.DESCRIPTION);
                entity.Property(e => e.CREATEDBY);
                entity.Property(e => e.CREATEDDATE);
                entity.Property(e => e.SERVICE_IMPACT_START);
                entity.Property(e => e.STATUS);
                entity.Property(e => e.SEVERITY);
                entity.Property(e => e.SERVICENUMBER);
                entity.Property(e => e.SERVICETYPE);
                entity.Property(e => e.PRODUCTCODE);
                entity.Property(e => e.TICKETCATEGORY);
                entity.Property(e => e.COMMITTIME);
                entity.Property(e => e.AREA);
                entity.Property(e => e.SUBAREA);
                entity.Property(e => e.CHANNELREF);
                entity.Property(e => e.CHANGEONT);
                entity.Property(e => e.FIRST_NAME);
                entity.Property(e => e.LAST_NAME);
                entity.Property(e => e.MOBILE);
                entity.Property(e => e.EMAIL);
                entity.Property(e => e.CUSTOMER_PRIORITY);
                entity.Property(e => e.FUNCTION_CODE);
                entity.Property(e => e.CAUSE_CODE);
                entity.Property(e => e.SOLUTION_CODE);
                entity.Property(e => e.MEMO_TEXT);
                entity.Property(e => e.SERVICE_ACCOUNT_NUMBER);
                entity.Property(e => e.TYPE);
                entity.Property(e => e.SUBTYPE);
                entity.Property(e => e.ACCOUNTID);
                entity.Property(e => e.SUBSCRIBERID);
                entity.Property(e => e.IDTYPE);
                entity.Property(e => e.IDNUMBER);
                entity.ToTable("TROUBLE_TICKET");
            });
        }
    }
}
