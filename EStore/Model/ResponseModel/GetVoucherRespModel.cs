using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class GetVoucherRespModel
    {
        public int totalCount { get; set; }
        public List<VoucherInfoRespModel> voucherList { get; set; }
    }
    public class VoucherInfoRespModel
    {
        public string promoCode { get; set; }
        public byte[] QRCode { get; set; }
        public DateTime ExpiryDate{get;set;}
    }
}
