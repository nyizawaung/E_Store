using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class BuyEVoucherRespModel
    {
        public string status { get; set; }
        public string reason { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public List<GeneratedPromo> promoCodes { get; set; }
    }
    public class GeneratedPromo
    {
        public string PromoCode { get; set; }
        public string QRImage { get; set; }
    }
}
