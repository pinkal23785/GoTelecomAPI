using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Text;

namespace GO.Process.EInvoiceProducer.Data
{
    public class CADBContext : DbContext
    {
        public DbSet<SearchOrders> SearchOrders { get; set; }
        public DbSet<EInvoice_Audit> EInvoiceAudit { get; set; }
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
            modelBuilder.Entity<SearchOrders>().HasNoKey();

            modelBuilder.Entity<EInvoice_Audit>(entity =>
            {
                entity.HasKey(e => e.ORDER_ID);
                entity.Property(e => e.ORDER_ID);
                entity.Property(e => e.ACCOUNT_ID);
                entity.Property(e => e.CUSTOMER_NAME);
                entity.Property(e => e.CUSTOMER_NAME_AR);
                entity.Property(e => e.MOBILE);
                entity.Property(e => e.EMAIL);
                entity.Property(e => e.PAYMENT_AMT);
                entity.Property(e => e.PAYMENT_METHOD);
                entity.Property(e => e.TAX_AMT);
                entity.Property(e => e.SUBSCRIBER_ID);
                entity.Property(e => e.SMS_LINK);
                entity.Property(e => e.SMS_STATUS);
                entity.Property(e => e.SMS_TIME);
                // entity.Property(e => e.EMAIL_STATUS);


                entity.Property(e => e.LANGUAGE);
                entity.Property(e => e.SUBSCRIBER_ID);
                entity.Property(e => e.ORDER_TYPE);
                entity.Property(e => e.INVOICEDATE);

                entity.ToTable("EINVOICE_AUDIT_TBL");
            });
        }
    }
}
