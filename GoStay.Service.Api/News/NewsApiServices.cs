using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.News;
using GoStay.DataDto.TourDto;
using GoStay.Service.Api.Base;
using Microsoft.AspNetCore.Mvc;

namespace GoStay.Service.Api.News
{
    public class NewsApiServices : ApiServiceBase, INewsApiServices
    {

        public ResponseBase<List<NewSearchOutDto>> GetListNews(GetListNewsParam param)
        {
            var response = Post<GetListNewsParam, List<NewSearchOutDto>>("News/list", param);
            return response;
        }
        public ResponseBase<List<NewSearchOutDto>> GetListTopNewsbyCategory(int? IdCategory, int? IdTopic)
        {
            var response = Get<List<NewSearchOutDto>>("News/top-news", new KeyValuePair<string, object>("IdCategory", IdCategory),
                                                                        new KeyValuePair<string, object>( "IdTopic", IdTopic));
            return response;
        }
        public ResponseBase<List<VideoNewsDto>> GetListVideoNews(int UserId, int status)
        {
            var response = Get<List<VideoNewsDto>>("News/list-video-news", new KeyValuePair<string, object>("UserId", UserId),
                                                                            new KeyValuePair<string, object>("status", status));
            return response;
        }
        public ResponseBase<List<VideoNewsDto>> GetListVideo(GetListVideoNewsParam param)
        {
            var response = Post<GetListVideoNewsParam, List<VideoNewsDto>>("News/list-video-news", param);
            return response;
        }
        public ResponseBase<NewsDetailDto> GetNews(int  Id)
        {
            var response = Get<NewsDetailDto>("News/news", new KeyValuePair<string, object>("Id", Id));
            return response;
        }

        public ResponseBase<int> AddNews(NewsDto newsDto)
        {
            var response = Post<NewsDto, int>("News/add-news", newsDto);
            return response;
        }
        public ResponseBase<int> AddVideoNews(NewsVideoModel videoNewsDto)
        {
            var response = Post<NewsVideoModel, int>("News/add-video-news", videoNewsDto);
            return response;
        }

        public ResponseBase<int> EditNews(NewsDto newsDto)
        {
            var response = Put<NewsDto, int>("News/edit-news", newsDto);
            return response;
        }
        public ResponseBase<int> EditContentNews(EditNewsContentParam param)
        {
            var response = Put<EditNewsContentParam, int>("News/edit-content-news", param);
            return response;
        }
        public ResponseBase<int> EditPictureTitleNews(EditNewsPictureTitleParam param)
        {
            var response = Put<EditNewsPictureTitleParam, int>("News/edit-picturetitle-news", param);
            return response;
        }
        public ResponseBase<string> DeleteNews(int Id)
        {
            var response = Put<int,string>("News/delete-news", Id);
            return response;
        }

        public ResponseBase<Dictionary<int, List<NewSearchOutDto>>> GetListNewsHomePage()
        {
            var response = Get<Dictionary<int, List<NewSearchOutDto>>> ("News/list-homepage");
            return response;
        }
        public ResponseBase<int> EditClick(int Id)
        {
            var response = Put<int, int>("News/click", Id);
            return response;
        }
        public ResponseBase<int> EditVideo(VideoNews VideoNews)
        {
            var item = Put<VideoNews, int>("News/edit-video-news", VideoNews);
            return item;
        }
        public ResponseBase<int> DeleteVideo(int Id)
        {
            var item = Put<int, int>("News/delete-video-news", Id);
            return item;
        }
        public ResponseBase<VideoNewsDetailDto> GetVideoNews(int Id)
        {
            var response = Get<VideoNewsDetailDto>("News/video-news", new KeyValuePair<string, object>("Id", Id));
            return response;
        }
    }
}