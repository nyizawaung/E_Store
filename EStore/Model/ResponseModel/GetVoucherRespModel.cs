using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class GetPromoCodeRespModel
    {
        public int totalCount { get; set; }
        public List<PromoCodeInfoRespModel> promoCodeList { get; set; }
        public string RespDescription { get; set; }
    }
    public class PromoCodeInfoRespModel
    {
        public string promoCode { get; set; }
        public string QRCode { get; set; }
        public DateTime ExpiryDate{get;set;}
        public int IsUsed { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }

    public class VoucherListRespModel
    {
        public string RespDescription { get; set; }
        public List<VoucherInfo> voucherList { get; set; }
        public int totalCount { get; set; }
    }
    public class VoucherInfo
    {
        public int VoucherID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string BuyType { get; set; }
        public Nullable<DateTime> ExpiryDate { get; set; }
        public string Image { get; set; }
    }
}
