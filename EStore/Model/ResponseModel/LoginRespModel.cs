using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Model
{
    public class LoginRespModel
    {
        public int UserID { get; set; }
        public string SessionID { get; set; }
        public string RespDescription { get; set; }
    }
}
