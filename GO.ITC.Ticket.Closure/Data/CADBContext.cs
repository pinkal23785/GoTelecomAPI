using GO.ITC.Ticket.Closure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GO.ITC.Ticket.Closure.Data
{
    public class CADBContext : DbContext
    {
        public DbSet<TicketMaster> TicketMasters { get; set; }
        public DbSet<CurrentOracleDateTime> CurrentOracleDateTimes { get; set; }
        public CADBContext(DbContextOptions<CADBContext> options)
        : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
            modelBuilder.Entity<CurrentOracleDateTime>().HasNoKey();
        }
    }
}
