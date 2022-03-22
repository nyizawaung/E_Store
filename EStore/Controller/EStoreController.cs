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
    [Route("api/[controller]")]
    [ApiController]
    public class EStoreController : ControllerBase
    {
        private readonly IEStoreBusinessLayer eStoreBusinessLayer;
        public EStoreController(IEStoreBusinessLayer _estoreBusinessLayer)
        {
            eStoreBusinessLayer = _estoreBusinessLayer;
        }        
        [HttpPost("BuyEVoucher")]
        [SessionFilter]
        public async Task<IActionResult> BuyEVoucher(BuyEVoucherRequestModel obj)
        {
            if(obj.PaymentType != "Cash")
            {
                if(string.IsNullOrEmpty(obj.CardNumber))
                {
                    return BadRequest("Required CardNumber");
                }
            }
            var result = await eStoreBusinessLayer.BuyEVoucher(obj);
            return Ok(result);
        }
    }
}
