using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    [Table("admin")]
    public class admin
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Session { get; set; }
        public Nullable<DateTime> SessionEndTime { get; set; }
        public int? IsActive { get; set; }
        public string Role { get; set; }
    }
}
