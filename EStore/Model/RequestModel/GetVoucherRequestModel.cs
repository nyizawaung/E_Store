using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class GetPromoCodeRequestModel
    {
        [Required]
        public int UserID { get; set; }
        public string SessionID { get; set; }
        public string promoType { get; set; } = "all";
    }
    public class GetVoucherRequestModel
    {
        [Required]
        public int UserID { get; set; }
        public string SessionID { get; set; }
        public int VoucherID { get; set; }
    }
}
