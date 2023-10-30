using AutoMapper;
using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.TourDto;
using GoStay.Service;
using GoStay.Services.WebSupport;
using goStayCore.CommonCode;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using static GoStay.Web.Areas.Admin.Model.PictureModels;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class TourHomeController : Controller
    {
        private readonly ITourServices _tourServices;
        private readonly IMyTypedClientServices _client;
        private readonly IPicturesServices _pictureServices;
        private readonly IAlbumServices _albumServices;
        private readonly IProvinceServices _provinceServices;
        private readonly IDistrictServices _districtServices;
        private readonly ISessionManager _SessionManag;
        private readonly IMapper _mapper;

        public TourHomeController(ITourServices TourServices, IMyTypedClientServices client, IMapper mapper
            , IPicturesServices pictureServices, IAlbumServices albumServices,
            ISessionManager sessionManag, IProvinceServices provinceServices, IDistrictServices districtServices)
        {
            _tourServices = TourServices;
            _client = client;
            _pictureServices = pictureServices;
            _albumServices = albumServices;
            _SessionManag = sessionManag;
            _provinceServices = provinceServices;
            _districtServices = districtServices;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TransitParamPicture(int IDTour, FilterBase filter)
        {
            filter.PageSize = AppConfigs.ItemPerPage;
            filter.PageIndex = 1;
            filter.IdObj = IDTour;
            PictureDto model = new PictureDto();
            model.Type = 2;
            model.IdType = IDTour;

            ResponseBase<PagingList<Picture>> response = new ResponseBase<PagingList<Picture>>();

            response.Data = _pictureServices.GetPicturesList(filter, 2);
            if (response.Data == null)
            {
                response.Code = ErrorCodeMessage.ListNull.Key;
                response.Message = ErrorCodeMessage.ListNull.Value;
            }
            AddPictureModels _AddPictureModels = new AddPictureModels
            {
                style = 2,
                IdType = IDTour,
                Picture = response.Data
            };

            return PartialView("~/Areas/Admin/Views/PicturesHome/_AddPicturePartial.cshtml", _AddPictureModels);
        }

        [HttpGet]
        public IActionResult TourList()
        {

            try
            {
                ViewBag.listTinhThanh = _provinceServices.GetAllProvince();
                ViewBag.listQuanHuyen = _districtServices.GetAllDistrict();
                ViewBag.listTourStyle = _tourServices.GetAllTourStyle();
                ViewBag.listTourTopic = _tourServices.GetAllTourTopic();
                ViewBag.listVehical = _tourServices.GetVehicle();
                ViewBag.listRating = _tourServices.GetTourRating();
                ViewBag.listStartTime = _tourServices.GetTourStartTime();

                var response = _tourServices.GetAllTour(25, 1);
                response.PageSize = 25;
                response.PageIndex = 1;
                //if (response.Data == null)
                //{
                //    response.Code = ErrorCodeMessage.ListNull.Key;
                //    response.Message = ErrorCodeMessage.ListNull.Value;
                //}
                return View(response);

            }
            catch
            {
                //response.Code = ErrorCodeMessage.InternalExeption.Key;
                //response.Message = ErrorCodeMessage.InternalExeption.Value;
                return View(null);

            }
        }
        [HttpGet]
        public IActionResult TourListPartial(int pageSize, int pageIndex)
        {
            try
            {
                ViewBag.listTinhThanh = _provinceServices.GetAllProvince();
                ViewBag.listQuanHuyen = _districtServices.GetAllDistrict();
                ViewBag.listTourStyle = _tourServices.GetAllTourStyle();
                ViewBag.listTourTopic = _tourServices.GetAllTourTopic();
                ViewBag.listVehical = _tourServices.GetVehicle();
                ViewBag.listRating = _tourServices.GetTourRating();
                ViewBag.listStartTime = _tourServices.GetTourStartTime();

                var response = _tourServices.GetAllTour(pageSize, pageIndex);
                response.PageSize = pageSize;
                response.PageIndex = pageIndex;
                //if (response.Data == null)
                //{
                //    response.Code = ErrorCodeMessage.ListNull.Key;
                //    response.Message = ErrorCodeMessage.ListNull.Value;
                //}
                return PartialView("~/Areas/Admin/Views/TourHome/TourListPartial.cshtml", response);
            }
            catch
            {
                //response.Code = ErrorCodeMessage.InternalExeption.Key;
                //response.Message = ErrorCodeMessage.InternalExeption.Value;
                return PartialView("~/Areas/Admin/Views/TourHome/TourListPartial.cshtml", null);
            }

        }


        [HttpPost]
        public IActionResult AddTour(string tourJson)
        {
            dynamic data = null;
            var tourAdd = JsonConvert.DeserializeObject<TourAddDto>(tourJson);
            var tour = new Tour();
            tourAdd.TourName = tourAdd.TourName.Trim();
            tour = _mapper.Map<TourAddDto, Tour>(tourAdd);
            tour.StartDate = new DateTime(2100, 1, 1);

            if (tourAdd.StartDateString != "" && tourAdd.IdStartTime == null)
            {
                tourAdd.StartDateString = tourAdd.StartDateString.Trim();
                try
                {
                    tour.StartDate = DateTime.ParseExact(tourAdd.StartDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch
                {
                    tour.IdStartTime = _tourServices.AddTourStartTime(tourAdd.StartDateString);
                }
            }
            ResponseBase result = new ResponseBase();
            tour.IdUser = _SessionManag.GetLoginAdminFromSessionAdmin().UserId;

            if (tour.Discount is null)
                tour.Discount = 0;
            tour.Status = 1;
            tour.ActualPrice = tour.Price * (100 - (double)tour.Discount) / 100;
            tour.SearchKey = tour.TourName.RemoveUnicode();
            tour.SearchKey = tour.SearchKey.Replace(" ", string.Empty).ToLower();
            result.Message = _tourServices.AddNewTour(tour, tourAdd.IdDistrictTo, tourAdd.Vehicle);

            if (result.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = result.Message;
                data = new
                {
                    IsCreate = true,
                    Message = "Thành công"
                };
                return Json(data);
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                data = new
                {
                    IsCreate = false,
                    Message = "Thêm Thất bại"
                };
                return Json(data);
            }

        }

        [HttpGet]
        public IActionResult TransitTourDetail(int Idtour, FilterBase? filter)
        {
            filter.PageSize = AppConfigs.ItemPerPage;
            filter.PageIndex = 1;
            filter.IdObj = Idtour;

            var model = _mapper.Map<List<TourDetail>, List<TourDetailDto>>(_tourServices.GetTourDetail(filter).ToList());
            //foreach (var item in model)
            //{
            //    item.Details = HtmlUtilities.ConvertToPlainText(item.Details);
            //}
            if (model.Count() == 0)
                model.Add(new TourDetailDto() { Id = 0, IdTours = 0, IdStyle = null, Title = "", Details = "" });

            model.ForEach(x => x.IdTours = Idtour);

            return PartialView("~/Areas/Admin/Views/TourHome/_AddTourDetail.cshtml", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AddTourDetail(string tourDetailJson)
        {
            dynamic data = null;
            var tourDetail = JsonConvert.DeserializeObject<TourDetail>(tourDetailJson);
            ResponseBase result = new ResponseBase();

            var returnUrl = Request.Headers["Referer"].ToString();


            result.Message = _tourServices.AddNewTourDetail(tourDetail);

            if (result.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = result.Message;
                data = new
                {
                    IsCreate = true,
                    Message = "Thành công"
                };
                return Json(data);
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                data = new
                {
                    IsCreate = false,
                    Message = "Thêm Thất bại"
                };
                return Json(data);
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditTourDetail(string tourDetailJson)
        {
            dynamic data = null;
            var FormData = JsonConvert.DeserializeObject<TourDetailDto>(tourDetailJson);
            ResponseBase result = new ResponseBase();

            var tourdetail = _mapper.Map<TourDetailDto, TourDetail>(FormData);
            result.Message = _tourServices.EditTourDetail(tourdetail);

            if (result.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = result.Message;
                data = new
                {
                    IsCreate = true,
                    Message = "Thành công"
                };
                return Json(data);
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                data = new
                {
                    IsCreate = false,
                    Message = "Edit Thất bại"
                };
                return Json(data);
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditTour(string tourJson)
        {
            dynamic data = null;
            var tourEdit = JsonConvert.DeserializeObject<TourAddDto>(tourJson);
            ResponseBase result = new ResponseBase();
            tourEdit.IdUser = _SessionManag.GetLoginAdminFromSessionAdmin().UserId;

            result.Message = _tourServices.EditTour(tourEdit);

            if (result.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = result.Message;
                data = new
                {
                    IsCreate = true,
                    Message = "Thành công"
                };
                return Json(data);
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                data = new
                {
                    IsCreate = false,
                    Message = "Thêm Thất bại"
                };
                return Json(data);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTour(int Id)
        {
            ResponseBase FormData = new ResponseBase();

            FormData.Message = _tourServices.DeleteTour(Id);

            if (FormData.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = FormData.Message;
                return Redirect("TourList");
            }
            else
            {
                TempData["SuccessMsg"] = FormData.Message;
                return Redirect("TourList");
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult RemoveTourDetail(int IdDetail)
        {
            var returnUrl = Request.Headers["Referer"].ToString();
            ResponseBase result = new ResponseBase();
            result.Message = _tourServices.RemoveTourDetail(IdDetail);
            if (result.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = result.Message;
                return Redirect(returnUrl);
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                return Redirect(returnUrl);
            }

        }


    }
}
