using GoStay.Common.Configuration;
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.Service;
using goStayCore.CommonCode;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static GoStay.Web.Areas.Admin.Model.PictureModels;
using AutoMapper;
using GoStay.Service.Api.News;
using GoStay.DataDto.News;
using System.IO;
using GoStay.Api.Attributes;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class NewsHomeController : Controller
    {
        private readonly INewsApiServices _newsApiServices;
        private readonly INewsServices _newsServices;

        private readonly IMyTypedClientServices _client;
        private readonly IPicturesServices _pictureServices;
        private readonly ISessionManager _SessionManag;
        private readonly IMapper _mapper;


        public NewsHomeController(INewsApiServices NewsApiServices, INewsServices NewsServices, IMyTypedClientServices client, IMapper mapper
            , IPicturesServices pictureServices,ISessionManager sessionManag)
        {
            _newsApiServices = NewsApiServices;
            _newsServices = NewsServices;
            _client = client;
            _pictureServices = pictureServices;
            _SessionManag = sessionManag;
            _mapper = mapper;
        }

        [HttpGet]
        [Role(0, 11)]
        public IActionResult TransitParamPicture(int IdNews, FilterBase filter)
        {
            filter.PageSize = AppConfigs.ItemPerPage;
            filter.PageIndex = 1;
            filter.IdObj = IdNews;
            PictureDto model = new PictureDto();
            model.Type = 3;
            model.IdType = IdNews;

            ResponseBase<PagingList<Picture>> response = new ResponseBase<PagingList<Picture>>();

            response.Data = _pictureServices.GetPicturesList(filter, 2);
            if (response.Data == null)
            {
                response.Code = ErrorCodeMessage.ListNull.Key;
                response.Message = ErrorCodeMessage.ListNull.Value;
            }
            AddPictureModels _AddPictureModels = new AddPictureModels
            {
                style=3,
                IdType=IdNews,
                Picture = response.Data
            };

            return PartialView("~/Areas/Admin/Views/PicturesHome/_AddPicturePartial.cshtml", _AddPictureModels);
        }

        [HttpGet]
        [Role(0, 11)]
        public IActionResult NewsList(GetListNewsParam param)
        {
            param.PageSize = AppConfigs.ItemPerPage;
            param.PageIndex = 1;
            param.IdDomain = Int32.Parse(AppConfigs.IdDomain);
            ResponseBase<List<NewSearchOutDto>> response = new ResponseBase<List<NewSearchOutDto>>();
            try
            {
                ViewBag.Categories = _newsServices.GetNewsCategories();
                ViewBag.Topics = _newsServices.GetNewsTopic();
                ViewBag.Languages = _newsServices.GetNewsLanguage();

                response = _newsApiServices.GetListNews(param);

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
        [Role(0, 11)]

        public IActionResult AddContentNews(int Id)
        {
            return View(Id);
        }
        [HttpGet]
        [Role(0, 11)]

        public IActionResult NewsListPartial()
        {
            GetListNewsParam param = new GetListNewsParam();
            param.PageSize = AppConfigs.ItemPerPage;
            param.PageIndex = 1;
            param.IdDomain = Int32.Parse(AppConfigs.IdDomain);
            ResponseBase<List<NewSearchOutDto>> response = new ResponseBase<List<NewSearchOutDto>>();
            try
            {
                ViewBag.Categories = _newsServices.GetNewsCategories();
                ViewBag.Topics = _newsServices.GetNewsTopic();
                ViewBag.Languages = _newsServices.GetNewsLanguage();

                response = _newsApiServices.GetListNews(param);
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
            return PartialView("~/Areas/Admin/Views/NewsHome/NewsListPartial.cshtml", response);
        }

        [HttpPost]
        [Role(0, 11)]
        public IActionResult AddNews(string newsJson)
        {
            dynamic data = null;
            var newsDto = JsonConvert.DeserializeObject<NewsDto>(newsJson);
            newsDto.IdUser = _SessionManag.GetLoginAdminFromSessionAdmin().UserId;
            newsDto.Keysearch = newsDto.Title.RemoveUnicode().Replace(" ", string.Empty).ToLower();
            newsDto.IdDomain = 1;
            var response = _newsApiServices.AddNews(newsDto);

            if (response.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = response.Message;
                data = new
                {
                    IsCreate = true,
                    Message = "Thành công"
                };
                return Json(data);
            }
            else
            {
                TempData["ErrorMsg"] = response.Message;
                data = new
                {
                    IsCreate = false,
                    Message = "Thêm Thất bại"
                };
                return Json(data);
            }

        }


        [HttpPost]
        [Role(0, 11)]

        public IActionResult EditNews(string newsJson)
        {
            dynamic data = null;
            var newsDto = JsonConvert.DeserializeObject<NewsDto>(newsJson);
            newsDto.IdUser = _SessionManag.GetLoginAdminFromSessionAdmin().UserId;
            newsDto.Keysearch = newsDto.Title.RemoveUnicode().Replace(" ", string.Empty).ToLower();
            newsDto.IdDomain = 1;

            var response = _newsApiServices.EditNews(newsDto);

            if (response.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = response.Message;
                data = new
                {
                    IsCreate = true,
                    Message = "Thành công"
                };
                return Json(data);
            }
            else
            {
                TempData["ErrorMsg"] = response.Message;
                data = new
                {
                    IsCreate = false,
                    Message = "Thêm Thất bại"
                };
                return Json(data);
            }

        }

        [Role(0, 11)]
        [HttpPost]
        public IActionResult EditContentNews(string content,int NewsId)
        {
            dynamic data = null;

            var response = _newsApiServices.EditContentNews(new EditNewsContentParam { Content= content,NewsId=NewsId});

            if (response.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = response.Message;
                data = new
                {
                    IsCreate = true,
                    Message = "Thành công"
                };
                return Json(data);
            }
            else
            {
                TempData["ErrorMsg"] = response.Message;
                data = new
                {
                    IsCreate = false,
                    Message = "Thêm Thất bại"
                };
                return Json(data);
            }

        }
        [HttpPost]
        [Role(0, 11)]
        //[ValidateAntiForgeryToken]
        public IActionResult EditPictureTitleNews(string url, int NewsId)
        {
            dynamic data = null;

            var response = _newsApiServices.EditPictureTitleNews(new EditNewsPictureTitleParam { Url = url, NewsId = NewsId });

            if (response.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = response.Message;
                data = new
                {
                    IsCreate = true,
                    Message = "Thành công"
                };
                return Json(data);
            }
            else
            {
                TempData["ErrorMsg"] = response.Message;
                data = new
                {
                    IsCreate = false,
                    Message = "Thêm Thất bại"
                };
                return Json(data);
            }

        }

        [HttpPost]
        [Role(0, 11)]
        public IActionResult DeleteNews(int Id)
        {

            var response  = _newsApiServices.DeleteNews(Id);

            if (response.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = response.Message;
                return Redirect("NewsList");
            }
            else
            {
                TempData["SuccessMsg"] = response.Message;
                return Redirect("NewsList");
            }

        }

        [HttpPost]
        [Role(0, 11)]
        public IActionResult AddPictureTitleNews(IFormFile upload, int Id)
        {
            dynamic data = null;
            try
            {
                List<IFormFile> list = new List<IFormFile>() { upload };
                var temp = _client.PostImgAndGetData(list, 1024, Id.ToString(), 3);

                string[] charsToRemove = new string[] { "@", "[", "]", "'" };
                foreach (var c in charsToRemove)
                {
                    temp.data = temp.data.Replace(c, string.Empty);
                    temp.size = temp.size.Replace(c, string.Empty);
                }

                var url = temp.data.Split(",");
                var size = temp.size.Split(",");
                ResponseBase resultul = new ResponseBase();


                Picture picture = new Picture();

                picture.Type = 3;
                picture.Name = DateTime.UtcNow.ToString() + $"00{0}";
                var x = Path.GetFileNameWithoutExtension(url[0]) + Path.GetExtension(url[0]).Replace("\"", "");

                picture.Url = $"/uploads/1/news/{DateTime.Now.Year}/" + Id.ToString() + "/" + x;

                picture.IdType = Id;
                picture.NewsId = Id;
                picture.Size = int.Parse(size[0]);
                resultul.Message += _pictureServices.AddNewPicture(picture);
                var response = _newsApiServices.EditPictureTitleNews(new EditNewsPictureTitleParam { Url = picture.UrlOut, NewsId = Id });

                return Redirect("NewsList");
            }
            catch(Exception e)
            {
                throw (new ApplicationException(string.Format("Exception"), e));
            }
        }

        [Role(0, 11)]
        public IActionResult GetNewsId(int Id)
        {
            ResponseBase<NewsDetailDto> response = new ResponseBase<NewsDetailDto>();
            try
            {
                response = _newsApiServices.GetNews(Id);
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
            return View("~/Areas/Admin/Views/Editor/Index.cshtml", response.Data);
        }
    }

}
