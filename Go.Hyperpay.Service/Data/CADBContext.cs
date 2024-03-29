using Go.Hyperpay.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Hyperpay.Service.Data
{
    public class CADBContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<PaymentTransactionStatus> PaymentTransactionStatuses { get; set; }
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
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.ID);
                entity.Property(e => e.FIRST_NAME);
                entity.Property(e => e.LAST_NAME);
                entity.Property(e => e.ID_VALUE);
                entity.Property(e => e.SUBSCRIBER_ID);
                entity.Property(e => e.STATUS);

                entity.ToTable("COURSE");
            });

            modelBuilder.Entity<PaymentTransactionStatus>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID);
                entity.Property(e => e.CHECKOUT);
                entity.Property(e => e.CLIENT_IP);
                entity.Property(e => e.CLIENT_PORT);
                entity.Property(e => e.CLIENT_CONTEXT);
                entity.Property(e => e.CLIENT_USER);
                entity.Property(e => e.CLIENT_SESSION_ID);
                entity.Property(e => e.HYPERPAY_TRANS_ID);
                entity.Property(e => e.PAYMENT_TYPE);
                entity.Property(e => e.PAYMENT_BRAND);
                entity.Property(e => e.PAYMENT_AMOUNT);
                entity.Property(e => e.PAYMENT_CURRENCY);
                entity.Property(e => e.PAYMENT_DESCRIPTOR);
                entity.Property(e => e.MERCHANT_TRANSACTION_ID);
                entity.Property(e => e.RESULT_CODE);
                entity.Property(e => e.RESULT_DESCRIPTION);
                entity.Property(e => e.CONNECTORTXID1);
                entity.Property(e => e.CARD_BIN);
                entity.Property(e => e.CARD_BIN_COUNTRY);
                entity.Property(e => e.CARD_LAST_4DIGITS);
                entity.Property(e => e.CARD_HOLDER);
                entity.Property(e => e.CARD_EXPIRY_MONTH);
                entity.Property(e => e.CARD_EXPIRY_YEAR);
                entity.Property(e => e.CUST_GIVEN_NAME);
                entity.Property(e => e.CUST_SURNAME);
                entity.Property(e => e.CUST_EMAIL);
                entity.Property(e => e.CUST_IP);
                entity.Property(e => e.CUST_COUNTRY);
                entity.Property(e => e.BILLING_STREET1);
                entity.Property(e => e.BILLING_CITY);
                entity.Property(e => e.BILLING_STATE);
                entity.Property(e => e.BILLING_POST_CODE);
                entity.Property(e => e.BILLING_COUNTRY);
                entity.Property(e => e.SHOPPER_ENDTOENDIDENTITY);
                entity.Property(e => e.CTPE_DESCRIPTOR_TEMPLATE);
                entity.Property(e => e.SCORE);
                entity.Property(e => e.BUILD_NUMBER);
                entity.Property(e => e.TRANS_TIMESTAMP);
                entity.Property(e => e.NDC);
                entity.Property(e => e.ERROR_PARAM_NAME);
                entity.Property(e => e.ERROR_PARAM_VAL);
                entity.Property(e => e.ERROR_PARAM_MSG);
                entity.Property(e => e.CLIENT_TRANSACTION_ID);
                entity.Property(e => e.TRANSACTION_TIMESTAMP);

                //entity.Property(e => e.MOBILENUMBER);
                entity.Property(e => e.MERCHANTTRANSACTIONID);
                entity.Property(e => e.ENTITYID);
                entity.Property(e => e.CARDTYPE);
                entity.Property(e => e.AMOUNT);
                entity.Property(e => e.CURRENCY);
                entity.Property(e => e.STREET);
                entity.Property(e => e.CITY);
                entity.Property(e => e.STATE);
                entity.Property(e => e.COUNTRY);
                entity.Property(e => e.POSTALCODE);
                entity.Property(e => e.GIVENNAME);
                entity.Property(e => e.SURNAME);
                entity.Property(e => e.CHECKOUTDATE);

                entity.Property(e => e.MOBILE_NUMBER);
                entity.Property(e => e.EMAIL_ID);
                entity.Property(e => e.USER_AGENT);


                entity.ToTable("HYPERPAY_PAYMENT_TRANS");
            });
        }
    }
}
