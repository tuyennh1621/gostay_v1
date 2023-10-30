using AutoMapper;
using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.Common.Helper;
using GoStay.Data.Enums;
using GoStay.DataDto.Apis;
using GoStay.DataDto.TourDto;
using GoStay.Service;
using GoStay.Service.Api.Tour;
using GoStay.Services.WebSupport;
using GoStay.Web.Model;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace GoStay.Web.Controllers
{
    public class ToursController : Controller
    {
        private ITourApiServices _tourApiServices;
        private ITourServices _tourServices;
        private IDistrictServices _webSupportApiServices;
        private IMapper _mapper;


        private static SearchTourParam _param = new SearchTourParam();
        private static List<RatingDto> Ratings = new List<RatingDto>()
        {
            new RatingDto { RatingChar = " 1sao", RatingValue = 1 },
            new RatingDto { RatingChar = " 2sao", RatingValue = 2 },
            new RatingDto { RatingChar = " 3sao", RatingValue = 3 },
            new RatingDto { RatingChar = " 4sao", RatingValue = 4 },
            new RatingDto { RatingChar = " 5sao", RatingValue = 5 },
        };
        private static List<ForeignTravelDto> ForeignTravels = new List<ForeignTravelDto>()
        {
            new ForeignTravelDto { ForeignTravelChar = "Tour Nội Địa", ForeignTravelCharEng = "Domestic tour", ForeignTravelCharChi = "国内游", ForeignTravelValue = 0 },
            new ForeignTravelDto { ForeignTravelChar = "Tour Nước Ngoài", ForeignTravelCharEng = "Foreign tour", ForeignTravelCharChi = "国外旅游", ForeignTravelValue = 1 },

        };

        public ToursController(ITourApiServices tourApiServices, ITourServices tourServices, IMapper mapper, IDistrictServices webSupportApiServices)
        {
            _tourApiServices = tourApiServices;
            _tourServices = tourServices;
            _webSupportApiServices = webSupportApiServices;
            _mapper = mapper;
            AppConfigs.acvivemenu = 2;
        }

        public IActionResult Index(string[]? IdTourTopic = null, string[]? IdTourStyle = null, string[]? IdProvinceFrom = null,
            string[]? IdProvinceTo = null, decimal? PriceMin = 0, decimal? PriceMax = 100000000
            , string[]? Rating = null, string[]? ForeignTravel = null, int? NumMature = 0, string? CheckinDate = null, int pageIndex = 1)
        {
            ViewData["TitlePage"] = "Tour du lịch giá rẻ";
            ViewData["descriptionPage"] = "Tour du lịch giá rẻ, tour chọn gói giá tốt";
            DateTime startDate = new DateTime();

            if (CheckinDate != null)
            {
                startDate = DateTime.ParseExact(CheckinDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                AppConfigs.StartDateTours = startDate.ToString("dd/MM/yyyy");
            }


            bool isSearch = true;
            if (PriceMin == 0 && PriceMax == 100000000 && NumMature == 0 &&
                IdTourTopic.IsNullOrEmpty() == true && IdTourStyle.IsNullOrEmpty() == true && IdProvinceFrom.IsNullOrEmpty() == true
                && IdProvinceTo.IsNullOrEmpty() == true && Rating.IsNullOrEmpty() == true && ForeignTravel.IsNullOrEmpty() == true && CheckinDate == null
                 && NumMature == 0)
            {
                isSearch = false;
            }
            if (isSearch == false)
            {
                _param.PriceMax = PriceMax;
                _param.PriceMin = PriceMin;

                _param.Rating = Ratings.Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.RatingValue.ToString(),
                    Text = s.RatingChar,
                    Selected = false
                }).ToList();

                _param.ForeignTravel = ForeignTravels.Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.ForeignTravelValue.ToString(),
                    Text = s.ForeignTravelCharLang,
                    Selected = false
                }).ToList();

                _param.IdTourTopic = _tourServices.GetAllTourTopic().Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.TourTopicLang,
                    Selected = false
                }).ToList();

                _param.IdTourStyle = _tourServices.GetAllTourStyle().Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.TourStyleLang,
                    Selected = false
                }).ToList();

                _param.IdProvinceFrom = _webSupportApiServices.ProvinceFromForTour().Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.TenTt,
                    Selected = false
                }).ToList();

                _param.IdProvinceTo = _webSupportApiServices.ProvinceToForTour().Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.TenTt,
                    Selected = false
                }).ToList();

                SearchTourRequest request = new SearchTourRequest();


                request.PageIndex = pageIndex;
                request.PageSize = 10;

                var tourHomePage = _tourApiServices.SearchTour(request);
                tourHomePage.ForEach(x => x.ProvinceFrom = _webSupportApiServices.GetProvinceNameByDistrictId(x.IdDistrictFrom));

                ToursModels toursModels = new ToursModels();
                if (tourHomePage.Count != 0)
                {
                    toursModels.tours = tourHomePage;
                    toursModels.Para = _param;
                    toursModels.PageIndex = request.PageIndex;
                    toursModels.PageSize = request.PageSize;
                    toursModels.Total = tourHomePage.FirstOrDefault().Total;
                    toursModels.RemainCount = toursModels.Total - toursModels.PageSize * toursModels.PageIndex;
                }
                else
                {
                    toursModels.tours = null;
                    toursModels.Para = _param;
                    toursModels.PageIndex = 1;
                    toursModels.PageSize = request.PageSize;
                }
                return View(toursModels);
            }
            else
            {
                _param.CheckinDate = CheckinDate;
                _param.PriceMin = PriceMin;
                _param.PriceMax = PriceMax;
                _param.NumMature = NumMature;

                Rating = Rating.CheckParamUrl();
                ForeignTravel = ForeignTravel.CheckParamUrl();
                IdProvinceFrom = IdProvinceFrom.CheckParamUrl();
                IdProvinceTo = IdProvinceTo.CheckParamUrl();
                IdTourStyle = IdTourStyle.CheckParamUrl();
                IdTourTopic = IdTourTopic.CheckParamUrl();

                _param.Rating = Ratings.Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.RatingValue.ToString(),
                    Text = s.RatingChar,
                    Selected = false
                }).ToList();

                _param.ForeignTravel = ForeignTravels.Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.ForeignTravelValue.ToString(),
                    Text = s.ForeignTravelCharLang,
                    Selected = false
                }).ToList();

                _param.IdTourTopic = _tourServices.GetAllTourTopic().Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.TourTopicLang,
                    Selected = false
                }).ToList();

                _param.IdTourStyle = _tourServices.GetAllTourStyle().Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.TourStyleLang,
                    Selected = false
                }).ToList();

                _param.IdProvinceFrom = _webSupportApiServices.ProvinceFromForTour().Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.TenTt,
                    Selected = false
                }).ToList();

                _param.IdProvinceTo = _webSupportApiServices.ProvinceToForTour().Select(s => new System.Web.Mvc.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.TenTt,
                    Selected = false
                }).ToList();

                SearchTourRequest request = new SearchTourRequest();
                if (PriceMin != 0)
                {
                    request.PriceMin = PriceMin;
                }
                if (PriceMax != 0)
                {
                    request.PriceMax = PriceMax;
                }
                if (NumMature != 0)
                {
                    request.NumMature = NumMature;
                }

                if (DateTime.Compare(new DateTime(), startDate) != 0)
                {
                    request.StartDate = startDate;
                }

                request.PageIndex = pageIndex;
                request.PageSize = 10;

                if (Rating != null)
                {
                    foreach (System.Web.Mvc.SelectListItem item in _param.Rating)
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
                    request.Rating = Array.ConvertAll<string, int>(Rating, int.Parse);
                }

                if (ForeignTravel != null)
                {
                    foreach (System.Web.Mvc.SelectListItem item in _param.ForeignTravel)
                    {
                        if (ForeignTravel.Contains(item.Value))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    request.ForeignTravel = Array.ConvertAll<string, int>(ForeignTravel, int.Parse);
                }

                if (IdProvinceTo != null)
                {

                    foreach (System.Web.Mvc.SelectListItem item in _param.IdProvinceTo)
                    {
                        if (IdProvinceTo.Any(x => x.Trim().Equals(item.Value.Trim())))
                        {
                            item.Selected = true;
                            ViewBag.Message += "-" + item.Value;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    request.IdDistrictTo = _webSupportApiServices.GetDistrictIdsByProvinceIds(Array.ConvertAll<string, int>(IdProvinceTo, int.Parse));
                }

                if (IdProvinceFrom != null)
                {
                    foreach (System.Web.Mvc.SelectListItem item in _param.IdProvinceFrom)
                    {
                        if (IdProvinceFrom.Contains(item.Value))
                        {
                            item.Selected = true;
                            ViewBag.Message += "-" + item.Value;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    request.IdDistrictFrom = _webSupportApiServices.GetDistrictIdsByProvinceIds(Array.ConvertAll<string, int>(IdProvinceFrom, int.Parse));
                }

                if (IdTourStyle != null)
                {
                    foreach (System.Web.Mvc.SelectListItem item in _param.IdTourStyle)
                    {
                        if (IdTourStyle.Contains(item.Value))
                        {
                            item.Selected = true;
                            ViewBag.Message += "-" + item.Value;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    request.IdTourStyle = Array.ConvertAll<string, int>(IdTourStyle, int.Parse);
                }

                if (IdTourTopic != null)
                {
                    foreach (System.Web.Mvc.SelectListItem item in _param.IdTourTopic)
                    {
                        if (IdTourTopic.Contains(item.Value))
                        {
                            item.Selected = true;
                            ViewBag.Message += "-" + item.Value;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    request.IdTourTopic = Array.ConvertAll<string, int>(IdTourTopic, int.Parse);
                }

                var tourHomePage = _tourApiServices.SearchTour(request);
                tourHomePage.ForEach(x => x.ProvinceFrom = _webSupportApiServices.GetProvinceNameByDistrictId(x.IdDistrictFrom));

                ToursModels toursModels = new ToursModels();
                if (tourHomePage.Count != 0)
                {
                    toursModels.tours = tourHomePage;
                    toursModels.Para = _param;
                    toursModels.PageIndex = request.PageIndex;
                    toursModels.PageSize = request.PageSize;
                    toursModels.Total = tourHomePage.FirstOrDefault().Total;
                    toursModels.RemainCount = toursModels.Total - toursModels.PageSize * toursModels.PageIndex;
                }
                else
                {
                    toursModels.tours = null;
                    toursModels.Para = _param;
                    toursModels.PageIndex = 1;
                    toursModels.PageSize = request.PageSize;
                }

                return View(toursModels);
            }
        }
        [HttpGet("tours/{Id}/{slug}")]
        public IActionResult TourContent(int Id, string slug)
        {
            var temp = _tourApiServices.GetTourContent(Id);
            ViewData["TitlePage"] = temp.TourName + " | Tours du lịch";
            //  ViewData["descriptionPage"] = temp.Descriptions;
            return View(temp);
        }

        [HttpGet]
        public IActionResult SuggestTour(string keyword)
        {
            dynamic data = null;
            data = new
            {
                status = "success",
                message = keyword
            };

            var result = _tourApiServices.SuggestTour(keyword);
            result.FindAll(x => x.Type == SuggestTourType.QuanTo || x.Type == SuggestTourType.QuanFrom).ForEach(x => x.IdProvince = _webSupportApiServices.GetDistrictById(x.Id).IdTinhThanh);
            result.FindAll(x => x.Type == SuggestTourType.TinhTo || x.Type == SuggestTourType.TinhFrom).ForEach(x => x.IdProvince = x.Id);
            result.FindAll(x => x.Type == SuggestTourType.TourName).ForEach(x => x.IdProvince = 0);

            return PartialView("~/Views/Shared/_suggestTourPartial.cshtml", result);
        }
        public IActionResult TotalTourLocation()
        {
            List<TourLocation> list = new List<TourLocation>()
            {
                new TourLocation()
                {
                    Location = "Phú Quốc",
                    Url = "/tours/kien-giang",
                    Img = "/assets/images/location/phu-quoc.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(38).Data
                },
                new TourLocation()
                {
                    Location = "Nha Trang",
                    Url = "/tours/khanh-hoa",
                    Img = "/assets/images/location/nha-trang.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(57).Data
                },
                new TourLocation()
                {
                    Location = "Phan Thiết",
                    Url = "/tours/binh-thuan",
                    Img = "/assets/images/location/phan-thiet.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(31).Data,
                },
                new TourLocation()
                {
                    Location = "Quy Nhơn",
                    Url = "/tours/binh-dinh",
                    Img = "/assets/images/location/quy-nhon.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(30).Data,
                },
                new TourLocation()
                {
                    Location = "Hà Nội",
                    Url = "/tours/ha-noi",
                    Img = "/assets/images/location/ha-noi.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(5).Data,
                },
                new TourLocation()
                {
                    Location = "Đà Nẵng",
                    Url = "/tours/da-nang",
                    Img = "/assets/images/location/da-nang.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(28).Data
                },
                new TourLocation()
                {
                    Location = "Đà Lạt",
                    Url = "/tours/lam-dong",
                    Img = "/assets/images/location/da-lat.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(39).Data
                },
                new TourLocation()
                {
                    Location = "Ninh Bình",
                    Url = "/tours/ninh-binh",
                    Img = "/assets/images/location/ninh-binh.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(41).Data,
                },
                new TourLocation()
                {
                    Location = "Sa Pa",
                    Url = "/tours/lao-cai",
                    Img = "/assets/images/location/sapa.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(58).Data
                },
                new TourLocation()
                {
                    Location = "Hạ Long",
                    Url = "/tours/quang-ninh",
                    Img = "/assets/images/location/ha-long.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(23).Data
                },
                new TourLocation()
                {
                    Location = "Mộc Châu",
                    Url = "/tours/son-la",
                    Img = "/assets/images/location/moc-chau.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(24).Data,
                },
                new TourLocation()
                {
                    Location = "Huế",
                    Url = "/tours/hue",
                    Img = "/assets/images/location/hue.jpg",
                    Total = _tourApiServices.GetTourLocationTotal(65).Data,
                }
            };

            return View(list);
        }


    }
}