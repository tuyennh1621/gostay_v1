using GoStay.Service.Api.News;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace GoStay.Web.Areas.Admin.Controllers
{
    public class Video
    {
        public string title { get; set; }
        public string poster { get; set; }
        public string source { get; set; }
    }
    [Area("Admin")]

    public class MediaPlayerController : Controller
    {
        private readonly INewsApiServices _newsApiServices;

        public MediaPlayerController(INewsApiServices newsApiServices)
        {
            _newsApiServices = newsApiServices;
        }
        public ActionResult Playlist(int UserId)
        {
            return View(GetVideos(UserId));
        }

        public ActionResult Videos_Read([DataSourceRequest] DataSourceRequest request, int UserId)
        {
            return Json(GetVideos(UserId).ToDataSourceResult(request));
        }

        private static IEnumerable<Video> GetVideos(int UserId)
        {
            List<Video> videos = new List<Video>();
            //var data = _newsApiServices.GetListVideoNews(UserId);
            videos.Add(new Video()
            {
                title = "Build HIPAA-Compliant Healthcare Apps Fast",
                poster = "https://img.youtube.com/vi/_S63eCewxRg/1.jpg",
                source = "https://www.youtube.com/watch?v=dyvxivS5EcI"
            });

            videos.Add(new Video()
            {
                title = "ProgressNEXT 2018 Highlights",
                poster = "https://img.youtube.com/vi/DYsiJRmIQZw/1.jpg",
                source = "https://www.youtube.com/watch?v=Gp7tcOcSKAU"
            });

            videos.Add(new Video()
            {
                title = "Kendo UI Testimonial",
                poster = "https://img.youtube.com/vi/gNlya720gbk/1.jpg",
                source = "https://www.youtube.com/watch?v=itoKeywVNBI"
            });

            videos.Add(new Video()
            {
                title = "Introducing Test Studio DevEdition",
                poster = "https://img.youtube.com/vi/rLtTuFbuf1c/1.jpg",
                source = "https://www.youtube.com/watch?v=A2rmIx9rPG0"
            });

            videos.Add(new Video()
            {
                title = "Progress Application Server OpenEdge",
                poster = "https://i.ytimg.com/vi/CpHKm2NruYc/1.jpg",
                source = "https://www.youtube.com/watch?v=3Ce11N9udR4&list=PLC679RvCan2BJ9HCdUyZhnhHKActnrape"
            });

            return videos;
        }
    }
}