using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class ESDBContext:DbContext
    {
        public ESDBContext(DbContextOptions<ESDBContext> options) : base(options){

        }
    }
}
