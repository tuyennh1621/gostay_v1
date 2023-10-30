using Microsoft.AspNetCore.Mvc;
using System.Collections;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Collections.Generic;
using GoStay.DataAccess.Entities;
using System.Data.SqlClient;
using GoStay.DataDto.News;
using static ServiceStack.Diagnostics.Events;
using GoStay.Service;
using GoStay.Common;
using GoStay.Service.Api.News;
using AutoMapper;
using goStayCore.CommonCode;
using Newtonsoft.Json;
using ServiceStack;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Google.Cloud.Firestore.V1;
using Google.Cloud.Firestore;
using GoStay.Web.Areas.Admin.Model;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Kiota.Abstractions;
using GoStay.Common.Configuration;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VideosHomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly INewsApiServices _newsApiServices;
        private readonly INewsServices _newsServices;
        private readonly IMyTypedClientServices _client;
        private readonly IVideoNewsServices _videoServices;
        private string _dir;
        private readonly ISessionManager _sessionManag;

        public VideosHomeController( IHostingEnvironment hostingEnvironment, IMyTypedClientServices client, INewsApiServices newsApiServices, 
            INewsServices newsServices, IVideoNewsServices videoServices, ISessionManager sessionManag)
        {
            _hostingEnvironment = hostingEnvironment;
            _newsApiServices = newsApiServices;
            _newsServices = newsServices;
            _client = client;
            _videoServices = videoServices;
            _sessionManag = sessionManag;
        }
        public IActionResult Index()
        {
            GetListVideoNewsParam param = new GetListVideoNewsParam();
            param.UserId= _sessionManag.GetLoginAdminFromSessionAdmin().UserId;
            param.Status= 1;
            param.PageIndex= 1;
            param.PageSize = 10;
            ViewBag.ListVideo = _newsApiServices.GetListVideo(param);
            param.Status = 0;
            param.PageSize = 100;
            return View();
        }
        public IActionResult Create()
        {
            GetListVideoNewsParam param = new GetListVideoNewsParam();
            param.UserId = _sessionManag.GetLoginAdminFromSessionAdmin().UserId;
            param.Status = 0;
            param.PageSize = 100;
            param.PageIndex = 1;
            ViewBag.ListVideoUploaded = _newsApiServices.GetListVideo(param);
            ViewBag.Categories = _newsServices.GetNewsCategories();
            ViewBag.Languages = _newsServices.GetNewsLanguage();
            return View();
        }

        [HttpPost]
        public IActionResult GetListVideo([FromBody] GetListVideoNewsParam param)
        {
            param.PageIndex = (param.PageIndex != 0) ? param.PageIndex : 1;
            param.PageSize = (param.PageSize != 0) ? param.PageSize : 10;
            param.Status = 1;
            param.UserId = _sessionManag.GetLoginAdminFromSessionAdmin().UserId;
            var results = _newsApiServices.GetListVideo(param);
            if (results.Data == null)
            {
                results.Message = "No data";
                results.Code = -1;
            }
            else
            {
                results.Message = "Thanh công";
                results.Code = 0;
            }
            return Json(results);
        }

        [HttpPost]
        public IActionResult AddVideoNews(IFormFile uploadVideo, IFormFile uploadImg, string titleVideo)
        {
            try
            {
                var Name = titleVideo;
                titleVideo = titleVideo.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower();
                
                var userId = _sessionManag.GetLoginAdminFromSessionAdmin().UserId;
                string[] charsToRemove = new string[] { "@", "[", "]", "'" };
                VideoNews video = new VideoNews();
                if (uploadVideo != null)
                {
                    List<IFormFile> list = new List<IFormFile>() { uploadVideo };
                    var temp = _client.PostVideoAndGetData(list, titleVideo, 3, userId);

                    foreach (var c in charsToRemove)
                    {
                        temp.data = temp.data.Replace(c, string.Empty);
                    }
                    var url = uploadVideo.FileName;
                    var type = url.Substring(url.Length - 4);
                    video.Video = $"https://cdn.realtech.com.vn/uploads/1/news/" + DateTime.Now.Year + "/0/" + userId + "/" + titleVideo + type;
                }
                if (uploadImg != null)
                {

                    List<IFormFile> image = new List<IFormFile>() { uploadImg };
                    var tempImg = _client.PostVideoAndGetData(image, titleVideo, 3, userId);

                    foreach (var c in charsToRemove)
                    {
                        tempImg.data = tempImg.data.Replace(c, string.Empty);
                    }
                    int m = uploadImg.FileName.LastIndexOf('.');
                    var typeImg = uploadImg.FileName.Substring(m);
                    video.PictureTitle = $"https://cdn.realtech.com.vn/uploads/1/news/" + DateTime.Now.Year + "/0/" + userId + "/" + titleVideo + typeImg;
                }

                ResponseBase resultul = new ResponseBase();
                video.Status = 0;
                video.DateCreate = DateTime.Today;
                video.IdUser = userId;
                video.Name = Name;
                video.IdCategory = 1;
                video.Title = "Video tải lên";
                resultul.Message += _videoServices.AddVideoNews(video);

                return Redirect("Create");
            }
            catch (Exception e)
            {
                throw (new ApplicationException(string.Format("Exception"), e));
            }
        }

        [HttpPost]
        public IActionResult CreateNews(string Title, int IdCategory, int LangId, string Video, string PictureTitle, string Descriptions)
        {
            dynamic data = null;
            NewsVideoModel param = new NewsVideoModel();
            //var newsDto = JsonConvert.DeserializeObject<NewsDto>(newsJson);
            param.IdUser = _sessionManag.GetLoginAdminFromSessionAdmin().UserId;
            param.Status = 1; // user đăng
            param.Title = Title;
            param.IdCategory = IdCategory;
            param.LangId = LangId;
            param.Video = Video;
            param.PictureTitle = PictureTitle;
            param.Descriptions = Descriptions;

            var response = _newsApiServices.AddVideoNews(param);

            if (response.Message == ErrorCodeMessage.Success.Value)
            {
                TempData["SuccessMsg"] = response.Message;
                data = new
                {   
                    IsCreate = true,
                    Message = "Thành công"
                };
                return Redirect("Index");
            }
            else
            {
                TempData["ErrorMsg"] = response.Message;
                data = new
                {
                    IsCreate = false,
                    Message = "Thêm Thất bại"
                };
                return Redirect("Index");
            }
        }

        [HttpPost]
        public IActionResult EditVideo( VideoNews param)
        {
            param.DateEdit = DateTime.Now;
            param.IdUser = _sessionManag.GetLoginAdminFromSessionAdmin().UserId;
            var results = _newsApiServices.EditVideo(param);
            if (results.Data == null)
            {
                results.Message = "No data";
                results.Code = -1;
            }
            else
            {
                results.Message = "Thanh công";
                results.Code = 0;
            }
            return Redirect("Index");
        }
        [HttpPost]
        public IActionResult DeleteVideo(int Id)
        {
            var results = _newsApiServices.DeleteVideo(Id);
            if (results == null)
            {
                results.Message = "No data";
                results.Code = -1;
            }
            else
            {
                results.Message = "Thanh công";
                results.Code = 0;
            }
            return Json(results);
        }

        public IActionResult SMS()
        {

            return View();
        }
        public class RegistrationViewModel
        {
            public string? Email { get; set; } = null!;
            public string? Mobile { get; set; }
            public string? Name { get; set; }
        }
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            ViewData["Message"] = "Your registration page!.";

            ViewBag.SuccessMessage = null;

            if (model.Email.Contains("menoth.com"))
            {
                ModelState.AddModelError("Email", "We don't support menoth Address !!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Find your Account Sid and Auth Token at twilio.com/user/account
                    const string accountSid = "AC3e41d278d07de5940dbb49bbbc58abff";
                    const string authToken = "ce07215f87df59ff746b18f713a47a35";
                    TwilioClient.Init(accountSid, authToken);

                    //var to = new PhoneNumber("+16073182306");
                    var to = new PhoneNumber("+84398025988");
                    //var message = MessageResource.Create(
                    //    to,
                    //    //from: new PhoneNumber("+84964531148"), //  From number, must be an SMS-enabled Twilio number ( This will send sms from ur "To" numbers ).
                    //    from: new PhoneNumber("+16073182306"), //  From number, must be an SMS-enabled Twilio number ( This will send sms from ur "To" numbers ).
                    //    body: $"Hello {model.Name} !! Welcome to Asp.Net Core With Twilio SMS API !!");

                    var messageOptions = new CreateMessageOptions(
                    new PhoneNumber("+84398025988"));
                    messageOptions.MessagingServiceSid = "MGb7785df5f907a98ac50d05d4728314ff";

                    messageOptions.Body = "test";
                   // messageOptions.PathAccountSid = "test1";

                    var message = MessageResource.Create(messageOptions);
                    Console.WriteLine(message.Body);


                    //var messageOptions = new CreateMessageOptions(
                    //new PhoneNumber("+84398025988"));
                    //messageOptions.From = new PhoneNumber("+16073182306");
                    //messageOptions.Body = "Test go stay";

                    //var message = MessageResource.Create(messageOptions);
                    //Console.WriteLine(message.Body);

                    ModelState.Clear();
                    ViewBag.SuccessMessage = "Registered Successfully !!";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Registration Failure : {ex.Message} ");
                }

            }

             return Redirect("SMS");
        }

        [HttpGet("Admin/Edit/{id}")]
        public IActionResult VideoDetail(int id)
        {
            ResponseBase<VideoNewsDetailDto> response = new ResponseBase<VideoNewsDetailDto>();
            GetListVideoNewsParam param = new GetListVideoNewsParam();
            param.UserId = _sessionManag.GetLoginAdminFromSessionAdmin().UserId;
            param.Status = 0;
            param.PageSize = 100;
            param.PageIndex = 1;
            try
            {
                ViewBag.ListVideoUploaded = _newsApiServices.GetListVideo(param);
                ViewBag.Categories = _newsServices.GetNewsCategories();
                ViewBag.Languages = _newsServices.GetNewsLanguage();

                response = _newsApiServices.GetVideoNews(id);
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
    }
}
