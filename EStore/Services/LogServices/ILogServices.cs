using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Services
{
    public interface ILogServices
    {
        void Logging(dynamic request, dynamic response,string RequestPath,int UserID);
    }
}
