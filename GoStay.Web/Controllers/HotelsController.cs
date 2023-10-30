using AutoMapper;
using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.Common.Enums;
using GoStay.Common.Helper;
using GoStay.DataDto.Apis;
using GoStay.DataDto.Service;
using GoStay.Service;
using GoStay.Service.Api.Hotels;
using GoStay.Service.Api.Users;
using GoStay.Services.WebSupport;
using GoStay.Web.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Text.Json;
using Uno.Extensions;
namespace GoStay.Web.Controllers
{
    public class HotelsController : Controller
    {
        private IUserApiServices _userRepository;
        private IHotelServices _hotelServices;
        private IDistrictServices _districtServices;
        private IProvinceServices _provinceServices;
        private IHotelApiServices _hotelApiServices;
        private IWardServices _wardServices;
        private IMapper _mapper;
        private IServicesServices _servicesServices;
        private ITypeHotelServices _typeHotelServices;
        private static HotelSearchParaRequest _para = new HotelSearchParaRequest();
        private static List<Criterion> Criterions = new List<Criterion>()
        {
            new Criterion { Key = 0, Value = "Nước ngoài", ValueEng = "Foreign", ValueChi = "外国的" },
            new Criterion { Key = 1, Value = "Trong nước", ValueEng = "Domestic", ValueChi = "国内的" },

        };
        private static List<RatingDto> Ratings = new List<RatingDto>()
        {
            new RatingDto { RatingChar = " 1sao", RatingValue = 1 },
            new RatingDto { RatingChar = " 2sao", RatingValue = 2 },
            new RatingDto { RatingChar = " 3sao", RatingValue = 3 },
            new RatingDto { RatingChar = " 4sao", RatingValue = 4 },
            new RatingDto { RatingChar = " 5sao", RatingValue = 5 },
        };

        public HotelsController(IUserApiServices userRepository, IHotelServices hotelServices, IHotelApiServices hotelApiServices,
            IMapper mapper, IServicesServices servicesServices, ITypeHotelServices typeHotelServices, IDistrictServices districtServices,
            IProvinceServices provinceServices, IWardServices wardServices)
        {
            _userRepository = userRepository;
            _hotelServices = hotelServices;
            _hotelApiServices = hotelApiServices;
            _mapper = mapper;
            _servicesServices = servicesServices;
            _typeHotelServices = typeHotelServices;
            _districtServices = districtServices;
            _provinceServices = provinceServices;
            AppConfigs.acvivemenu = 1;
            _wardServices = wardServices;
        }
        public async Task<IActionResult> Index(int IdTinhthanh = 0, int IdPhuong = 0, decimal PriceMin = 0, decimal PriceMax = 100000000, int Palletbed = 0, int NumRoom = 1, int NumMature = 1,
            int NumChild = 0, string CheckinDate = null, string CheckoutDate = null, string[] Quans = null, string[] Types = null, string[] Criterion = null,
            string[] ServiceHotel = null, string[] ServiceRoom = null, string[] Rating = null, int pageIndex = 1, int idHotel = 0, string InputRoomSearch = "")
        {
            ViewData["descriptionPage"] = "Khách sạn giá tốt nhất, khách sạn giá rẻ, khách sạn vị trí trung tâm";

            DateTime checkinDate = new DateTime();
            DateTime checkoutDate = new DateTime();
            AppConfigs.TextHotelSearchBar = InputRoomSearch;
            if (CheckinDate != null)
            {
                checkinDate = DateTime.ParseExact(CheckinDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                AppConfigs.checkinDate = checkinDate.ToString("dd/MM/yyyy");
            }
            if (CheckoutDate != null)
            {
                checkoutDate = DateTime.ParseExact(CheckoutDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                AppConfigs.checkoutDate = checkoutDate.ToString("dd/MM/yyyy");
            }

            if (idHotel > 0)
            {
                return RedirectToAction("Index", "HotelDetail", new { hotelId = idHotel });
            }

            bool isSearch = true;
            if (PriceMin == 0 && PriceMax == 100000000 && Palletbed == 0 && NumMature == 1 && NumRoom == 1 && NumChild == 0 && IdPhuong == 0
                && Quans.IsNullOrEmpty() == true && Types.IsNullOrEmpty() == true && ServiceHotel.IsNullOrEmpty() == true &&
                ServiceRoom.IsNullOrEmpty() == true && Rating.IsNullOrEmpty() == true && CheckinDate == null && CheckoutDate == null)
            {
                isSearch = false;
            }
            if (isSearch == false)
            {
                _para.PriceMax = PriceMax;
                _para.PriceMin = PriceMin;
                _para.Rating = Ratings.Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.RatingValue.ToString(),
                    Text = s.RatingChar,
                    Selected = false
                }).ToList();

                _para.Criterion = Criterions.Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Key.ToString(),
                    Text = s.ValueLang,
                    Selected = false
                }).ToList();

                _para.Quans = _districtServices.GetDistrictByProvinceId(IdTinhthanh).Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Tenquan,
                    Selected = false
                }).ToList();


                _para.Types = _typeHotelServices.GetAllTypeHotel().Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.TypeLang,
                    Selected = false
                }).ToList();

                _para.ServiceHotel = _mapper.Map<List<DataAccess.Entities.Service>, List<ServiceSearchDto>>(_servicesServices.GetServicesSearch(0).ToList())
                    .Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.NameLang,
                        Selected = false
                    }).ToList();

                _para.ServiceRoom = _mapper.Map<List<DataAccess.Entities.Service>, List<ServiceSearchDto>>(_servicesServices.GetServicesSearch(1).ToList())
                    .Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.NameLang,
                        Selected = false
                    }).ToList();

                _para.IDTinhthanh = IdTinhthanh;
                _para.IdPhuong = IdPhuong;
                _para.TenTT = _provinceServices.GetProvinceNameById(IdTinhthanh);

                HotelSearchRequest request = new HotelSearchRequest();

                request.IdTinhThanh = IdTinhthanh;

                request.PageIndex = pageIndex;
                request.PageSize = 15;
                AppConfigs.NumChildren = NumChild;
                AppConfigs.NumAdult = NumMature;
                AppConfigs.NumRoom = NumRoom;




                var hotelForHotelPage = _hotelApiServices.GetListHotelForSearch(request);
                hotelForHotelPage.OrderByDescending(x => x.IntDate);
                HotelsModels hotelsModels = new HotelsModels();
                if (hotelForHotelPage.Count != 0)
                {

                    hotelsModels.hotels = hotelForHotelPage;
                    hotelsModels.Para = _para;
                    hotelsModels.PageIndex = request.PageIndex;
                    hotelsModels.PageSize = request.PageSize;
                    hotelsModels.Total = hotelForHotelPage.FirstOrDefault().Total;
                    hotelsModels.RemainCount = hotelsModels.Total - hotelsModels.PageSize * hotelsModels.PageIndex;
                }
                else
                {
                    hotelsModels.hotels = null;
                    hotelsModels.Para = _para;
                    hotelsModels.PageIndex = 1;
                    hotelsModels.PageSize = request.PageSize;
                }
                ViewData["TitlePage"] = "Khách sạn mới nhất tại Việt Nam";
                return View(hotelsModels);
            }
            else
            {
                _para.CheckinDate = CheckinDate;
                _para.CheckoutDate = CheckoutDate;
                _para.PriceMin = PriceMin;
                _para.PriceMax = PriceMax;
                Rating = Rating.CheckParamUrl();
                Quans = Quans.CheckParamUrl();
                Types = Types.CheckParamUrl();
                ServiceHotel = ServiceHotel.CheckParamUrl();
                ServiceRoom = ServiceRoom.CheckParamUrl();
                AppConfigs.TextHotelSearchBar = InputRoomSearch;

                _para.IDTinhthanh = IdTinhthanh;
                _para.IdPhuong = IdPhuong;
                _para.TenTT = _provinceServices.GetProvinceNameById(IdTinhthanh);

                _para.Rating = Ratings.Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.RatingValue.ToString(),
                    Text = s.RatingChar,
                    Selected = false
                }).ToList();
                _para.Criterion = Criterions.Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Key.ToString(),
                    Text = s.Value,
                    Selected = false
                }).ToList();

                _para.Quans = _districtServices.GetDistrictByProvinceId(IdTinhthanh).Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Tenquan,
                    Selected = false

                }).ToList();


                _para.Types = _typeHotelServices.GetAllTypeHotel().Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.TypeLang,
                    Selected = false
                }).ToList();

                _para.ServiceHotel = _mapper.Map<List<DataAccess.Entities.Service>, List<ServiceSearchDto>>(_servicesServices.GetServicesSearch(0).ToList())
                .Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.NameLang,
                    Selected = false
                }).ToList();



                _para.ServiceRoom = _mapper.Map<List<DataAccess.Entities.Service>, List<ServiceSearchDto>>(_servicesServices.GetServicesSearch(1).ToList())
                .Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.NameLang,
                    Selected = false
                }).ToList();


                HotelSearchRequest request = new HotelSearchRequest();
                if (PriceMin != 0)
                {
                    request.PriceMin = PriceMin;
                }
                if (PriceMax != 0)
                {
                    request.PriceMax = PriceMax;
                }
                if (Palletbed != 0)
                {
                    request.Palletbed = Palletbed;
                }
                if (NumChild != 0)
                {
                    request.NumChild = NumChild;

                }
                AppConfigs.NumChildren = NumChild;

                if (NumRoom != 0)
                {
                    request.NumRoom = NumRoom;

                }
                AppConfigs.NumRoom = NumRoom;

                if (NumMature != 0)
                {
                    request.NumMature = NumMature;
                }
                AppConfigs.NumAdult = NumMature;

                if (DateTime.Compare(new DateTime(), checkinDate) != 0)
                {
                    request.CheckinDate = checkinDate;
                }
                if (DateTime.Compare(new DateTime(), checkoutDate) != 0)
                {
                    request.CheckoutDate = checkoutDate;
                }
                request.IdTinhThanh = IdTinhthanh;
                if (IdPhuong != 0)
                    request.IdPhuong = new List<int> { IdPhuong };
                request.PageIndex = pageIndex;
                request.PageSize = 15;

                if (Rating != null)
                {

                    foreach (System.Web.Mvc.SelectListItem item in _para.Rating)
                    {

                        if (Rating.Contains(item.Value))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }

                    request.Ratings = Array.ConvertAll<string, int>(Rating, int.Parse).ToList();
                }
                if (Criterion != null)
                {

                    foreach (System.Web.Mvc.SelectListItem item in _para.Criterion)
                    {

                        if (Criterion.Contains(item.Value))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }

                    request.Criterion = Array.ConvertAll<string, int>(Criterion, int.Parse).ToList();
                }


                if (Quans != null)
                {

                    foreach (System.Web.Mvc.SelectListItem item in _para.Quans)
                    {
                        if (Quans.Any(x => x.Trim().Equals(item.Value.Trim())))
                        {
                            item.Selected = true;
                            ViewBag.Message += "-" + item.Value;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }

                    request.IdQuans = Array.ConvertAll<string, int>(Quans, int.Parse).ToList();

                }

                if (Types != null)
                {
                    foreach (System.Web.Mvc.SelectListItem item in _para.Types)
                    {
                        if (Types.Contains(item.Value))
                        {
                            item.Selected = true;
                            ViewBag.Message += "-" + item.Value;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    request.Types = Array.ConvertAll<string, int>(Types, int.Parse).ToList();

                }
                request.ServicesHotel = new List<int>();
                if (ServiceHotel != null)
                {
                    foreach (System.Web.Mvc.SelectListItem item in _para.ServiceHotel)
                    {
                        if (ServiceHotel.Contains(item.Value))
                        {
                            item.Selected = true;
                            ViewBag.Message += "-" + item.Value;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    request.ServicesHotel.AddRange(Array.ConvertAll<string, int>(ServiceHotel, int.Parse).ToList());

                }
                request.ServicesRoom = new List<int>();
                if (ServiceRoom != null)
                {
                    foreach (System.Web.Mvc.SelectListItem item in _para.ServiceRoom)
                    {
                        if (ServiceRoom.Contains(item.Value))
                        {
                            item.Selected = true;
                            ViewBag.Message += "-" + item.Value;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    request.ServicesRoom.AddRange(Array.ConvertAll<string, int>(ServiceRoom, int.Parse).ToList());

                }


                var hotelForHotelPage = _hotelApiServices.GetListHotelForSearch(request);
                hotelForHotelPage.OrderByDescending(x => x.IntDate);

                HotelsModels hotelsModels = new HotelsModels();
                if (hotelForHotelPage.Count != 0)
                {

                    hotelsModels.hotels = hotelForHotelPage;
                    hotelsModels.Para = _para;
                    hotelsModels.PageIndex = request.PageIndex;
                    hotelsModels.PageSize = request.PageSize;
                    hotelsModels.Total = hotelForHotelPage.FirstOrDefault().Total;
                    hotelsModels.RemainCount = hotelsModels.Total - hotelsModels.PageSize * hotelsModels.PageIndex;
                }
                else
                {
                    hotelsModels.hotels = null;
                    hotelsModels.Para = _para;
                    hotelsModels.PageIndex = 1;
                    hotelsModels.PageSize = request.PageSize;
                }
                ViewData["TitlePage"] = "Khách sạn mới nhất tại " + _para.TenTT;
                return View(hotelsModels);
            }
        }
        [HttpGet]
        public IActionResult SuggestSearch(string keyword, float lat = 0, float lon = 0)
        {
            if (keyword == ".")
            {
                var result = _hotelApiServices.GetListNearHotel(20, lat, lon);
                return PartialView("~/Views/Shared/_suggestPartial.cshtml", result.Data);
            }
            else
            {

                var result = _hotelApiServices.GetListSuggestHotel(keyword);
                if (result.IsSuccessful)
                {
                    foreach (var item in result.Data.Where(x => x.Type == LocationDropdown.Phuong))
                    {
                        item.QuanID = _wardServices.GetIdDistrictByIdWard(item.Id);
                    }
                }
                return PartialView("~/Views/Shared/_suggestPartial.cshtml", result.Data);
            }

        }

        public IActionResult getNearbyLocation(float lat = 0, float lon = 0)
        {

            var result = _hotelApiServices.GetListNearHotel(20, lat, lon);
            return PartialView("~/Views/Map/mapList.cshtml", result.Data);

        }

        public string getstringNearbyLocation(float lat = 0, float lon = 0)
        {
            var result = _hotelApiServices.GetListNearHotel(20, lat, lon);
            if (result.IsSuccessful)
            {
                return JsonSerializer.Serialize(result.Data);
            }
            else
            {
                return "";
            }
        }
    }
}
