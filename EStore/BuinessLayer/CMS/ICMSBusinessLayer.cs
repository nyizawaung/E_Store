using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.Model;

namespace EStore.BuinessLayer.CMS
{
    public interface ICMSBusinessLayer
    {
        Task<CMSRespModel> CreateVoucher(CreateVoucherRequestModel obj);
        Task<CMSRespModel> UpdateVoucher(EditVoucherRequestModel obj);
        Task<CMSVoucherRespModel> GetVoucherList(GetCMSVoucherRequestModel obj);

    }
}
