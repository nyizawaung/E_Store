using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class BuyEVoucherRequestModel
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public string SessionID { get; set; }
        [Required]
        public int VoucherID { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string PaymentType { get; set; }
        
        public string CardNumber { get; set; }

        public string CashAmount { get; set; }
        public string UserName { get; set; }
        [Required]
        public string PhoneNumber { get; set; } 

    }
}
