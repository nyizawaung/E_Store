using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class GetVoucherRequestModel
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public string SessionID { get; set; }
        public string VoucherType { get; set; } = "all";
    }
}
