using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    [Table("item_purchase_history")]
    public class item_purchase_history
    {
        public int ID { get; set; }
        public int? ItemID { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string PromoCode { get; set; }
        public int Qty { get; set; }
        public decimal? TotalPrice { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public decimal? NetAmount { get; set; }
    }
}
