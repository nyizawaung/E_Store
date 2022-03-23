using EStore.Model;
using EStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.BuinessLayer.EStore;
using Microsoft.AspNetCore.Http.Extensions;
using System.Threading;

namespace EStore.Controller
{
    [ApiController]
    public class EStoreController : ControllerBase
    {
        private readonly IEStoreBusinessLayer eStoreBusinessLayer;
        private readonly ILogServices logServices;
        
        public EStoreController(IEStoreBusinessLayer _estoreBusinessLayer,ILogServices log)
        {
            eStoreBusinessLayer = _estoreBusinessLayer;
            logServices = log;
        }        
        [HttpPost]
        [Route("api/EStore/Login")]
        public async Task<IActionResult> Login(LoginRequestModel obj)
        {
            var error = "";
            var respModel = new LoginRespModel();
            try
            {
                respModel = await eStoreBusinessLayer.Login(obj);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                respModel.RespDescription = ex.Message;
            }
            finally {
                logServices.Logging(obj, respModel, this.Request.GetDisplayUrl(), 0);
                    }

            return Ok(respModel);
        }

        [Route("api/EStore/BuyEVoucher")]
        [HttpPost]
        public async Task<IActionResult> BuyEVoucher(BuyEVoucherRequestModel obj)
        {
            if (obj.PaymentType != "Cash")
            {
                if(string.IsNullOrEmpty(obj.CardNumber))
                {
                    return BadRequest("Required CardNumber");
                }
            }
            var respModel = new BuyEVoucherRespModel();
            try
            {
                respModel = await eStoreBusinessLayer.BuyEVoucher(obj);
            }
            catch (Exception ex)
            {
                respModel.status = ex.Message;
            }
            finally
            {
                logServices.Logging(obj, respModel, this.Request.GetDisplayUrl(), obj.UserID);
            }

            var result = await eStoreBusinessLayer.BuyEVoucher(obj);
            return Ok(result);
        }

        [Route("api/EStore/PromoPurchaseHistory")]
        [HttpPost]
        public async Task<IActionResult> PromoPurchaseHistory(GetPromoCodeRequestModel obj)
        {   
            var respModel = new GetPromoCodeRespModel();
            try
            {
                respModel = await eStoreBusinessLayer.GetPromoCodeList(obj);
            }
            catch(Exception ex)
            {
                respModel.RespDescription = ex.Message;
            }
            finally
            {
                logServices.Logging(obj, new { Result = respModel.RespDescription }, this.Request.GetDisplayUrl(), obj.UserID);
            }
            return Ok(respModel);
        }
        [Route("api/EStore/VoucherList")]
        [HttpPost]
        public async Task<IActionResult> VoucherList(GetVoucherRequestModel obj)
        { 
            var respModel = new VoucherListRespModel();
            try
            {
                respModel = await eStoreBusinessLayer.GetVoucherList(obj);
            }
            catch (Exception ex)
            {
                respModel.RespDescription = ex.Message;
            }
            finally
            {
                logServices.Logging(obj, new { Result = respModel.RespDescription }, this.Request.GetDisplayUrl(), obj.UserID);
            }
            return Ok(respModel);
        }
        [Route("api/EStore/Items")]
        [HttpPost]
        public async Task<IActionResult> itemList(ItemListRequestModel obj)
        {
            var respModel = new ItemListRespModel();
            try
            {
                respModel = await eStoreBusinessLayer.GetItemList(obj);
            }
            catch (Exception ex)
            {
                respModel.RespDescription = ex.Message;
            }
            finally
            {
                logServices.Logging(obj, respModel, this.Request.GetDisplayUrl(), obj.UserID);
            }
            return Ok(respModel);
        }

        [Route("api/EStore/BuyItems")]
        [HttpPost]
        public async Task<IActionResult> buyItems(ItemBuyRequestModel obj)
        {
            var respModel = new ItemBuyRespModel();
            try
            {
                respModel = await eStoreBusinessLayer.BuyItem(obj);
            }
            catch (Exception ex)
            {
                respModel.RespDescription = ex.Message;
            }
            finally
            {
                logServices.Logging(obj, respModel, this.Request.GetDisplayUrl(), obj.UserID);
            }
            return Ok(respModel);
        }

        [Route("api/EStore/PaymentMethod")]
        [HttpPost]
        public async Task<IActionResult> PaymentMethod(PaymentMethodRequestModel obj)
        {
            var error = "";
            var respModel = new PaymentMethodRespModel();
            try
            {
                respModel = await eStoreBusinessLayer.GetPaymentMethod(obj);
            }
            catch (Exception ex)
            {
                respModel.RespDescription = ex.Message;
            }
            finally
            {
                logServices.Logging(obj, respModel, this.Request.GetDisplayUrl(), obj.UserID);
            }
            return Ok(respModel);
        }

        [Route("api/EStore/ItemPurchaseHistory")]
        [HttpPost]
        public async Task<IActionResult> ItemPurchaseHistory(ItemPurchaseHistoryRequestModel obj)
        {
            var respModel = new ItemPurchaseHistoryRespModel();
            try
            {
                respModel = await eStoreBusinessLayer.ItemPurchaseHistory(obj);
            }
            catch (Exception ex)
            {
                respModel.RespDescription = ex.Message;
            }
            finally
            {
                logServices.Logging(obj, new { Result = respModel.RespDescription } this.Request.GetDisplayUrl(), obj.UserID);
            }
            return Ok(respModel);
        }
    }

}
