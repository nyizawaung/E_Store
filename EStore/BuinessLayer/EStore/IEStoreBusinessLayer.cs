using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.Model;

namespace EStore.BuinessLayer.EStore
{
    public interface IEStoreBusinessLayer
    {
        Task<LoginRespModel> Login(LoginRequestModel obj);
        Task<BuyEVoucherRespModel> BuyEVoucher(BuyEVoucherRequestModel obj);
        Task<VoucherListRespModel> GetVoucherList(GetVoucherRequestModel obj);
        Task<GetPromoCodeRespModel> GetPromoCodeList(GetPromoCodeRequestModel obj);
        Task<ItemListRespModel> GetItemList(ItemListRequestModel obj);
        Task<ItemBuyRespModel> BuyItem(ItemBuyRequestModel obj);
    }
}
