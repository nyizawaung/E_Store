using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class CMSModels
    {
    }
    public class CreateVoucherRequestModel
    {
        [Required]
        public int UserID { get; set; }
        public string SessionID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Expiry_Date { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Payment_Method { get; set; }
        [Required]
        public int Payment_Method_Discount { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Buy_Type { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public int MaximumVoucherPerUser { get; set; }
        [Required]
        public decimal Price { get; set; }
    }

    public class EditVoucherRequestModel
    {
        [Required]
        public int VoucherID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public string SessionID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Expiry_Date { get; set; }
        public string Image { get; set; }
        [Required]
        public string ImageURL { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Payment_Method { get; set; }
        [Required]
        public int Payment_Method_Discount { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Buy_Type { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public int MaximumVoucherPerUser { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int IsActive { get; set; }
    }

    public class GetCMSVoucherRequestModel
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public string SessionID { get; set; }
        public int VoucherID { get; set; }
    }
    public class CMSVoucherRespModel
    {
        public string status { get; set; }
        public string RespDescription { get; set; }
        public List<voucher> vouchers { get; set; }
        public int totalCount { get; set; }
    }

    public class CMSRespModel
    {
        public string status { get; set; }
        public string reason { get; set; }
    }
}
