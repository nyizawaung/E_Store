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
    }
}
