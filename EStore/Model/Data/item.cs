using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    [Table("item")]
    public class item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? IsActive { get; set; }
        public decimal? Price { get; set; }
    }
}
