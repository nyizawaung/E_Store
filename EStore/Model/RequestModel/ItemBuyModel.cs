using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class ItemBuyRequestModel
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public string SessionID { get; set; }
        public string UserName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int ItemID { get; set; }
        public int qty { get; set; }
        public string cardNumber { get; set; }
        public string promoCode { get; set; }
    }
    public class ItemBuyRespModel
    {
        public string status { get; set; }
        public string RespDescription { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal NetAmount { get; set; }
    }

    public class ItemListRequestModel
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public string SessionID { get; set; }
    }
    public class ItemListRespModel
    {
        public string status { get; set; }
        public string RespDescription { get; set; }
        public List<ItemInfo> items { get; set; }
        public int totalCount { get; set; }
    }
    public class ItemInfo
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
