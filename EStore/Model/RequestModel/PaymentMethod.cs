using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class PaymentMethodRequestModel
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public string SessionID { get; set; }
    }
    public class PaymentMethodRespModel
    {
        public string status { get; set; }
        public string RespDescription { get; set; }
        public List<paymentmethod> paymentmethods { get; set; }
    }
}
