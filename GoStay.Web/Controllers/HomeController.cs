using Dapper;
using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.Common.Constants;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Air;
using GoStay.DataDto.Banner;
using GoStay.DataDto.News;
using GoStay.DataDto.TourDto;
using GoStay.Repository.DapperHelper;
using GoStay.Service;
using GoStay.Service.Api.Air;
using GoStay.Service.Api.Hotels;
using GoStay.Service.Api.News;
using GoStay.Service.Api.Tour;
using GoStay.Service.Api.Users;
using GoStay.Service.Api.WebSupport;
using GoStay.Services.WebSupport;
using GoStay.Web.Controllers;
using GoStay.Web.Model;
using goStayCore.CommonCode;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace goStayCore.Controllers
{
    public class HomeController : BaseController
    {
        private IUserApiServices _userRepository;
        private IHotelServices _hotelServices;
        private IHotelApiServices _hotelApiServices;
        private IWebSupportApiServices _webSupportApiServices;
        private ITourApiServices _tourApiServices;
        private ITourServices _tourServices;
        private IDistrictServices _districtServices;
        private IAirApiServices _airApiServices;
        private INewsApiServices _newsApiServices;
        private readonly ISessionManager _SessionManager;

        private readonly ILogger<HomeController> _logger;


        private static List<SearchTourDto> listTourHomePage = new List<SearchTourDto>();
        private static List<Airline> Airlines = new List<Airline>()
        {
            new Airline { Value = "VN", Text = "Vietnam Airline" },
            new Airline { Value = "QH", Text = "Bamboo Airways" },
            new Airline { Value = "VJ", Text = "VietjetAir" },
            new Airline { Value = "VU", Text = "Vietravel Airline" },
            new Airline { Value = "BL", Text = "Jestar Pacifics" },

        };
        private static Dictionary<string, string> LogoAirline = new Dictionary<string, string>()
        {
            { "VN", "assets/images/flight/vna.jpg"},
            { "QH", "assets/images/flight/bamboo.jpg"},
            { "VJ", "assets/images/flight/vietjet.jpg"},
            { "VU", "assets/images/flight/vietravel.jpg"},
            { "BL", "assets/images/flight/jestar.jpg"},
        };
        private static Dictionary<string, string> Points = new Dictionary<string, string>()
        {
            { "SGN", "TPHCM (SGN)"},
            { "HAN", "Hà Nội (HAN)"},
            { "DAD", "Đà Nẵng (DAD)"},
            { "VCS", "Bà Rịa Vũng Tàu (VCS)"},
            { "UIH", "Bình Định (UIH)"},
            { "CAH", "Cà Mau (CAH)"},
            { "VCA", "Cần Thơ (VCA)"},
            { "BMV", "Đắk Lăk (BMV)"},
            { "DIN", "Điện Biên (DIN)"},
            { "PXU", "Gia Lai (PXU)"},
            { "HPH", "Hải Phòng (HPH)"},
            { "HUI", "Huế (HUI)"},
            { "CXR", "Khánh Hòa (CXR)"},
            { "PQC", "Kiên Giang (PQC)"},
            { "VKG", "Kiên Giang (VKG)"},
            { "DLI", "Lâm Đồng (DLI)"},
            { "VII", "Nghệ An (VII)"},
            { "TBB", "Phú Yên (TBB)"},
            { "VDH", "Quảng Bình (VDH)"},
            { "VCL", "Quảng Nam (VCL)"},
            { "VDO", "Quảng Ninh (VDO)"},
            { "THD", "Thanh Hóa (THD)"},
        };
        public HomeController(IUserApiServices userRepository, ITourServices tourServices, IHotelServices hotelServices,
            IHotelApiServices hotelApiServices, IWebSupportApiServices webSupportApiServices, ITourApiServices tourApiServices,
            IDistrictServices districtServices, IAirApiServices airApiServices, ILogger<HomeController> logger, INewsApiServices newsApiServices,
            ISessionManager sessionManager)
        {
            _userRepository = userRepository;
            _hotelServices = hotelServices;
            _hotelApiServices = hotelApiServices;
            _webSupportApiServices = webSupportApiServices;
            _tourApiServices = tourApiServices;
            AppConfigs.acvivemenu = 0;
            _tourServices = tourServices;
            _districtServices = districtServices;
            _airApiServices = airApiServices;
            _newsApiServices = newsApiServices;
            _logger = logger;
            _SessionManager = sessionManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["TitlePage"] = AppConfigs.strTitle;
            ViewData["descriptionPage"] = "Khách sạn giá rẻ, tour du lịch chọn gói giá rẻ, vé máy bay giá rẻ";
            var hotelForHomePage = _hotelApiServices.GetListHotelTopFlashSale();
            foreach (var item in hotelForHomePage)
            {
                if (item.LastOrderDateTemp < TimeSpan.FromMinutes(1))
                {
                    item.LastOrderDate = $"Vừa xong";
                }
                else
                {
                    if (item.LastOrderDateTemp < TimeSpan.FromHours(1))
                    {
                        item.LastOrderDate = $"Đã đặt {item.LastOrderDateTemp.Minutes} phút trước ";
                    }
                    else
                    {
                        if (item.LastOrderDateTemp < TimeSpan.FromDays(1))
                        {
                            item.LastOrderDate = $"Đã đặt {item.LastOrderDateTemp.Hours} giờ trước ";
                        }
                        else
                        {
                            item.LastOrderDate = $"Đã đặt {System.Math.Truncate(item.LastOrderDateTemp.TotalDays)} ngày trước ";

                        }
                    }
                }
            }
            var bannerDetailHome = _webSupportApiServices.GetBannerDetail();

            GetListNewsParam newsParam = new GetListNewsParam();
            newsParam.PageIndex = 1;
            newsParam.PageSize = AppConfigs.ItemPerPage;
            newsParam.IdDomain = Int32.Parse(AppConfigs.IdDomain);
            var topNews = _newsApiServices.GetListNews(newsParam).Data;
            GetListVideoNewsParam paramVideo = new GetListVideoNewsParam();
            paramVideo.Status = 1;
            paramVideo.PageSize = 5;
            paramVideo.PageIndex = 1;
            var topVideo = _newsApiServices.GetListVideo(paramVideo).Data;
            HomeModels homeModels = new HomeModels
            {
                CheckinDate = DateTime.Today.ToString("dd/MM/yyyy"),
                CheckoutDate = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy"),
                ListTopSliderHotel = hotelForHomePage,
                BannerHomePage = new BannerPageDto
                {
                    BannerDetails = bannerDetailHome,
                },
                ListTopNews = topNews?.ToList(),
                video = topVideo
            };
            return View(homeModels);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult LoginWithPhone()
        {
            var user = GetCurrentUser();
            if (user != null)
                return HomePage();
            return View();
        }
        public IActionResult RegisterUserForm(User user)
        {
            var reusult = _userRepository.CheckUserByEmail(user.Email);
            if (reusult != null && reusult.UserId > 0)
            {
                SetCurrentUserCookie(user);
                return HomePage();
            }
            else
            {
                return Content("Đăng nhap không thành công, Email: " + user.Email + " Khong ton tai");
            }
        }
        public IActionResult RegisterUserGooleForm(User user)
        {
            var reusult = _userRepository.RegisterUserPhone(user);
            if (reusult != null)
            {
                SetCurrentUserCookie(user);
                return HomePage();
            }
            else
            {
                return Content("Đăng kí không thành công !");
            }
        }
        public IActionResult RegisterUserPhone(string phoneNumber)
        {
            User user = new User { MobileNo = phoneNumber.StandardizedPhoneNumber() };
            return View(user);
        }
        public IActionResult ExecutePhoneVerified(string number, string statusStr)
        {
            try
            {
                var phoneNumber = JsonConvert.DeserializeObject<string>(number).StandardizedPhoneNumber();
                bool status = JsonConvert.DeserializeObject<bool>(statusStr);
                if (status)
                {
                    var user = _userRepository.CheckUserByPhone(phoneNumber);
                    if (user != null)
                    {
                        SetCurrentUserCookie(user);
                    }
                    else
                    {
                        var reusult = _userRepository.RegisterUserPhone(new User
                        {
                            MobileNo = phoneNumber,
                            UserName = "",
                            Email = "",
                            Password = System.Guid.NewGuid().ToString().Substring(0, 4)
                        });
                        SetCurrentUserCookie(reusult);
                    }

                    var data = new
                    {
                        Dashboard = true,
                        verified = true
                    };
                    return Json(data);
                }
                else
                    return HomePage();
            }
            catch (Exception ex)
            {
                return HomePage();
            }
        }

        public void GoogleLogin()
        {
            HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse" +
                "")
            });
        }
        public void FacebookLogin()
        {
            HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("FacebookResponse")
            });
        }
        public async Task<IActionResult> FacebookResponse()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                var claims = result.Principal.Identities
                    .FirstOrDefault().Claims.Select(claim => new
                    {
                        claim.Value
                    }).ToArray();
                var email = claims[1].Value;
                var user = _userRepository.CheckUserByEmail(email);
                if (user != null)
                {
                    SetCurrentUserCookie(user);
                    return HomePage();
                }
                else
                {
                    return RedirectToAction("RegisterUserEmail", "Home", new
                    {
                        email = claims[1].Value,
                        fullName = claims[2].Value,
                        lastName = claims[3].Value,
                        firstName = claims[4].Value
                    });
                }
                return Json(claims);
            }
            catch
            {
                return HomePage();
            }
        }
        public async Task<IActionResult> GoogleResponse()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                var claims = result.Principal.Identities
                    .FirstOrDefault().Claims.Select(claim => new
                    {
                        claim.Value
                    }).ToArray();
                var email = claims[5].Value;
                var user = _userRepository.CheckUserByEmail(email);
                if (user != null)
                {
                    SetCurrentUserCookie(user);
                    return HomePage();
                }
                else
                {
                    return RedirectToAction("RegisterUserEmail", "Home", new
                    {
                        fullName = claims[1].Value,
                        lastName = claims[2].Value,
                        firstName = claims[3].Value,
                        avatar = claims[4].Value,
                        email = claims[5].Value
                    });
                }
                return Json(claims);
            }
            catch
            {
                return HomePage();
            }
        }
        [HttpPost]
        public IActionResult LoginbyAccount(string email, string password)
        {
            try
            {
                email = JsonConvert.DeserializeObject<string>(email);
                password = JsonConvert.DeserializeObject<string>(password);
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return Json(false);
                }

                var user = _userRepository.CheckUserByAccount(email, password);
                if (user == null)
                    return Json(false);
                else
                {

                    SetCurrentUserCookie(user);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return default;
        }

        [HttpPost]
        public IActionResult UpdateUserInfo(string userJson)
        {
            var _user = JsonConvert.DeserializeObject<GoStay.DataAccess.Entities.User>(userJson);
            dynamic data = null;
            try
            {
                var p = new DynamicParameters();
                p.Add("@user_id", _user.UserId, System.Data.DbType.Int32);
                p.Add("@user_name", _user.UserName, System.Data.DbType.String);
                p.Add("@address", _user.Address, System.Data.DbType.String);
                p.Add("@mobileNo", _user.MobileNo, System.Data.DbType.String);
                p.Add("@email", _user.Email, System.Data.DbType.String);
                p.Add("@picture", _user.Picture, System.Data.DbType.String);
                p.Add("@password", _user.Password, System.Data.DbType.String);

                var reusult = DapperExtensions.ExecuteDapperStoreProc(Procedures.User_updateinfobyIDnotEmail, p);

                SetCurrentUserCookie(_userRepository.CheckUserByID(_user.UserId));

                data = new
                {
                    status = "success",
                    message = "Cập nhật thành công"
                };
                return Json(data);
            }
            catch (DbUpdateConcurrencyException)
            {
                data = new
                {
                    status = "fail!",
                    message = "Cập nhật thất bại"
                };
                return Json(data);
            }
        }

        [HttpPost]
        public IActionResult loginWidthFireBase(string jsonUser)
        {
            var User = JsonConvert.DeserializeObject<user>(jsonUser);

            var user = _userRepository.CheckUserByEmail(User.Email);
            if (user != null)
            {
                SetCurrentUserCookie(user);
            }
            else
            {
                var reusult = _userRepository.RegisterUserEmail(new User
                {
                    MobileNo = "",
                    UserName = User.DisplayName,
                    Email = User.Email,
                    Picture = User.PhotoURL,
                    Password = System.Guid.NewGuid().ToString().Substring(0, 4)
                });
                SetCurrentUserCookie(reusult);
            }
            return HomePage();
        }

        public IActionResult RegisterUserEmail(string fullName, string lastName, string firstName, string email, string avatar)
        {
            User user = new User { UserName = fullName, LastName = lastName, FirstName = firstName, Email = email, Picture = avatar };
            return View(user);
        }
        public async Task<IActionResult> Logout()
        {
            RemoveCurrentUserCookie();
            return HomePage();
        }
        public async Task<IActionResult> GooleLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return HomePage();
        }
        public IActionResult GetHotelHomePage(int IdProvince)
        {

            var result = _hotelApiServices.GetHotelHomePage(IdProvince);
            foreach (var item in result.Data)
            {
                if (item.LastOrderDateTemp < TimeSpan.FromMinutes(1))
                {
                    item.LastOrderDate = $"Vừa xong";
                }
                else
                {
                    if (item.LastOrderDateTemp < TimeSpan.FromHours(1))
                    {


                        item.LastOrderDate = $"Đã đặt {item.LastOrderDateTemp.Minutes} phút trước ";
                    }
                    else
                    {
                        if (item.LastOrderDateTemp < TimeSpan.FromDays(1))
                        {
                            item.LastOrderDate = $"Đã đặt {item.LastOrderDateTemp.Hours} giờ trước ";
                        }
                        else
                        {
                            item.LastOrderDate = $"Đã đặt {System.Math.Truncate(item.LastOrderDateTemp.TotalDays)} ngày trước ";

                        }
                    }
                }
            }
            return PartialView("~/Views/Home/GetHotelHomePage.cshtml", result.Data);
        }
        public IActionResult GetTourHomePage(int IdTourStyle)
        {
            //if (listTourHomePage.Count<=0)
            //{

            //}
            listTourHomePage = _tourApiServices.GetTourHomePage().Data.Where(x => x.IdTourStyle == IdTourStyle).Take(4).ToList();
            return PartialView("~/Views/Home/GetTourHomePage.cshtml", listTourHomePage);
        }

        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            return View();
        }

        public IActionResult CompareTours(int IdTour)
        {
            var userId = _SessionManager.GetUserGostay().UserId;
            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies.FirstOrDefault(x => x.Key == "comparetoursession").Value;
            if (cookie == null)
            {
                cookie = Guid.NewGuid().ToString();
                cookieOptions.Expires = DateTime.Now.AddDays(365);
                cookieOptions.Path = "/";
                Response.Cookies.Append("comparetoursession", $"{cookie}", cookieOptions);
            }
            var data = _tourApiServices.UpdateTourToCompare(new CompareTourParam
            {
                IdTour = IdTour.ToString(),
                IdUser = userId,
                Session = cookie
            });
            var listId = data.Data.Remove(data.Data.Length - 1);
            var listData = _tourApiServices.GetListToursCompare(listId);


            return PartialView("~/Views/Shared/_compareTours.cshtml", listData.Data);
        }

        public IActionResult PaymentGuide()
        {
            return View();
        }
    }
}
