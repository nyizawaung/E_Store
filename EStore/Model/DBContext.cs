using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EStore.Model
{
    public class ESDBContext:DbContext
    {
        public ESDBContext()
        {

        }
        public ESDBContext(DbContextOptions<ESDBContext> options) : base(options){

        }
        public DbSet<voucher> Vouchers { get; set; }
        public DbSet<customer_voucher> Customer_Vouchers { get; set; }
        public DbSet<admin> Admins { get; set; }
    }
}
