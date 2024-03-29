using Go.Service.NumberBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Go.Service.NumberBooking.Data
{
    public class NBSContext : DbContext
    {

        public DbSet<SPNNumber> SPNNumbers { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<GTSCategory> GTSCategories { get; set; }
        public DbSet<GTSNumberRange> GTSNumberRanges { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public NBSContext(DbContextOptions<NBSContext> options)
         : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder modelBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SPNNumber>(entity =>
            {
                entity.HasKey(x => x.SPN_Id);
                entity.Property(e => e.SPN_Id);

                entity.Property(e => e.SPN_Cat_Id);
                entity.Property(e => e.SPN_Number);
                entity.Property(e => e.SPN_Status);
                entity.Property(e => e.SPN_Date_Booked);
                entity.Property(e => e.SPN_Date_Expiry);
                entity.Property(e => e.SPN_Customer_Id);
                entity.Property(e => e.SPN_AM_Email);
                entity.Property(e => e.SPN_Account_Manager);
                entity.Property(e => e.SPN_Number_Type);
                entity.Property(e => e.SPN_Request_Validity);
                entity.Property(e => e.SPN_Order_number);
                entity.Property(e => e.SPN_Action_Taken);
                entity.Property(e => e.SPN_Modified_By);
                entity.Property(e => e.SPN_NetworkStatus);
                entity.Property(e => e.SPN_Cancel_Date);
                entity.Property(e => e.SPN_Action_Update);
                entity.ToTable("SPN_Numbers");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Cat_Id);
                entity.Property(e => e.Cat_Id);

                entity.Property(e => e.Cat_Name);
                entity.ToTable("Categories");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(x => x.Customer_Id);
                entity.Property(e => e.Customer_Id);

                entity.Property(e => e.Customer_Company);
                entity.Property(e => e.Customer_Name);
                entity.Property(e => e.Customer_Email);
                entity.Property(e => e.Customer_Address);
                entity.Property(e => e.Customer_City);
                entity.Property(e => e.Customer_Tel);
                entity.Property(e => e.Customer_Mobile);
                entity.ToTable("Customers");
            });

            modelBuilder.Entity<GTSCategory>(entity =>
            {
                entity.HasKey(x => x.GTS_CAT_ID);
                entity.Property(e => e.GTS_CAT_ID);

                entity.Property(e => e.GTS_CATEGORY).UseCollation("SQL_Latin1_General_CI_AI");
                entity.ToTable("GTS_Categories");
            });

            modelBuilder.Entity<GTSNumberRange>(entity =>
            {
                entity.HasKey(x => x.Range_GTS_Id);
                entity.Property(e => e.Range_GTS_Id);

                entity.Property(e => e.Range_GTS_CATID);
                entity.Property(e => e.Range_GTS_CityID);
                entity.Property(e => e.Range_GTS_Number_From);
                entity.Property(e => e.Range_GTS_Number_To);
                entity.Property(e => e.Range_GTS_Status);
                entity.Property(e => e.Range_GTS_Date_Booked);
                entity.Property(e => e.Range_GTS_Date_Expiry);
                entity.Property(e => e.Range_Release_By);
                entity.Property(e => e.Range_Release_On);
                entity.Property(e => e.Range_Release_Type);
                entity.Property(e => e.Range_GTS_Account_Manager);
                entity.Property(e => e.Range_GTS_AM_Email);
                entity.Property(e => e.Range_GTS_Customer_Id);
                entity.Property(e => e.Range_GTS_Approved_by);
                entity.Property(e => e.Range_GTS_Approved_On);
                entity.Property(e => e.Range_GTS_Consumed_by);
                entity.Property(e => e.Range_GTS_Consumed_On);
                entity.Property(e => e.Range_GTS_Network_Status);
                entity.Property(e => e.Range_GTS_Cancel_On);
                entity.Property(e => e.Range_GTS_Prov_NonProv);
                entity.ToTable("GTS_Numbers_Range");
            });
        }
    }
}
