
using Go.Web.ShareHolders.Data.Entities;
using GO.Web.ShareHolders.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Web.ShareHolders.Data
{
    public class CADBContext : DbContext
    {

        public DbSet<CustomerOTP> CustomerOTPs { get; set; }
        public DbSet<ShareHolderDetails> ShareHolderDetails { get; set; }
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

            modelBuilder.Entity<ShareHolderDetails>(entity =>
            {
                entity.HasKey(e => e.ID_Number);
                entity.Property(e => e.ID_Number).HasColumnName("ID_NUMBER");
                entity.Property(e => e.FullName).HasColumnName("FULL_NAME");
                entity.Property(e => e.Address).HasColumnName("ADDRESS");
                entity.Property(e => e.City).HasColumnName("CITY");
                entity.Property(e => e.POBOX).HasColumnName("POBOX");
                entity.Property(e => e.ZipCode).HasColumnName("ZIP_CODE");
                entity.Property(e => e.MobileNumber).HasColumnName("MOBLIE_NUMBER");
                entity.Property(e => e.Email).HasColumnName("EMAIL");
                entity.Property(e => e.CTimeStamp).HasColumnName("CTIME_STAMP");
                entity.Property(e => e.Status).HasColumnName("STATUS");
                entity.ToTable("SHAREHOLDER_DETAILS");
            });
        }
    }
}

