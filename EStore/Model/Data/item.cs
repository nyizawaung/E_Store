using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model.Data
{
    public class item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int IsActive { get; set; }
        public decimal Price { get; set; }
    }
}
