using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Helper
{
    public static class Validater
    {
        public static string CheckValidateToBuy(string phoneNumber,int voucherID,int qty, out bool allow)
        {
            string reason = "Not allowed";
            allow = false;
            return reason;
        }
    }
}
