using GoStay.Api.Providers;
using GoStay.DataDto.Users;
using GoStay.Service.Api.Users;
using goStayCore.CommonCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace GoStay.Api.Attributes
{
    public class RoleAttribute : Attribute, IActionFilter
    {
        public int[] Permision { get; set; }
        public RoleAttribute(params int[] permision)
        {
            Permision = permision;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // không thể khởi tạo service thông qua contructor như bình thường 
            // phải tạo ra 1 cái là provider, đây là 1 cách để lấy service đã được khởi tạo ra dùng

            var httpContext = (IHttpContextAccessor)StaticServiceProvider.Provider.GetService(typeof(IHttpContextAccessor));
            var authorityService = (IUserApiServices)httpContext.HttpContext.RequestServices.GetService(typeof(IUserApiServices));
            var _SessionManag = (ISessionManager )httpContext.HttpContext.RequestServices.GetService(typeof(ISessionManager));
            var UserId = _SessionManag.GetLoginAdminFromSessionAdmin().UserId;
            var isCheked = authorityService.CheckUserPermision(new CheckPermisionParam { Permission= Permision, UserId = UserId });

            if (!isCheked.Data)
            {
                context.Result = new JsonResult("NoPermission")
                {
                    StatusCode = 405,
                    Value = new
                    {
                        Status = "Error",
                        Message = "Sorry, You don't have permission for the acction."
                    },
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("OnActionExecuted");
        }
    }
}
