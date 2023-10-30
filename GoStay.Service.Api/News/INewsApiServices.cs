

using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.News;

namespace GoStay.Service.Api.News
{
    public interface INewsApiServices
    {
        public ResponseBase<List<NewSearchOutDto>> GetListNews(GetListNewsParam param);
        public ResponseBase<List<NewSearchOutDto>> GetListTopNewsbyCategory(int? IdCategory, int? IdTopic);
        public ResponseBase<Dictionary<int, List<NewSearchOutDto>>> GetListNewsHomePage();
        public ResponseBase<NewsDetailDto> GetNews(int Id);
        public ResponseBase<int> AddNews(NewsDto newsDto);
    
        public ResponseBase<int> EditNews(NewsDto newsDto);
        public ResponseBase<string> DeleteNews(int Id);
        public ResponseBase<int> EditContentNews(EditNewsContentParam param);
        public ResponseBase<int> EditPictureTitleNews(EditNewsPictureTitleParam param);
        public ResponseBase<int> EditClick(int Id);
        public ResponseBase<List<VideoNewsDto>> GetListVideoNews(int UserId, int status);
        public ResponseBase<List<VideoNewsDto>> GetListVideo(GetListVideoNewsParam param);
        public ResponseBase<int> AddVideoNews(NewsVideoModel videoNewsDto);

        public ResponseBase<int> EditVideo(VideoNews VideoNews);
        public ResponseBase<int> DeleteVideo(int Id);
        public ResponseBase<VideoNewsDetailDto> GetVideoNews(int Id);

    }
}