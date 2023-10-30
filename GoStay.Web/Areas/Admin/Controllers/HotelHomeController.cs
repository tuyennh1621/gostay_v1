

using AutoMapper;
using DAL.Helpers.PageHelpers;
using GoStay.Api.Attributes;
using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.Common.Helper;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Album;
using GoStay.DataDto.HotelDto;
using GoStay.DataDto.Service;
using GoStay.Service;
using GoStay.Service.Api.Hotels;
using GoStay.Service.Api.Rating;
using GoStay.Services.WebSupport;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class HotelHomeController : Controller
    {

        private readonly IHotelServices _hotelServices;
        private readonly IHotelApiServices _hotelApiServices;
        private readonly IRatingApiServices _ratingApiServices;
        private readonly IMyTypedClientServices _client;
        private readonly IPicturesServices _pictureServices;
        private readonly ITypeHotelServices _typeHotelServices;
        private readonly IPriceRangeServices _priceRangeServices;
        private readonly IHotelRoomServices _roomServices;
        private readonly IAlbumServices _albumServices;
        private readonly IServicesServices _serviceServices;
        private readonly IViewDirectionServices _viewDirectionServices;
        private readonly IProvinceServices _provinceServices;
        private readonly IDistrictServices _districtServices;
        private readonly IWardServices _wardServices;

        private readonly IMapper _mapper;
        private static ResponseBase<HotelListAdmin> response = new ResponseBase<HotelListAdmin>();
        public HotelHomeController(IHotelServices HotelServices, IMyTypedClientServices client,
            IPicturesServices pictureServices, ITypeHotelServices typeHotelServices, IPriceRangeServices priceRangeServices,
            IHotelRoomServices roomServices, IAlbumServices albumServices, IViewDirectionServices viewDirectionServices,
            IServicesServices serviceServices, IMapper mapper, IProvinceServices provinceServices, IDistrictServices districtServices,
            IWardServices wardServices, IHotelApiServices hotelApiServices, IRatingApiServices ratingApiServices)
        {
            _hotelServices = HotelServices;
            _client = client;
            _pictureServices = pictureServices;
            _typeHotelServices = typeHotelServices;
            _priceRangeServices = priceRangeServices;
            _roomServices = roomServices;
            _albumServices = albumServices;
            _viewDirectionServices = viewDirectionServices;
            _serviceServices = serviceServices;
            _mapper = mapper;
            _districtServices = districtServices;
            _wardServices = wardServices;
            _provinceServices = provinceServices;
            _hotelApiServices = hotelApiServices;
            _ratingApiServices = ratingApiServices;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult TransitParamRoom(int IdHotel, FilterBase? filter)
        {
            filter.PageSize = AppConfigs.ItemPerPage;
            filter.PageIndex = 1;
            filter.IdObj = IdHotel;
            RoomAddDto model = new RoomAddDto();
            model.Idhotel = IdHotel;
            var l = _albumServices.GetAllAlbum(1, IdHotel).ToList();
            ViewBag.listAlbum = l;
            ViewBag.listServiceRoom = _serviceServices.GetServices(1);

            ViewBag.listView = _viewDirectionServices.GetAllViewDirection();
            ViewBag.listPalletbed = _roomServices.GetListPalletbed();

            ResponseBase<PagingList<RoomEditDto>> response = new ResponseBase<PagingList<RoomEditDto>>();



            var temp = _roomServices.GetHotelRoomList(filter).ToList();
            if (temp != null)
            {
                var data = _mapper.Map<List<HotelRoom>, List<RoomEditDto>>(temp);
                data.ForEach(x => x.ViewRoom = _roomServices.GetListRoomView(x.Id).TranArrayIntToString());
                data.ForEach(x => x.ServiceRoom = _roomServices.GetListRoomService(x.Id).TranArrayIntToString());
                response.Data = data.ConvertToPaging(filter.PageSize, filter.PageIndex);

            }
            else
            {
                PagingList<RoomEditDto> data = null;
                response.Data = data;

            }
            ViewBag.listRoomHotel = response;
            return PartialView("~/Areas/Admin/Views/HotelHome/_AddRoomPartial.cshtml", model);
        }

        [HttpGet]
        public IActionResult TransitParamServiceHotel(int IDHotel, FilterBase filter)
        {
            filter.PageSize = AppConfigs.ItemPerPage;
            filter.PageIndex = 1;
            filter.IdObj = IDHotel;
            List<ServiceDto> model = new List<ServiceDto>(); /*DataAccess.Entities.Service*/

            ViewBag.listServiceNotAssigned = _mapper.Map<List<DataAccess.Entities.Service>, List<ServiceDto>>(_serviceServices.GetServicesNotAssigned(IDHotel, 0));
            model = _mapper.Map<List<DataAccess.Entities.Service>, List<ServiceDto>>(_serviceServices.GetServicesAssigned(IDHotel, 0));
            //model = ViewBag.listServiceAssigned;
            if (model.Count == 0)
                model.Add(new ServiceDto() { Id = 0, IdHotelorRoom = 0, Icon = "", IdStyle = null, Name = "" });

            model.ForEach(x => x.IdHotelorRoom = IDHotel);

            return PartialView("~/Areas/Admin/Views/ServicesHome/_AddServiceHotelPartial.cshtml", model);
        }

        [HttpGet]
        public IActionResult TransitParamServiceRoom(int IDRoom, FilterBase filter)
        {
            filter.PageSize = AppConfigs.ItemPerPage;
            filter.PageIndex = 1;
            filter.IdObj = IDRoom;
            List<ServiceDto> model = new List<ServiceDto>();

            ViewBag.listServiceNotAssignedRoom = _mapper.Map<List<DataAccess.Entities.Service>, List<ServiceDto>>(_serviceServices.GetServicesNotAssigned(IDRoom, 1));
            model = _mapper.Map<List<DataAccess.Entities.Service>, List<ServiceDto>>(_serviceServices.GetServicesAssigned(IDRoom, 1));

            if (model.Count == 0)
                model.Add(new ServiceDto() { Id = 0, IdHotelorRoom = 0, Icon = "", IdStyle = null, Name = "" });
            model.ForEach(x => x.IdHotelorRoom = IDRoom);

            return PartialView("~/Areas/Admin/Views/ServicesHome/_AddServiceRoomPartial.cshtml", model);
        }

        [HttpGet]
        public IActionResult TransitParamPicture(int Type, int IDObj, FilterBase filter)
        {
            filter.PageSize = AppConfigs.ItemPerPage;
            filter.PageIndex = 1;
            filter.IdObj = IDObj;
            PictureDto model = new PictureDto();
            model.Type = Type;
            model.IdType = IDObj;
            var l = _albumServices.GetAllAlbum(0, IDObj).ToList();
            ViewBag.listAlbum = l;
            ResponseBase<PagingList<Picture>> response = new ResponseBase<PagingList<Picture>>();
            response.Data = _pictureServices.GetPicturesList(filter, Type);
            if (response.Data == null)
            {
                response.Code = ErrorCodeMessage.ListNull.Key;
                response.Message = ErrorCodeMessage.ListNull.Value;
            }
            ViewBag.listPicture = response;

            return PartialView("~/Areas/Admin/Views/HotelHome/_AddPictureHotelPartial.cshtml", model);
        }



        [HttpGet]
        [Role(0)]
        public IActionResult HotelList()
        {
            try
            {
                ViewBag.listService = _serviceServices.GetServices(0);
                ViewBag.listRoom = _roomServices.GetAllHotelRoom();

                ViewBag.listType = _typeHotelServices.GetAllTypeHotel().ToList();
                ViewBag.listPrice = _priceRangeServices.GetAllPriceRange();
                ViewBag.listView = _viewDirectionServices.GetAllViewDirection();

                ViewBag.listTinhThanh = _provinceServices.GetAllProvince();
                ViewBag.listQuan = _districtServices.GetAllDistrict();
                ViewBag.listPhuong = _wardServices.GetAllWards();
                response.Data = _hotelServices.GetHotelList(new GetHotelAdminParam() { IdProvince = 0, PageIndex = 1, PageSize = 25, NameSearch = null });
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

        [HttpGet]
        [Role(0)]
        public IActionResult HotelListPartial(string paramJson)
        {

            var param = JsonConvert.DeserializeObject<GetHotelAdminParam>(paramJson);
            response.Data = _hotelServices.GetHotelList(param);
            if (response.Data == null)
            {
                response.Code = ErrorCodeMessage.ListNull.Key;
                response.Message = ErrorCodeMessage.ListNull.Value;
                response.Message = ErrorCodeMessage.ListNull.Value;
            }
            return PartialView("~/Areas/Admin/Views/HotelHome/HotelListPartial.cshtml", response);
        }


        [HttpGet]
        public IActionResult RatingList()
        {

            var data = _ratingApiServices.GetListRating(null, 0, null, 1, 1000);

            return View(data);
        }
        [HttpGet]
        public IActionResult RatingListPartial(int? HotelId, byte? Status, string? NameSearch, int PageIndex, int PageSize)
        {

            var data = _ratingApiServices.GetListRating(HotelId, Status, NameSearch, 1, 1000);

            return PartialView("~/Areas/Admin/Views/HotelHome/RatingListPartial.cshtml", data);
        }

        [HttpGet]
        public IActionResult binDropdownAjax(int obj, int style)
        {

            List<DataAccess.Entities.MulltiKeyValue> v = new List<DataAccess.Entities.MulltiKeyValue>();

            if (style == 1)
            {
                var listTT = _provinceServices.GetAllProvince();
                foreach (var m in listTT)
                {
                    v.Add(new DataAccess.Entities.MulltiKeyValue { TextValue = m.Id.ToString(), Title = m.TenTt });
                }
            }
            else if (style == 2)
            {
                var listTT = _districtServices.GetDistrictByProvinceId(obj);
                foreach (var m in listTT)
                {
                    v.Add(new DataAccess.Entities.MulltiKeyValue { TextValue = m.Id.ToString(), Title = m.Tenquan });
                }
            }
            if (style == 3)
            {
                var listTT = _wardServices.GetWardsByIdDistrict(obj);
                foreach (var m in listTT)
                {
                    v.Add(new DataAccess.Entities.MulltiKeyValue { TextValue = m.Id.ToString(), Title = m.Tenphuong });
                }
            }

            return PartialView("~/Views/Shared/_dropdownPartical.cshtml", v);
        }


        [HttpGet]
        public IActionResult HotelRoomList()
        {
            RequestGetListRoomAdmin request = new RequestGetListRoomAdmin() { IdUser = null, IdHotel = null, RoomName = "", HotelName = "", PageIndex = 1, PageSize = 25, RoomStatus = null };
            try
            {

                var response = _hotelApiServices.GetListRoomAdmin(request);
                var model = new ViewModel<RoomAdminDto>();
                model.Data = response.Data;
                model.PageSize = request.PageSize;
                model.PageIndex = request.PageIndex;
                return View(model);
            }
            catch
            {
                return View(new ViewModel<RoomAdminDto>());
            }

        }

        [HttpGet]
        public IActionResult HotelRoomListPartial(string request)
        {

            var model = new ViewModel<RoomAdminDto>();
            var search = JsonConvert.DeserializeObject<RequestGetListRoomAdmin>(request);
            try
            {
                var response = _hotelApiServices.GetListRoomAdmin(search);
                model.Data = response.Data;
                model.PageSize = search.PageSize;
                model.PageIndex = search.PageIndex;
                return View("~/Areas/Admin/Views/HotelHome/HotelRoomListPartial.cshtml", model);
            }
            catch
            {
                return PartialView("~/Areas/Admin/Views/HotelHome/HotelRoomListPartial.cshtml", model);
            }
        }

        public void ChangeRoomStatus(int IdRoom, int RoomStatus)
        {

            try
            {

                var response = _hotelApiServices.ChangeRoomStatus(IdRoom, RoomStatus);
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
        }

        public void UpdateStatusRating(int Id, byte Status)
        {

            try
            {

                var response = _ratingApiServices.UpdateStatusRating(Id, Status);
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
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult _AddMapPartial(string IdHotel, string hotelName, string Lat, string Lon)
        {
            float flat = 0;
            float flon = 0;
            float.TryParse(Lat, out flat);
            float.TryParse(Lon, out flon);
            string model = IdHotel + "|" + System.Net.WebUtility.UrlDecode(hotelName) + "|" + flat + "|" + flon;
            return PartialView("~/Areas/Admin/Views/HotelHome/_AddMapPartial.cshtml", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AddHotel(string hotelJson)
        {
            dynamic data = null;
            var hotel = JsonConvert.DeserializeObject<Hotel>(hotelJson);
            ResponseBase result = new ResponseBase();
            var returnUrl = Request.Headers["Referer"].ToString();
            if (hotel.Name == null)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                data = new
                {
                    IsCreate = false,
                    Message = "Bạn cần Nhập Tên"
                };
                return Json(data);
            }
            hotel.SearchKey = hotel.Name.RemoveUnicode();
            hotel.SearchKey = hotel.SearchKey.Replace(" ", string.Empty).ToLower();

            result.Message = _hotelServices.AddNewHotel(hotel);

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
        public IActionResult AddHotelRoom(string roomdto)
        {
            dynamic data = null;
            var room = JsonConvert.DeserializeObject<RoomAddDto>(roomdto);

            var hotelRoom = _mapper.Map<RoomAddDto, HotelRoom>(room);
            ResponseBase result = new ResponseBase();


            if (hotelRoom.Discount == null)
            {
                hotelRoom.Discount = 0;
            }
            if (hotelRoom.Palletbed == null)
            {
                hotelRoom.Palletbed = 1;
            }
            hotelRoom.NewPrice = hotelRoom.PriceValue * (100 - (decimal)hotelRoom.Discount) / 100;
            hotelRoom.CurrentPrice = hotelRoom.PriceValue;
            hotelRoom.Iduser = 9;

            result.Message = _roomServices.AddNewHotelRoom(hotelRoom, room.ViewRoom, room.ServiceRoom);
            if (result.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = result.Message;
                return Redirect("HotelList");
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                return Redirect("HotelList");
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AddServiceHotel(string fromdata)
        {

            var data = JsonConvert.DeserializeObject<ServicesDto>(fromdata);
            var returnUrl = Request.Headers["Referer"].ToString();
            ResponseBase result = new ResponseBase();

            foreach (string idService in data.IdServices)
            {
                int _idService = 0;

                int.TryParse(idService, out _idService);

                if (_idService > 0)
                {
                    var mameniti = new HotelMameniti { Idhotel = data.IdHotelorRoom };

                    var service = _serviceServices.GetServiceById(int.Parse(idService));
                    service.HotelMamenitis.Add(mameniti);

                    result.Message = _serviceServices.EditServices(service);
                }
            }

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


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AddServiceRoom(string fromdata)
        {
            var data = JsonConvert.DeserializeObject<ServicesDto>(fromdata);
            var returnUrl = Request.Headers["Referer"].ToString();
            ResponseBase result = new ResponseBase();

            foreach (string idService in data.IdServices)
            {
                int _idService = 0;

                int.TryParse(idService, out _idService);

                if (_idService > 0)
                {
                    var mameniti = new RoomMameniti { Idroom = data.IdHotelorRoom };

                    var service = _serviceServices.GetServiceById(int.Parse(idService));
                    service.RoomMamenitis.Add(mameniti);

                    result.Message = _serviceServices.EditServices(service);
                }
            }

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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AddPicture(PictureDto FormData)
        {
            string sfolder = "";


            switch (FormData.Type)
            {
                case 0:
                    sfolder = "hotel";

                    break;
                case 1:
                    sfolder = "room";

                    break;
                case 2:
                    sfolder = "tour";

                    break;
                default:
                    sfolder = "news";
                    break;
            }
            var limitFileSize = 4194304; /*4194304;*/
            foreach (var file in FormData.Img)
            {
                if (file.Length > limitFileSize)
                {
                    TempData["ErrorMsg"] = "Có ảnh quá 4MB, vui lòng chọn lại";
                    return Redirect("HotelList");
                }
            }

            if (FormData.newAlbum != null)
            {
                if (_albumServices.GetAllAlbum(FormData.Type ?? 0, FormData.IdType).SingleOrDefault(x => x.Name == FormData.newAlbum) == null)
                {
                    AlbumDto albumDto = new AlbumDto();
                    albumDto.Name = FormData.newAlbum;
                    albumDto.TypeAlbum = FormData.Type; // type = hotel
                    albumDto.IdType = FormData.IdType;
                    _albumServices.AddNewAlbum(albumDto);

                    FormData.IdAlbum = _albumServices.GetAllAlbum(FormData.Type ?? 0, FormData.IdType)
                    .Select(f => (int)f.Id).Max();
                }
                else
                {
                    FormData.IdAlbum = _albumServices.GetAllAlbum(FormData.Type ?? 0, FormData.IdType).SingleOrDefault(x => x.Name == FormData.newAlbum).Id;
                }

            }
            if (FormData.Name == null)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect("AddPicture");
            }

            int i = 0;
            string success = "";
            string fail = null;
            UploadImagesResponse temp = new UploadImagesResponse();


            temp = _client.PostImgAndGetData(FormData.Img, FormData.width, FormData.IdType.ToString(), FormData.Type ?? 0);

            string[] charsToRemove = new string[] { "@", "[", "]", "'" };
            foreach (var c in charsToRemove)
            {
                temp.data = temp.data.Replace(c, string.Empty);
                temp.size = temp.size.Replace(c, string.Empty);
            }

            var url = temp.data.Split(",");
            var size = temp.size.Split(",");
            for (i = 0; i < url.Length; i++)
            {

                ResponseBase result = new ResponseBase();
                Picture picture = new Picture();

                switch (FormData.Type)
                {
                    case 0:

                        picture.HotelId = FormData.IdType;
                        break;
                    case 1:

                        picture.HotelRoomId = FormData.IdType;
                        break;
                    case 2:

                        picture.TourId = FormData.IdType;
                        break;
                    default:
                        sfolder = "news"; break;
                }
                picture.Description = FormData.Description;
                picture.Type = FormData.Type;
                picture.Name = FormData.Name + $"00{i}";
                var x = Path.GetFileNameWithoutExtension(url[i]) + Path.GetExtension(url[i]).Replace("\"", "");

                picture.Url = $"/uploads/" + sfolder + "/" + FormData.IdType + "/" + x;

                picture.IdAlbum = FormData.IdAlbum;

                picture.IdType = FormData.IdType;

                picture.Size = int.Parse(size[i]);
                result.Message = _pictureServices.AddNewPicture(picture);
                if (result.Message == ErrorCodeMessage.Success.Value)
                {
                    success = success + $" ảnh {i} , ";

                }
                else
                {
                    fail = fail + $" ảnh {i} , ";
                }
            }
            TempData["SuccessMsg"] = success + ErrorCodeMessage.Success.Value;
            if (fail != null)
                TempData["ErrorMsg"] = fail + ErrorCodeMessage.AddPictureFail.Value;

            return Redirect("HotelList");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditHotel(Hotel FormData)
        {
            ResponseBase model = new ResponseBase();

            if (FormData.Name == null)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect("EditHotel");
            }
            FormData.SearchKey = FormData.Name.RemoveUnicode();
            FormData.SearchKey = FormData.SearchKey.Replace(" ", string.Empty).ToLower();

            //--Data update Part starts here
            model.Message = _hotelServices.EditHotel(FormData);
            if (model.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = model.Message;
                return Redirect("HotelList");
            }
            else
            {
                TempData["ErrorMsg"] = model.Message;
                return Redirect("HotelList");
            }

        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditHotelRoom(string roomedit)
        {
            dynamic data = null;
            var room = JsonConvert.DeserializeObject<RoomAddDto>(roomedit);

            var hotelRoom = _mapper.Map<RoomAddDto, HotelRoom>(room);
            ResponseBase result = new ResponseBase();



            if (hotelRoom.Discount == null)
            {
                hotelRoom.Discount = 0;
            }
            if (hotelRoom.Palletbed == null)
            {
                hotelRoom.Palletbed = 1;
            }
            hotelRoom.NewPrice = (decimal)room.PriceValue * (100 - (decimal)hotelRoom.Discount) / 100;

            hotelRoom.Iduser = 9;

            result.Message = _roomServices.EditHotelRoom(hotelRoom, room.ViewRoom, room.ServiceRoom);
            if (result.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = result.Message;
                return Redirect("HotelList");
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                return Redirect("HotelList");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteHotel(int Id)
        {
            ResponseBase FormData = new ResponseBase();


            //--getting calling page URL

            //--Data Delete Part starts here
            FormData.Message = _hotelServices.DeleteHotel(Id);

            if (FormData.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = FormData.Message;
                return Redirect("HotelList");
            }
            else
            {
                TempData["SuccessMsg"] = FormData.Message;
                return Redirect("HotelList");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteHotelRoom(int Id)
        {
            ResponseBase FormData = new ResponseBase();
            var returnUrl = Request.Headers["Referer"].ToString();


            //--getting calling page URL

            //--Data Delete Part starts here
            FormData.Message = _roomServices.DeleteHotelRoom(Id);

            if (FormData.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = FormData.Message;
                return Redirect(returnUrl);
            }
            else
            {
                TempData["SuccessMsg"] = FormData.Message;
                return Redirect(returnUrl);
            }

        }

        public string CapNhatToaDo(int IdHotel, string LAT, string LON)
        {
            try
            {
                System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Float;

                float lat = 0;
                float lon = 0;

                float.TryParse(LAT, style, CultureInfo.InvariantCulture, out lat);
                float.TryParse(LON, style, CultureInfo.InvariantCulture, out lon);

                return _hotelServices.SetMapHotel(new DataDto.HotelDto.SetMapHotelRequest()
                {
                    HotelId = IdHotel,
                    LAT = lat,
                    LON = lon,
                });

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


    }
}
