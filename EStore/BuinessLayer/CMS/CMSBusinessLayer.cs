using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EStore.Model;
using EStore.Services;
using Microsoft.Extensions.Configuration;
using EStore.Helper;

namespace EStore.BuinessLayer.CMS
{
    public class CMSBusinessLayer : ICMSBusinessLayer
    {
        private ESDBContext dbContext;
        private ITokenService tokenService;
        private IConfiguration configuration;
        public CMSBusinessLayer(ESDBContext context, ITokenService tokenService,IConfiguration config)
        {
            dbContext = context;
            this.tokenService = tokenService;
            configuration = config;
        }
        public async Task<CMSRespModel> CreateVoucher(CreateVoucherRequestModel obj)
        {
            var respModel = new CMSRespModel();
            #region check JWT
            if (!tokenService.IsTokenValid(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), Helper.Helper.DecryptString(obj.SessionID)))
            {
                respModel.status = "Please try to login again!";
                respModel.reason = "Logg outed";
                return respModel;
            }
            #endregion   
            if (IsExist(obj.Title))
            {
                respModel.status = "Fail";
                respModel.reason = "Voucher Title already existed";
                return respModel;
            }
            var voucher = new voucher()
            {
                Amount = obj.Amount,
                Title = obj.Title,
                Buy_Type = obj.Buy_Type,
                CreatedDate = DateTime.Now,
                Description = obj.Description,
                Expiry_Date = obj.Expiry_Date,
                Image = Convert.FromBase64String(obj.Image),
                IsActive = 1,
                MaximumVoucherPerUser = obj.MaximumVoucherPerUser,
                PaymentMethod = obj.Payment_Method,
                Discount = obj.Payment_Method_Discount,
                Price = obj.Price,
                Quantity = obj.Quantity
            };
            await dbContext.AddAsync<voucher>(voucher);
            await dbContext.SaveChangesAsync();
            dbContext.Dispose();
            respModel.status = "success";
            respModel.status = "Successfully created";
            return respModel;
        }

        public async Task<CMSRespModel> UpdateVoucher(EditVoucherRequestModel obj)
        {
            var respModel = new CMSRespModel();
            #region check JWT
            if (!tokenService.IsTokenValid(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), Helper.Helper.DecryptString(obj.SessionID)))
            {
                respModel.status = "Please try to login again!";
                return respModel;
            }
            #endregion   
            if (IsExist(obj.Title, obj.VoucherID))
            {
                respModel.status = "Fail";
                respModel.reason = "Voucher Title already existed";
                return respModel;
            }
            var voucher = dbContext.Vouchers.Where(a => a.ID == obj.VoucherID).FirstOrDefault();
            if (voucher == null)
            {
                respModel.status = "Fail";
                respModel.reason = "Voucher ID does not exist.";
                return respModel;
            }
            voucher.ID = obj.VoucherID;
            voucher.Amount = obj.Amount;
            voucher.Title = obj.Title;
            voucher.Buy_Type = obj.Buy_Type;
            voucher.CreatedDate = obj.CreatedDate;
            voucher.Description = obj.Description;
            voucher.Expiry_Date = obj.Expiry_Date;
            voucher.Image = Convert.FromBase64String(obj.Image);
            voucher.IsActive = obj.IsActive;
            voucher.MaximumVoucherPerUser = obj.MaximumVoucherPerUser;
            voucher.PaymentMethod = obj.Payment_Method;
            voucher.Discount = obj.Payment_Method_Discount;
            voucher.Price = obj.Price;
            voucher.Quantity = obj.Quantity;
            await dbContext.SaveChangesAsync();
            dbContext.Dispose();
            return respModel;
        }
        public async Task<CMSVoucherRespModel> GetVoucherList(GetCMSVoucherRequestModel obj)
        {
            var respModel = new CMSVoucherRespModel();
            #region check JWT
            if (!tokenService.IsTokenValid(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), Helper.Helper.DecryptString(obj.SessionID)))
            {
                respModel.status = "Fail";
                respModel.RespDescription = "Please try to login again!";
                return respModel;
            }
            #endregion
            Expression<Func<voucher, bool>> voucherTypeFilter = x => true;
            if (obj.VoucherID > 0)
            {
                voucherTypeFilter = x => x.ID == obj.VoucherID;
            }
            var voucherList = dbContext.Vouchers.Where(a => a.IsActive == 1 && a.Expiry_Date >= DateTime.Now).Where(voucherTypeFilter).ToList();
            respModel.totalCount = voucherList.Count();
            respModel.vouchers = voucherList;
            respModel.status = "success";
            respModel.RespDescription = "success";
            dbContext.Dispose();
            return respModel;
        }

        private bool IsExist(string title)
        {
            return dbContext.Vouchers.Count(a => a.Title == title) > 0 ? true : false;
        }
        private bool IsExist(string title, int id)
        {
            return dbContext.Vouchers.Count(a => a.Title == title && a.ID != id) > 0 ? true : false;
        }
    }
}
