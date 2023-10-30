using GoStay.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace goStayCore.CommonCode
{
    public class AdminAuthorization : ActionFilterAttribute
    {
        private readonly ISessionManager _SessionManag;
        public AdminAuthorization(ISessionManager sessionManag)
        {
            _SessionManag = sessionManag;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            User? usr = new User();

            usr = _SessionManag.GetLoginAdminFromSessionAdmin();


            //--if teacher session null, then redirect to admin login page
            if (usr == null || usr.UserId < 1)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "AdminAccount",
                    action = "Login",
                    area = "Admin"
                }));

            }
            base.OnActionExecuting(filterContext);
        }

    }
}