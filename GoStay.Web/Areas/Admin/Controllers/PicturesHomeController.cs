using DAL.Entities;
using DAL.Helpers.PageHelpers;

using GoStay.Common;
using GoStay.Common.Configuration;

using GoStay.DataAccess.Entities;
using GoStay.DataDto.Album;
using GoStay.Service;
using goStayCore.CommonCode;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PicturesHomeController : Controller
    {
        private readonly IPicturesServices _PictureServices;

        private readonly IMyTypedClientServices _client;
        private readonly IAlbumServices _albumServices;

        private readonly IHotelServices _hotelServices;
        private readonly IHotelRoomServices _hotelRoomServices;

        public PicturesHomeController(IPicturesServices PictureServices, IMyTypedClientServices client,
            IAlbumServices albumServices, IHotelServices hotelServices, IHotelRoomServices hotelRoomServices)
        {
            _PictureServices = PictureServices;
            _client = client;
            _albumServices = albumServices;

            _hotelServices = hotelServices;
            _hotelRoomServices = hotelRoomServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PicturesList(FilterBase filter)
        {
            filter.PageSize = AppConfigs.ItemPerPage;
            filter.PageIndex = 1;
            ResponseBase<PagingList<Picture>> response = new ResponseBase<PagingList<Picture>>();
            try
            {
                ViewBag.listAlbum = _albumServices.GetAllAlbum(0,0);

                ViewBag.listRoom = _hotelRoomServices.GetAllHotelRoom();
                ViewBag.listHotel = _hotelServices.GetAllHotel();

                response.Data = _PictureServices.GetPicturesList(filter,0);
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
        public IActionResult AlbumList(FilterBase filter)
        {
            filter.PageSize = AppConfigs.ItemPerPage;
            filter.PageIndex = 1;


            ResponseBase<PagingList<Album>> response = new ResponseBase<PagingList<Album>>();
            try
            {

                response.Data = _albumServices.GetAlbumList(filter);
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
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AddPictureNew(PictureDto FormData)
        {
            var limitFileSize = 4194304; /*4194304;*/
            string sfolder = "tour";
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
            foreach (var file in FormData.Img)
            {
                if (file.Length > limitFileSize)
                {
                    TempData["ErrorMsg"] = "Có ảnh quá 4MB, vui lòng chọn lại";
                    return Json(TempData); 
                }
            }

            int i = 0;
            string success = "";
            string fail = null;
            UploadImagesResponse temp = new UploadImagesResponse();
            
            //FormData.Type = 2;// picture Tour

            temp = _client.PostImgAndGetData(FormData.Img, FormData.width, FormData.IdType.ToString(), 2);

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
                picture.Description = FormData.Description;
                picture.Type = FormData.Type;
                picture.Name = FormData.Name + $"00{i}";
                var x = Path.GetFileNameWithoutExtension(url[i]) + Path.GetExtension(url[i]).Replace("\"", "");
                picture.Url = $"/uploads/"+ sfolder + "/"+ FormData.IdType + "/" + x;
                picture.IdType = FormData.IdType;
                picture.TourId = FormData.IdType;
                picture.Size = int.Parse(size[i]);
                result.Message = _PictureServices.AddNewPicture(picture);
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

            return Json(TempData);

            // return Redirect("TourList");
        }



        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public IActionResult AddPicture(PictureDto FormData)
        //{
        //    if (FormData.Name == null )
        //    {
        //        TempData["ErrorMsg"] = "Please fill all required fields!";
        //        return Redirect("AddPicture");
        //    }

        //    var album = _albumServices.GetAlbumByID(FormData.IdAlbum);
        //    string hotelName = "";
        //    string roomName = "";

        //    int i = 0;
        //    string success = "";
        //    string fail = null;
        //    UploadImagesResponse temp = new UploadImagesResponse();

        //    string[] charsToRemove = new string[] { "@", "[", "]", "'", "\\" };
        //    foreach (var c in charsToRemove)
        //    {
        //        temp.data = temp.data.Replace(c, string.Empty);
        //    }

            
        //    var url = temp.data.Split(",");
        //    for (i = 0 ; i < url.Length ; i++)
        //    {

        //        ResponseBase result = new ResponseBase();

        //        Picture picture = new Picture();
        //        picture.Description = FormData.Description;
        //        picture.Type = FormData.Type;
        //        picture.Name = FormData.Name + $"00{i}";
        //        picture.Url = url[i];
        //        picture.IdAlbum = FormData.IdAlbum;

        //        //--Data Insert Part starts here
        //        result.Message = _PictureServices.AddNewPicture(picture);
        //        if (result.Message == ErrorCodeMessage.Success.Value)
        //        {
        //            success = success + $" ảnh {i} , ";

        //        }
        //        else
        //        {
        //            fail = fail + $" ảnh {i} , ";
        //        }
        //    }
        //    TempData["SuccessMsg"] = success + ErrorCodeMessage.Success.Value;
        //    if(fail != null)
        //        TempData["ErrorMsg"] = fail + ErrorCodeMessage.AddPictureFail.Value;

        //    return Redirect("PicturesList");
        //}

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AddAlbum(AlbumDto FormData)
        {
            ResponseBase result = new ResponseBase();


            if (FormData.Name == null)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect("AddAlbum");
            }

            //--Data Insert Part starts here
            result.Message = _albumServices.AddNewAlbum(FormData);
            if (result.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = result.Message;
                return Redirect("AlbumList");
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                return Redirect("AlbumList");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPicture(Picture FormData)
        {
            ResponseBase model = new ResponseBase();
            var returnUrl = Request.Headers["Referer"].ToString();

            if (FormData.Name == null)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect("EditPicture");
            }

            //--Data update Part starts here
            model.Message = _PictureServices.EditPicture(FormData);
            if (model.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = model.Message;
                return Redirect(returnUrl);
            }
            else
            {
                TempData["ErrorMsg"] = model.Message;
                return Redirect(returnUrl);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAlbum(Album FormData)
        {
            ResponseBase model = new ResponseBase();
            var returnUrl = Request.Headers["Referer"].ToString();
            if (FormData.Name == null)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect("EditPicture");
            }

            //--Data update Part starts here
            model.Message = _albumServices.EditAlbum(FormData);
            if (model.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = model.Message;
                return Redirect(returnUrl);
            }
            else
            {
                TempData["ErrorMsg"] = model.Message;
                return Redirect(returnUrl);
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeletePicture(int Id)
        {
            ResponseBase FormData = new ResponseBase();
            var returnUrl = Request.Headers["Referer"].ToString();
            //--Data Delete Part starts here
            FormData.Message = _PictureServices.DeletePicture(Id);

            if (FormData.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = "Xóa thành công PictureID: "+ Id;
            }
            else
            {
                TempData["ErrorMsg"] = FormData.Message;
                
            }
            return Json(TempData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAlbum(int Id)
        {
            ResponseBase FormData = new ResponseBase();
            var returnUrl = Request.Headers["Referer"].ToString();

            //--Data Delete Part starts here
            FormData.Message = _albumServices.DeleteAlbum(Id);

            if (FormData.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = FormData.Message;
                return Redirect(returnUrl);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.Message;
                return Redirect(returnUrl);
            }

        }
    }

}
