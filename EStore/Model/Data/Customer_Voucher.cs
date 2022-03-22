using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    [Table("customer_voucher")]
    public class customer_voucher
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public int VoucherID { get; set; }
        public int IsUsed { get; set; }
        public DateTime UsedDate { get; set; }
        [MaxLength(11)]
        public string PromoCode { get; set; }
        public byte[] QRCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
