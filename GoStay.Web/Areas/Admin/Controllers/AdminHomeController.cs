using DAL.Entities;
using DAL.Helpers.PageHelpers;
using Microsoft.AspNetCore.Mvc;
using goStayCore.CommonCode;
using GoStay.Service;
using GoStay.Service.Api.Hotels;
using GoStay.DataDto.Statistical;
using GoStay.Service.Api.Users;
using GoStay.DataAccess.Entities;
using GoStay.Common.Configuration;

namespace GoStay.Web.Areas.Admin.Controllers
{

    [ServiceFilter(typeof(AdminAuthorization))]
    [Area("Admin")]
    public class AdminHomeController : Controller
    {
        private readonly IConstants _constants;
        private readonly ISessionManager _SessionManag;
        private readonly IAdminServices _adminServices;
        private readonly IUserApiServices _userServices;

        private readonly IStatisticalApiServices _statisticalApiServices;
        public AdminHomeController( IConstants constants , ISessionManager sessionManag, IAdminServices adminServices, 
            IStatisticalApiServices statisticalApiServices, IUserApiServices userServices)
        {
            this._constants = constants;
            _SessionManag = sessionManag;
            this._adminServices = adminServices;
            _statisticalApiServices = statisticalApiServices;
            _userServices = userServices;
        }

        public IActionResult Dashboard()
        {
            var userId = _SessionManag.GetLoginAdminFromSessionAdmin();

            AppConfigs.permission = _userServices.GetUserPermission(userId.UserId).Data.FirstOrDefault();
            var model = _statisticalApiServices.GetValueChart(); //--Leads chart data
            model.roomByDay = _statisticalApiServices.GetRoomInMonthByDay(DateTime.Now.Month,DateTime.Now.Year);
            return View(model);
        }

    }
}
