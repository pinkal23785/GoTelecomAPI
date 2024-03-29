using Go.FtthCollection.Service.Data.Entities;
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

        public DbSet<AccountDetail> AccountDetails { get; set; }
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
            modelBuilder.Entity<AccountDetail>().HasNoKey();
           // modelBuilder.Entity<AccountDetail>().Property(p => p.LAST_INVOICE_DT).HasPrecision(6);
        }
    }
}

