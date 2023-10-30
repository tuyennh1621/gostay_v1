using GoStay.Common.Configuration;
using GoStay.Common;
using goStayCore.CommonCode;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using GoStay.Service.Api.Users;
using GoStay.DataAccess.Entities;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class UserHomeController : Controller
    {
        private readonly IUserApiServices _userServices;
        private readonly ISessionManager _SessionManag;
        private readonly IMapper _mapper;

        public UserHomeController(IUserApiServices userServices, IMapper mapper, ISessionManager sessionManag)
        {
            _SessionManag = sessionManag;
            _mapper = mapper;
            _userServices = userServices;
        }

        [HttpGet]
        public IActionResult UserList(FilterBase filter)
        {
            filter.PageSize = AppConfigs.ItemPerPage;
            filter.PageIndex = 1;
            ResponseBase<List<User>> response = new ResponseBase<List<User>>();
            try
            {

                var result = _userServices.GetAllUser();
                response.Data = result;
                if (response.Data == null)
                {
                    response.Code = ErrorCodeMessage.ListNull.Key;
                    response.Message = ErrorCodeMessage.ListNull.Value;
                }

            }
            catch
            {
                response.Code = ErrorCodeMessage.InternalExeption.Key;
                response.Message = ErrorCodeMessage.InternalExeption.Value;
            }
            return View(response);
        }
    }
}
