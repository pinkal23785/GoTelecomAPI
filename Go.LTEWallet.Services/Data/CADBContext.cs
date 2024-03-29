using Go.LTEWallet.Services.Data.Entities;
using Go.LTEWallet.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data
{
    public class CADBContext : DbContext
    {

        public DbSet<CommissionMerchantUser> CommissionMerchantUsers { get; set; }
        public DbSet<CommissionWalletBalance> CommissionWalletBalances { get; set; }
        public DbSet<PlanDetail> PlanDetails { get; set; }
        public DbSet<CommissionPlan> CommissionPlans { get; set; }

        public DbSet<PlanVoucher> PlanVouchers { get; set; }

        public DbSet<RedeemStatus> RedeemStatuses { get; set; }

        public DbSet<VoucherDetail> VoucherDetails { get; set; }

        public DbSet<RenewalOrderLogs> RenewalOrders { get; set; }

        public DbSet<CustomerOTP> CustomerOTPs { get; set; }

        public DbSet<OrderSummary> OrderSummaries { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
        public CADBContext(DbContextOptions<CADBContext> options)
          : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //            CADDB - Database:
            //Host Name:172.16.68.34
            //Port: 1521
            //SID: caddb
            // User Name / Password:cadb_bsim3 / cadb_bsim3
            //optionsBuilder.UseOracle(@"User Id=cadb_bsim3;Password=cadb_bsim3;Data Source=172.16.68.34:1521/caddb;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommissionMerchantUser>(entity =>
            {
                entity.HasKey(x=>new { x.MERCHANTID });
                entity.Property(e => e.MOBILENUMBER);
                entity.Property(e => e.MERCHANTID);
                entity.Property(e => e.FULLNAME);
                entity.Property(e => e.SALESDEVELOPERCODE);
                entity.Property(e => e.CREATED);
                entity.Property(e => e.EMAIL);
                entity.Property(e => e.STATUS);
                entity.Property(e => e.FAILEDLOGINATTEMPTS);
                entity.Property(e => e.LASTLOGINATTEMPT);
                entity.Property(e => e.ISMOBILEVALID);
                entity.Property(e => e.PREFERREDLANGUAGE);
                entity.Property(e => e.STORENAME);
                entity.Property(e => e.CITY);
                entity.ToTable("COMMISSION_MERCHANT_USERS");
            });
            modelBuilder.Entity<CommissionWalletBalance>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID);
                //entity.Property(e => e.USER_ID1);
                entity.Property(e => e.USER_FULL_NAME_EN);
                entity.Property(e => e.USER_FULL_NAME_AR);
                entity.Property(e => e.WALLET_BALANCE);
                entity.Property(e => e.INSERT_DATE);
                entity.Property(e => e.INSERT_BY);
                entity.Property(e => e.LAST_MODIFY_DATE);
                entity.Property(e => e.LAST_MODIFY_BY);
                entity.Property(e => e.LAST_TRANS_ID);
                entity.Property(e => e.SALESDEVELOPERCODE);
                entity.Property(e => e.STORENAME);
                entity.Property(e => e.USER_ID);
                entity.Property(e => e.MERCHANTID);
                entity.ToTable("COMMISSION_WALLET_BALANCE");
            });

            modelBuilder.Entity<CustomerOTP>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.ID);
                entity.Property(e => e.MOBILENUMBER);
                entity.Property(e => e.OTP);
                entity.Property(e => e.DATE_CREATED);
                entity.Property(e => e.OTP_EXPIRE_DATE);
                entity.Property(e => e.IS_VERIFIED);
                entity.ToTable("DEVICE_EXTENDER_OTP_TBL");
            });

            modelBuilder.Entity<PlanDetail>().HasNoKey();
            modelBuilder.Entity<CommissionPlan>().HasNoKey();
            modelBuilder.Entity<PlanVoucher>().HasNoKey();

            modelBuilder.Entity<RedeemStatus>().HasNoKey();
            modelBuilder.Entity<VoucherDetail>().HasNoKey();


            modelBuilder.Entity<RenewalOrderLogs>().HasNoKey();

            modelBuilder.Entity<OrderSummary>().HasNoKey();
            modelBuilder.Entity<OrderDetail>().HasNoKey();
        }
    }
}

