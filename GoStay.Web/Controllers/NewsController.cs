using GoStay.Service;
using GoStay.Services.WebSupport;
using Microsoft.AspNetCore.Mvc;
using GoStay.Common.Configuration;
using GoStay.DataDto.News;
using GoStay.Service.Api.News;
using GoStay.Web.Model;
using GoStay.Web.ViewsModel;
using GoStay.Common;
using ServiceStack;
using ServiceStack.Web;
using Microsoft.AspNetCore.Http;

namespace GoStay.Web.Controllers
{
    public class NewsController : Controller
    {
        private INewsApiServices _newsApiServices;
        private readonly INewsServices _newsServices;
        private readonly IVideoNewsServices _videoNewsServices;
        public NewsController( INewsApiServices newsApiServices, INewsServices NewsServices, IVideoNewsServices videoNewsServices)
        {
            _newsApiServices = newsApiServices;
            _newsServices = NewsServices;
            _videoNewsServices = videoNewsServices;
            AppConfigs.acvivemenu = 4;  
        }
        public IActionResult Index(GetListNewsParam param)
        {
            ViewData["TitlePage"] = "tin tức du lịch";
            ViewData["descriptionPage"] = "Tin tức mới nhất, tin tức văn hóa phong tục ẩm thực du lịch";
            param.PageSize = param.PageSize != 0 ? param.PageSize : AppConfigs.ItemPerPage;
            param.PageIndex = param.PageIndex != 0 ? param.PageIndex : 1;
            param.IdDomain = Int32.Parse(AppConfigs.IdDomain); 
            NewsViewsModel results = new NewsViewsModel();
            GetListVideoNewsParam paramVideo = new GetListVideoNewsParam();
            paramVideo.Status = 1;
            paramVideo.PageSize = 5;
            paramVideo.PageIndex = 1;
            //var list = _newsServices.GetNewsCategories().Select(x => x.Id).ToList();
            results.listCategoriesNews = _newsApiServices.GetListNewsHomePage();
            results.video = _newsApiServices.GetListVideo(paramVideo);
            results.topCultureNews = _newsApiServices.GetListTopNewsbyCategory(AppConfigs.CultureNews, null);
            results.topFoodNews = _newsApiServices.GetListTopNewsbyCategory(AppConfigs.FoodNews, null);
            results.topNews = _newsApiServices.GetListNews(param);
            results.festivallNews = results.listCategoriesNews.Data[AppConfigs.FestivallNews];
            results.foodNews = results.listCategoriesNews.Data[AppConfigs.FoodNews];
            results.saleslNews = results.listCategoriesNews.Data[AppConfigs.SaleslNews];
            //results.courtesyNews = results.listCategoriesNews.Data[3];
            results.cultureNews = results.listCategoriesNews.Data[AppConfigs.CultureNews];
            results.interviewNews = results.listCategoriesNews.Data[AppConfigs.InterviewNews];
            results.travelNews = results.listCategoriesNews.Data[AppConfigs.TravelNews];
            return View(results);
        }
        [HttpGet]
        public IActionResult GetList(int idCategory, int idTopic, int pageNumber) { 
            ResponseBase<List<NewSearchOutDto>> results = new ResponseBase<List<NewSearchOutDto>>();
            GetListNewsParam param = new GetListNewsParam();
            param.PageSize = 10;
            param.IdTopic = (idTopic > 0 ? idTopic : null);
            param.PageIndex = (pageNumber > 0 ? pageNumber : 1);
            param.IdCategory = (idCategory > 0 ? idCategory : null);
            param.IdDomain = Int32.Parse(AppConfigs.IdDomain);
            if (idCategory == 0 && idTopic == 0) param.IdCategory = 1;
            try
            {
                results = _newsApiServices.GetListNews(param);

                if (results.Data == null)
                {
                    results.Code = ErrorCodeMessage.ListNull.Key;
                    results.Message = ErrorCodeMessage.ListNull.Value;
                }
            }
            catch
            {
                results.Code = ErrorCodeMessage.InternalExeption.Key;
                results.Message = ErrorCodeMessage.InternalExeption.Value;
            }
            return Json(results.Data);
        }
        [HttpGet("tin-tuc/{id}/{slug}")]
        public IActionResult NewsDetails (int id, string slug)
        {
            GetListNewsParam param = new GetListNewsParam();
            param.PageSize = param.PageSize != 0 ? param.PageSize : AppConfigs.ItemPerPage;
            param.PageIndex = param.PageIndex != 0 ? param.PageIndex : 1;
            param.IdDomain = Int32.Parse(AppConfigs.IdDomain);
            ResponseBase<NewsDetailDto> response = new ResponseBase<NewsDetailDto>();
            var click = _newsApiServices.EditClick(id);
            try
            {
                var topic = _newsServices.GetNewsTopicTotal();
                ViewBag.TotalTopic = topic.Values.ToList();
                response = _newsApiServices.GetNews(id);
                if (response.Data == null)
                {
                    response.Code = ErrorCodeMessage.ListNull.Key;
                    response.Message = ErrorCodeMessage.ListNull.Value;
                    ViewBag.TopNews = _newsApiServices.GetListTopNewsbyCategory(AppConfigs.CultureNews, null);
                }
                else
                {
                   var cate = (response.Data.IdCategory != null ) ? response.Data.IdCategory : AppConfigs.CultureNews;
                    ViewBag.TopNews = _newsApiServices.GetListTopNewsbyCategory(cate, null);
                    ViewBag.CateTitle = response.Data.Category.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower();
                    ViewData["TitlePage"] = response.Data.Title;
                    string desc = response.Data.Description != null ? response.Data.Description : response.Data.Category;
                    ViewData["descriptionPage"] = (desc.Length  < 160) ? desc : desc.Substring(0,160);
                }
                
            }
            catch
            {
                response.Code = ErrorCodeMessage.InternalExeption.Key;
                response.Message = ErrorCodeMessage.InternalExeption.Value;
            }
            return View(response);
        }
        [HttpGet("danh-muc-tin/idCategory={idCategory}/idTopic={idTopic}")]
        [HttpGet("danh-muc-tin/idCategory={idCategory}")]
        public IActionResult ListNews(int idCategory, int idTopic, string? slug)
        {
            GetListNewsParam param = new GetListNewsParam();
            param.PageSize = param.PageSize != 0 ? param.PageSize : 10;
            param.PageIndex = param.PageIndex != 0 ? param.PageIndex : 1;
            param.IdCategory = (idCategory > 0 ? idCategory : null);
            param.IdTopic = (idTopic > 0 ? idTopic : null);
            param.IdDomain = Int32.Parse(AppConfigs.IdDomain);
            ResponseBase<List<NewSearchOutDto>> results = new ResponseBase<List<NewSearchOutDto>>();
            try
            {

                var topic = _newsServices.GetNewsTopicTotal();
                ViewBag.TotalTopic = topic.Values.ToList();
                ViewBag.TopNews = _newsApiServices.GetListTopNewsbyCategory((idCategory == 0) ? null : idCategory, (idTopic == 0) ? null : idTopic);
                results = _newsApiServices.GetListNews(param);
                if (results.Data == null)
                {
                    results.Code = ErrorCodeMessage.ListNull.Key;
                    results.Message = ErrorCodeMessage.ListNull.Value;
                }
                else
                {
                    var cateTitle = results.Data[0].Category;
                    ViewBag.CateTitle = new { cateTitle = cateTitle, idCategory = idCategory, cateSlug = cateTitle.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower() };

                    ViewData["TitlePage"] = results.Data[0].Category;
                    ViewData["descriptionPage"] = results.Data[0].Category;
                }
            }
            catch
            {
                results.Code = ErrorCodeMessage.InternalExeption.Key;
                results.Message = ErrorCodeMessage.InternalExeption.Value;
            }
            return View(results);
        }

        [HttpGet("danh-muc-tin/{idTopic}/{slug}")]
        public IActionResult ListNewsTopic(int idTopic, string? slug)
        {
            GetListNewsParam param = new GetListNewsParam();
            param.PageSize = param.PageSize != 0 ? param.PageSize : 10;
            param.PageIndex = param.PageIndex != 0 ? param.PageIndex : 1;
            param.IdTopic = (idTopic > 0 ? idTopic : null);
            param.IdDomain = Int32.Parse(AppConfigs.IdDomain);
            ResponseBase<List<NewSearchOutDto>> results = new ResponseBase<List<NewSearchOutDto>>();
            try
            {

                var topic = _newsServices.GetNewsTopicTotal();
                ViewBag.TotalTopic = topic.Values.ToList();
                ViewBag.TopNews = _newsApiServices.GetListTopNewsbyCategory( null, (idTopic == 0) ? null : idTopic);
                var topicTitle = _newsServices.GetNewsTopicTitle(idTopic);
                ViewBag.TopicTitle = new { topicTitle = topicTitle, topicId = idTopic, topicSlug = topicTitle.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower() };              
                results = _newsApiServices.GetListNews(param);
                if (results.Data == null)
                {
                    results.Code = ErrorCodeMessage.ListNull.Key;
                    results.Message = ErrorCodeMessage.ListNull.Value;
                } else
                {

                    ViewData["TitlePage"] = results.Data[0].Category;
                    ViewData["descriptionPage"] = results.Data[0].Category;
                }
            }
            catch
            {
                results.Code = ErrorCodeMessage.InternalExeption.Key;
                results.Message = ErrorCodeMessage.InternalExeption.Value;
            }
            return View(results);
        }
        [HttpGet("video")]
        public IActionResult ListVideoNews()
        {
            GetListVideoNewsParam paramVideo = new GetListVideoNewsParam();
            VideoNewsViewsModel results = new VideoNewsViewsModel();
            paramVideo.Status = 1;
            paramVideo.PageSize = 10;
            paramVideo.PageIndex = 1;
            //var list = _newsServices.GetNewsCategories().Select(x => x.Id).ToList();
            ViewBag.TopNews = _newsApiServices.GetListTopNewsbyCategory(null, null);
            results.AllVideo = _newsApiServices.GetListVideo(paramVideo);
            ViewData["TitlePage"] = "Video GoStay ";
            ViewData["descriptionPage"] = "Video GoStay ";
            var cate = _newsServices.GetTotalByCate();
            ViewBag.GetTotalByCate = cate.Values.ToList();
            //ViewBag.cate = cate..ToList();
            return View(results);
        }

        [HttpGet("danh-muc-video/{IdCategory}/{slug}")]
        [HttpGet("danh-muc-video/{IdCategory}")]
        public IActionResult ListVideoByCate(int IdCategory, string slug)
        {
            GetListVideoNewsParam paramVideo = new GetListVideoNewsParam();
            VideoNewsViewsModel results = new VideoNewsViewsModel();

            paramVideo.Status = 1;
            paramVideo.PageSize = 10;
            paramVideo.PageIndex = 1;
            ViewBag.TopNews = _newsApiServices.GetListTopNewsbyCategory(IdCategory, null);

            paramVideo.IdCategory = IdCategory;
            //var list = _newsServices.GetNewsCategories().Select(x => x.Id).ToList();
            results.AllVideo = _newsApiServices.GetListVideo(paramVideo);
            var cate = _newsServices.GetTotalByCate();
            ViewBag.GetTotalByCate = cate.Values.ToList();
            ViewData["TitlePage"] = (results.AllVideo.Data.Count > 0) ? results.AllVideo.Data[0].Category : slug;
            ViewData["descriptionPage"] = "Video GoStay ";
            //ViewBag.cate = cate..ToList();
            return View(results);
        }

        [HttpGet]
        public IActionResult GetListVideo(int idCategory, int pageNumber)
        {
            ResponseBase<List<VideoNewsDto>> results = new ResponseBase<List<VideoNewsDto>>();
            GetListVideoNewsParam param = new GetListVideoNewsParam();
            param.PageSize = 10;
            param.Status = 1;
            param.PageIndex = (pageNumber > 0 ? pageNumber : 1);
            param.IdCategory = (idCategory > 0 ? idCategory : null);
            try
            {
                results = _newsApiServices.GetListVideo(param);

                if (results.Data == null)
                {
                    results.Code = ErrorCodeMessage.ListNull.Key;
                    results.Message = ErrorCodeMessage.ListNull.Value;
                }
            }
            catch
            {
                results.Code = ErrorCodeMessage.InternalExeption.Key;
                results.Message = ErrorCodeMessage.InternalExeption.Value;
            }
            return Json(results.Data);
        }


        [HttpGet("video/{id}/{slug}")]
        [HttpGet("video/{id}")]
        public IActionResult VideoNewsDetails(int id, string slug)
        {
            var click = _videoNewsServices.UpdateClick(id);
            ViewData["descriptionPage"] = "Video GoStay ";
            GetListVideoNewsParam param = new GetListVideoNewsParam();
            param.PageSize = 10;
            param.PageIndex = 1;
            param.Status = 1;
            VideoNewsViewsModel response = new VideoNewsViewsModel();
            var cate = _newsServices.GetTotalByCate();
            ViewBag.GetTotalByCate = cate.Values.ToList();
            try
            {
                response.VideoNewsDetail = _newsApiServices.GetVideoNews(id);
                response.AllVideo = _newsApiServices.GetListVideo(param);
                if (response.VideoNewsDetail.Data == null)
                {
                    response.VideoNewsDetail.Code = ErrorCodeMessage.ListNull.Key;
                    response.VideoNewsDetail.Message = ErrorCodeMessage.ListNull.Value;
                }
                else
                {
                    ViewData["TitlePage"] = response.VideoNewsDetail.Data.Title;


                }


            }
            catch
            {
                response.VideoNewsDetail.Code = ErrorCodeMessage.InternalExeption.Key;
                response.VideoNewsDetail.Message = ErrorCodeMessage.InternalExeption.Value;
            }
            return View(response);
        }
    }
}
