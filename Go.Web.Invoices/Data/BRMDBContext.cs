using Go.Web.Invoices.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Web.Invoices.Data
{
    public class BRMDBContext : DbContext
    {
        public DbSet<CustomerInvoice> CustomerInvoices { get; set; }
        public DbSet<EInvoice_Audit> EInvoiceAudit { get; set; }
        public BRMDBContext(DbContextOptions<BRMDBContext> options)
         : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerInvoice>(entity =>
            {
                entity.HasKey(e => e.POID_ID0);
                entity.Property(e => e.POID_ID0);
                entity.Property(e => e.ACCOUNT_NO);
                entity.Property(e => e.CUSTOMER_NAME);
                entity.Property(e => e.CUSTOMER_NAME_AR);
                entity.Property(e => e.PHONE);
                entity.Property(e => e.EMAIL_ADDR);
                entity.Property(e => e.BILL_NO);
                entity.Property(e => e.START_T);
                entity.Property(e => e.END_T);
                entity.Property(e => e.DUE_T);
                entity.Property(e => e.PAID_AMOUNT);
                entity.Property(e => e.LAST_BILL_AMT);
                entity.Property(e => e.TOTAL_DUE);
                entity.Property(e => e.CURRENT_TOTAL);
                entity.Property(e => e.PREVIOUS_TOTAL);
                entity.Property(e => e.CYCLE_TAX);
                entity.Property(e => e.CURRENT_TOTAL_WV);
                entity.Property(e => e.ACCOUNT_OBJ_ID0);
                entity.Property(e => e.PLAN_NAME);
                entity.Property(e => e.CREDIT_LIMIT);
                entity.Property(e => e.PLAN_PRICE);
                entity.Property(e => e.SMS_LINK);
                entity.Property(e => e.SMS_STATUS);
                entity.Property(e => e.SMS_TIME);
                entity.Property(e => e.EMAIL_STATUS);
                entity.Property(e => e.EMAIL_TIME);
                entity.Property(e => e.PDF_STATUS);
                entity.Property(e => e.PDF_TIME);
                entity.Property(e => e.SADAD_STATUS);
                entity.Property(e => e.SADAD_TIME);
                entity.Property(e => e.DUE);
                entity.Property(e => e.LANGUAGE);
                entity.Property(e => e.SUBSCRIBER_ID);

                entity.ToTable("TEMP_INVOICE_UPDATE_MAIN_T");
            });

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
