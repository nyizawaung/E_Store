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
        public ESDBContext(DbContextOptions<ESDBContext> options) : base(options){

        }
        public DbSet<voucher> Vouchers { get; set; }
        public DbSet<customer_voucher> Customer_Vouchers { get; set; }
        public DbSet<admin> Admins { get; set; }
        public DbSet<item> Items { get; set; }
        public DbSet<item_purchase_history> Item_Purchase_Histories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){

            base.OnModelCreating(modelBuilder);
        }
    }
}
