
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.News;

namespace GoStay.Service
{
    public interface INewsServices
    {
        public IQueryable<NewsCategory> GetNewsCategories();
        public IQueryable<TopicNews> GetNewsTopic();
        public IQueryable<Language> GetNewsLanguage();
        public Dictionary<int, NewsTopicTotal> GetNewsTopicTotal();
        public string GetNewsTopicTitle(int Id);
        public Dictionary<int, VideoNewsDto> GetTotalByCate();

    }

}
