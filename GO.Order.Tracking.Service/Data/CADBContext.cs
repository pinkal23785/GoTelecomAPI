using GO.Order.Tracking.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace GO.Order.Tracking.Service.Data
{
    public class CADBContext : DbContext
    {

        public DbSet<OrderTrackingPoint> OrderTrackingPoints { get; set; }
        public DbSet<MileStone> MileStones { get; set; }
        public DbSet<ExchangeNote> ExchangeNotes { get; set; }
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
            modelBuilder.Entity<OrderTrackingPoint>().HasNoKey();
            modelBuilder.Entity<MileStone>().HasNoKey();
            modelBuilder.Entity<ExchangeNote>().HasNoKey();
        }
    }
}

