using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class VMModels
    {
    }
    public class VoucherInfoWithExpireDate
    {
        public string PromoCode { get; set; }
        public byte[] QRCode { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
