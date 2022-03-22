using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.Model;

namespace EStore.BuinessLayer.EStore
{
    public interface IEStoreBusinessLayer
    {
        Task<BuyEVoucherRespModel> BuyEVoucher(BuyEVoucherRequestModel obj);
        Task<GetVoucherRespModel> GetVoucherList(GetVoucherRequestModel obj);
    }
}
