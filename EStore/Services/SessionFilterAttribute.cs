//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EStore.Services
//{
//    public class SessionFilter :ActionFilterAttribute
//    {
//        ITokenService tokenService;
//        IConfiguration config;
//        public SessionFilter(ITokenService token)
//        {
//            tokenService = token;
//        }
//        public override void OnActionExecuting(ActionExecutingContext context)
//        {
//            var session = context.HttpContext.Session;
//            var isValid = tokenService.IsTokenValid()
//            base.OnActionExecuting(context);
//        }
//    }
//}
