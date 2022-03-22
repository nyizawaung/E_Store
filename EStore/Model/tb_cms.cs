using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class tb_cms
    {
        public int ID{get;set;}
public string Title { get; set; }
        public string Description {get;set;}
        public string Expiry_Date {get;set;}
    public byte[] Image { get; set; }
    public string Amount { get; set; }
        public string Payment_Method { get; set; }
        public int Payment_Method_Discount { get; set; }
        public int Quantity { get; set; }
        public string Buy_Type { get; set; }
        public bool Is_Active { get; set; }
    public DateTime CreatedDate { get; set; }
        public int MaximumVoucherPerUser { get; set; }
    }
}
