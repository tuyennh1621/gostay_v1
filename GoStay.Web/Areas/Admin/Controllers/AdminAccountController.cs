using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using goStayCore.CommonCode;

using GoStay.DataAccess.Entities;
using GoStay.Common.Configuration;
using GoStay.Web.Controllers;
using GoStay.Service.Api.Users;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : BaseController
    {
        public readonly ISessionManager _SessionManag;
        private readonly IHttpContextAccessor _contx;
        private readonly IUserApiServices _userRepository;


        public AdminAccountController(ISessionManager sessionManag, IUserApiServices userRepository, IHttpContextAccessor contx)
        {
            _SessionManag = sessionManag;
            _userRepository = userRepository;
            _contx = contx;
        }

        [HttpGet]
        public IActionResult Login()
        {
            RemoveCurrentUserCookieAdmin();
            return View();
        }
        [HttpPost]
        public IActionResult Login(string? email, string? password, bool saveLogin)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                TempData["ErrorMsg"] = "Incorrect email or password!";
                return RedirectToAction("Login", "AdminAccount");
            }

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login", "AdminAccount");
            }

            var user = _userRepository.CheckUserByAccount(email, password);
            if (user == null)
                return RedirectToAction("Login", "AdminAccount");
            else
            {
                _contx.HttpContext.Session.SetString(AppConfigs.CurrentUserAdmin, JsonConvert.SerializeObject(user));
                SetCurrentUserCookieAdmin(user);
                return RedirectToAction("Dashboard", "AdminHome");
            }

        }
        //[HttpPost]
        //public IActionResult Login(string? email, string? password, bool saveLogin)
        //{
        //    if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
        //    {
        //        TempData["ErrorMsg"] = "Incorrect email or password!";
        //        return RedirectToAction("Login", "AdminAccount");
        //    }

        //    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        //    {
        //        return RedirectToAction("Login", "AdminAccount");
        //    }

        //    //var user = _userRepository.CheckUserByAccount(email, password);
        //    var data = _userRepository.CheckUserByAccountAndGetToken(email, password);

        //    if (data.User == null)
        //        return RedirectToAction("Login", "AdminAccount");
        //    else
        //    {
        //        _contx.HttpContext.Session.SetString(AppConfigs.CurrentUserAdmin, JsonConvert.SerializeObject(data.User));
        //        SetCurrentUserCookieAdmin(data.User);
        //        return RedirectToAction("Dashboard", "AdminHome");
        //    }

        //}


        [HttpGet]
        public IActionResult Logout()
        {
            string? msg = RemoveSessionAndCookies();
            if (msg != "Removed Successfully!")
            {
                return RedirectToAction("Index", "Login");
            }
            return RedirectToAction("Login", "AdminAccount");
        }

        public string? RemoveSessionAndCookies()
        {
            string? result = "Removed Successfully!";
            HttpContext.Session.Clear();
            if (Request.Cookies != null)
            {
                try
                {
                    foreach (var cookie in Request.Cookies.Keys)
                    {
                        Response.Cookies.Delete(cookie);
                    }
                }
                catch (Exception ex)
                {
                    string? errorMsg = ex.Message;
                    result = "An error occured.";
                    return result;
                }

            }

            return result;

        }

    }
}
