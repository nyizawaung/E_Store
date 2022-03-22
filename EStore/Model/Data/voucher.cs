using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    [Table("voucher")]
    public class voucher
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Expiry_Date { get; set; }
        public byte[] Image { get; set; }
        public decimal Amount { get; set; }
        public string Payment_Method { get; set; }
        public int Payment_Method_Discount { get; set; }
        public int Quantity { get; set; }
        public string Buy_Type { get; set; }
        public int Is_Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public int MaximumVoucherPerUser { get; set; }
        public decimal Price { get; set; }
    }
}
