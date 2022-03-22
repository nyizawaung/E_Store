using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.Model;
using EStore.BuinessLayer.CMS;

namespace EStore.Controller
{
    [ApiController]
    public class CMSController : ControllerBase
    {
        ICMSBusinessLayer cmsBusinessLayer;
        public CMSController(ICMSBusinessLayer cms)
        {
            cmsBusinessLayer = cms;
        }

        [Route("api/CMS/CreateVoucher")]
        [HttpPost]
        public async Task<IActionResult> CreateVoucher(CreateVoucherRequestModel obj)
        {
            var result = new CMSRespModel();
            try
            {
                result = await cmsBusinessLayer.CreateVoucher(obj);
            }
            catch(Exception ex)
            {
                result.status = "Fail";
                result.reason = ex.Message;
            }
            return Ok(result);
        }
        [Route("api/CMS/UpdateVoucher")]
        [HttpPost]
        public async Task<IActionResult> UpdateVoucher(EditVoucherRequestModel obj)
        {
            var result = new CMSRespModel();
            try
            {
                result = await cmsBusinessLayer.UpdateVoucher(obj);
            }
            catch (Exception ex)
            {
                result.status = "Fail";
                result.reason = ex.Message;
            }
            return Ok(result);
        }
        [Route("api/CMS/GetVoucher")]
        [HttpPost]
        public async Task<IActionResult> VoucherList(GetCMSVoucherRequestModel obj)
        {
            var error = "";
            var respModel = new CMSVoucherRespModel();
            try
            {
                respModel = await cmsBusinessLayer.GetVoucherList(obj);
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
