using Go.Process.InvoiceURLProducer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Process.InvoiceURLProducer.Data
{
    public class BRMContext : DbContext
    {
        public DbSet<CustomerInvoice> CustomerInvoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<SMS_NOT_SEND_ACCOUNTS> NOT_SEND_ACCOUNTS { get; set; }
        public BRMContext(DbContextOptions<BRMContext> options)
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
                entity.Property(e => e.IS_ORDERED);
                entity.Property(e => e.PLAN_ID);
                entity.ToTable("TEMP_INVOICE_UPDATE_MAIN_T");
            });
            modelBuilder.Entity<SMS_NOT_SEND_ACCOUNTS>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.ACCOUNT_NO);
                
                entity.ToTable("SMS_NOT_SEND_ACCOUNTS");
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.ORDER_ID);
                entity.Property(e => e.ORDER_ID);
                entity.Property(e => e.ORDER_TYPE);
                entity.Property(e => e.ORDER_CREATION_DATE);
                entity.Property(e => e.ORDER_START_DATE);
                entity.Property(e => e.ORDER_EXPECTED_COMPL_DATE);
                entity.Property(e => e.ORDER_ACT_COMPL_DATE);
                entity.Property(e => e.ORDER_RFS_DATE);
                entity.Property(e => e.ORDER_STATUS);
                entity.Property(e => e.INSTALLATION_DATE);
                entity.Property(e => e.SUBSCRIBER_ID);
                entity.Property(e => e.PLAN_ID);
                entity.Property(e => e.AIDED_UNAIDEDN_FLAG);
                entity.Property(e => e.CHANNEL_ID);
                entity.Property(e => e.CHANNEL_TYPE);
                entity.Property(e => e.PYMT_METHOD);
                entity.Property(e => e.PYMT_REF_NO);
                entity.Property(e => e.DEALER_CODE);
                entity.Property(e => e.ACCOUNT_ID);
                entity.Property(e => e.PAYMENT_AMOUNT);
                entity.Property(e => e.INTERACTION_ID);
                entity.Property(e => e.ORDER_SUB_STATUS);
                entity.Property(e => e.ORDER_SUB_TYPE);
                entity.Property(e => e.CREATED_BY);
                entity.Property(e => e.CONTRACT_ID);
                entity.Property(e => e.PURCHASE_FEE);
                entity.Property(e => e.DEVICE_COST);
                entity.Property(e => e.NUMBER_COST);
                entity.Property(e => e.DELIVERY_COST);
                entity.Property(e => e.MASTER_ORDER_ID);
                entity.Property(e => e.ORIGINAL_AMOUNT);
                entity.Property(e => e.DISCOUNT_PERCENTAGE);
                entity.Property(e => e.PYMT_DATE);
                entity.Property(e => e.APPROVER_OVERRIDEN);
                entity.Property(e => e.SECURITY_DEPOSIT);
                entity.Property(e => e.TAX_AMOUNT);
                entity.ToTable("ORDER_TBL");

            });


            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.INVOICE_ID);
                entity.Property(e => e.INVOICE_ID);
                entity.Property(e => e.ACCOUNT_ID);
                entity.Property(e => e.STATEMENT_DATE);
                entity.Property(e => e.BILL_AMOUNT);
                entity.Property(e => e.MIN_AMOUNT_DUE);
                entity.Property(e => e.DUE_DATE);
                entity.Property(e => e.CREATED_DATE);
                entity.Property(e => e.CREATED_BY);
                entity.Property(e => e.INVOICE_DATA_PDF);
                entity.Property(e => e.FINAL_INVOICE_XML);
                entity.Property(e => e.BILL_NO);
                entity.Property(e => e.BILL_STATUS);
                entity.Property(e => e.INVOICE_DATA_PDF_AR);
                entity.Property(e => e.FINAL_INVOICE_XML_AR);
                entity.ToTable("INVOICE_TBL");
            });
        }
    }
}
