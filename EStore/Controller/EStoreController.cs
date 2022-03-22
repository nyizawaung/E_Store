using EStore.Model;
using EStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.BuinessLayer.EStore;

namespace EStore.Controller
{
    [ApiController]
    public class EStoreController : ControllerBase
    {
        private readonly IEStoreBusinessLayer eStoreBusinessLayer;
        //private readonly ITokenService tokenService;
        public EStoreController(IEStoreBusinessLayer _estoreBusinessLayer/*,ITokenService _tokenservice*/)
        {
            eStoreBusinessLayer = _estoreBusinessLayer;
            //tokenService = _tokenservice;
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
            var result = await eStoreBusinessLayer.BuyEVoucher(obj);
            return Ok(result);
        }

        [Route("api/EStore/PurchaseHistory")]
        [HttpPost]
        public async Task<IActionResult> PurchaseHistory(GetPromoCodeRequestModel obj)
        {   
            var error = "";
            var respModel = new GetPromoCodeRespModel();
            try
            {
                respModel = await eStoreBusinessLayer.GetPromoCodeList(obj);
                respModel.RespDescription = "Success";
            }
            catch(Exception ex)
            {
                error = ex.Message;
                respModel.RespDescription = ex.Message;
            }
            return Ok(respModel);
        }
        [Route("api/EStore/VoucherList")]
        [HttpPost]
        public async Task<IActionResult> VoucherList(GetVoucherRequestModel obj)
        { 
            var error = "";
            var respModel = new VoucherListRespModel();
            try
            {
                respModel = await eStoreBusinessLayer.GetVoucherList(obj);
                respModel.RespDescription = "Success";
            }
            catch (Exception ex)
            {
                error = ex.Message;
                respModel.RespDescription = ex.Message;
            }
            return Ok(respModel);
        }
    }
}
