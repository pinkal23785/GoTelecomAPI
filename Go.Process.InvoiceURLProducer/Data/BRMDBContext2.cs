using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Process.InvoiceURLProducer.Data
{
    public class BRMDBContext2 : DbContext
    {
        public BRMDBContext2(DbContextOptions<BRMDBContext2> options)
        : base(options)
        {

        }
    }
}
