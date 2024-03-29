using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Web.Invoices.Data
{
    public class BRMDBContext2 : DbContext
    {
        public BRMDBContext2(DbContextOptions<BRMDBContext2> options)
        : base(options)
        {

        }
    }
}
