using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.Model;

namespace EStore.Services
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, admin user);
        bool IsTokenValid(string key, string issuer, string token,int userID=1);
    }

}
