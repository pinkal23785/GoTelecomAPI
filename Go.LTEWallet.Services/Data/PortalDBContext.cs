using Go.LTEWallet.Services.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data
{
    public class PortalDBContext : DbContext
    {
        public DbSet<HotLTEMerchant> HotLTEMerchants { get; set; }
        public PortalDBContext(DbContextOptions<PortalDBContext> options)
         : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder modelBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HotLTEMerchant>(entity =>
            {
                entity.HasKey(e => e.MerchantId);

                entity.Property(e => e.MerchantId);
                entity.Property(e => e.MobileNumber);
                entity.Property(e => e.FullName);
                entity.Property(e => e.Created);
                entity.Property(e => e.Email);
                entity.Property(e => e.Status);
                entity.Property(e => e.FailedLoginAttempts);
                entity.Property(e => e.LastLoginAttempt);
                entity.Property(e => e.IsMobileValid);
                entity.Property(e => e.PreferredLanguage);
                entity.Property(e => e.StoreName);
                entity.Property(e => e.City);
                entity.Property(e => e.SalesDeveloperCode);
                entity.ToTable("HotLTEMerchant");
            });

        }
    }
}
