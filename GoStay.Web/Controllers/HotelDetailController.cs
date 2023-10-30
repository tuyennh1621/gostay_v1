using Google.Type;
using GoStay.Common;
using GoStay.Common.Configuration;

using GoStay.DataDto.HotelDto;
using GoStay.DataDto.RatingDto;
using GoStay.Service;
using GoStay.Service.Api.Hotels;
using GoStay.Service.Api.Rating;
using GoStay.Service.Api.Users;
using goStayCore.CommonCode;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;

namespace GoStay.Web.Controllers
{
    public class HotelDetailController : Controller
    {
        private IUserApiServices _userRepository;
        private IHotelServices _hotelServices;
        private IHotelApiServices _hotelApiServices;
        private IRatingApiServices _ratingApiServices;

        private readonly ISessionManager _SessionManag;
        public HotelDetailController(IUserApiServices userRepository, IHotelServices hotelServices,
            IHotelApiServices hotelApiServices, IRatingApiServices ratingApiServices,
            ISessionManager SessionManag)

        {
            _userRepository = userRepository;
            _hotelServices = hotelServices;
            _hotelApiServices = hotelApiServices;
            _ratingApiServices = ratingApiServices;
            _SessionManag = SessionManag;
            AppConfigs.acvivemenu = 1;
        }
        [HttpGet("khach-san/{hotelId}/{slug}")]
        [HttpGet("khach-san/{hotelId}")]
        public IActionResult Index(int hotelId, string CheckinDate = null, string CheckoutDate = null, int NumMature = 0, int NumChild = 0, string slug = "")
        {

            var UserId = _SessionManag.GetGostayUserFromSession().UserId;
            //var hotelForHotelPage = _hotelApiServices.GetHotelDetail(hotelId);
            var summary = _hotelApiServices.GetHotelDetailSummary(hotelId, UserId);
            //var summary = _hotelApiServices.GetHotelDetail(hotelId);

            if (summary.HotelDetailDto != null)
            {
                ViewData["TitlePage"] = summary.HotelDetailDto.HotelName + " | Khách sạn";
                ViewData["descriptionPage"] = summary.HotelDetailDto.HotelName + " vị trí đẹp giá tốt, khách sạn giá rẻ";
            }
            //hoangvx lay danh gia cua ks
            if (UserId == 1)
            {
                summary.HotelDetailDto.Ordered = 2;// chưa login
            }
            else
            {

                //var check = _ratingApiServices.CheckOrdered(hotelId, UserId);
                summary.HotelDetailDto.Ordered = summary.HotelDetailDto.Ordered > 0 ? 1 : 0;

            }
            var hotelRating = _ratingApiServices.GetUserBoxReview(hotelId);
            //var hotelRating = new ResponseBase<List<UserBoxReview>>();
            hotelRating.Data = summary.UserBoxReviews;
            ViewBag.HotelRating = hotelRating;
            return View(summary.HotelDetailDto);
            //return View(summary);

        }


        [HttpPost]
        public JsonResult UserReview(RatingOrUpdateDto rating)
        {
            try
            {

                rating.UserId = _SessionManag.GetGostayUserFromSession().UserId;
                if (rating.UserId == 1) return Json(Ok("Vui lòng đăng nhập để được đánh giá"));
                if (rating is null || rating.UserId == 0 || rating.HotelId == 0) return Json(Ok("Có lỗi xảy ra vui lòng thử lại sau"));

                rating.Description = rating.Description ?? "";
                var respon = _ratingApiServices.ReviewOrUpdateScore(rating);
                Log.Information($"respon ReviewOrUpdateScore {JsonConvert.SerializeObject(respon)}");
                if (respon.IsSuccessful == false) Json(Ok("Có lỗi xảy ra vui lòng thử lại sau!"));
                return Json(Ok("Cảm ơn đánh giá của bạn!"));
                
            }
            catch (Exception ex)
            {
                Log.Error($"--> UserReview ex {ex}");
                return Json(Ok("Có lỗi xảy ra vui lòng thử lại sau"));
            }

        }

        public IActionResult showImageRoom(int IDRooms, string RoomName, byte NumMature, decimal RoomSize, decimal PriceValue, double Discount, string ViewRoom, string PalletbedText, decimal NewPrice, int Idhotel)
        {
            DataDto.Apis.HotelRoomDto Rooms = new DataDto.Apis.HotelRoomDto();
            var pictureDetail = _hotelApiServices.GetPicturesRoom(IDRooms);
            Rooms.Pictures = pictureDetail.Data.Select(x => x.UrlOut).ToList();
            Rooms.Id = IDRooms;
            Rooms.Idhotel = Idhotel;
            Rooms.Name = RoomName;
            Rooms.NumMature = NumMature;
            Rooms.RoomSize = RoomSize;
            Rooms.PriceValue = PriceValue;
            Rooms.Discount = Discount;
            Rooms.ViewRoom = ViewRoom;
            Rooms.PalletbedText = PalletbedText;
            Rooms.NewPrice = PriceValue * (decimal)(100 - Discount) / 100;

            var serviceRoom = _hotelApiServices.GetServicesRoom(IDRooms);
            Rooms.Services = serviceRoom.Data.ToList();
            return View(Rooms);
        }
        public IActionResult showAllServiceOnly(int IDRooms)
        {
            DataDto.Apis.HotelRoomDto Rooms = new DataDto.Apis.HotelRoomDto();

            var serviceRoom = _hotelApiServices.GetServicesRoom(IDRooms);
            Rooms.Services = serviceRoom.Data.ToList();
            return View(Rooms);
        }

        public IActionResult showImageHotel(int hotelId)
        {
            var Pictures = _hotelApiServices.GetPicturesHotel(hotelId).Data.Select(x => x.UrlOut).ToList();
            return View(Pictures);
        }
        public IActionResult SelectDate(int IdRoom, int IdHotel, int BasePrice,int MinNight)
        {
            ParamSelectDateRoom param = new ParamSelectDateRoom()
            {
                IdHotel = IdHotel,
                IdRoom = IdRoom,
                BasePrice = BasePrice,
                MinNight = MinNight
            };
            return View(param);
        }
    }
}
