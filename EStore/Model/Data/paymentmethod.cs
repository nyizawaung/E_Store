using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    [Table("paymentmethod")]
    public class paymentmethod
    {
        public int ID { get; set; }
        public string PaymentName { get; set; }
        public int IsActive { get; set; }
    }
}
