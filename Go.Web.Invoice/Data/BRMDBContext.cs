using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Web.Invoice.Data
{
    public class BRMDBContext : DbContext
    {
        public BRMDBContext(DbContextOptions<BRMDBContext> options)
         : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
