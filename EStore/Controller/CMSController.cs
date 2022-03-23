using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.Model;
using EStore.BuinessLayer.CMS;
using EStore.Services;
using Microsoft.AspNetCore.Http.Extensions;
using System.Threading;

namespace EStore.Controller
{
    [ApiController]
    public class CMSController : ControllerBase
    {
        ICMSBusinessLayer cmsBusinessLayer;
        ILogServices logServices;
        public CMSController(ICMSBusinessLayer cms,ILogServices log)
        {
            cmsBusinessLayer = cms;
            logServices = log;
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
            finally
            {
                logServices.Logging(obj, result, this.Request.GetDisplayUrl(), obj.UserID);
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
            finally
            {
                logServices.Logging(obj, result, this.Request.GetDisplayUrl(), obj.UserID);
            }
            return Ok(result);
        }
        [Route("api/CMS/GetVoucher")]
        [HttpPost]
        public async Task<IActionResult> VoucherList(GetCMSVoucherRequestModel obj)
        {
            var respModel = new CMSVoucherRespModel();
            try
            {
                respModel = await cmsBusinessLayer.GetVoucherList(obj);
                respModel.RespDescription = "Success";
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
    }
}
