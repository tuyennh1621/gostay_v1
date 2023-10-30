
using AutoMapper;
using DAL.Entities;
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.Utilities;
using GoStay.DataDto.News;
using System.Text.RegularExpressions;

namespace GoStay.Service
{
    public class NewsServices : INewsServices
    {
        private readonly ICommonRepository<News> _newsRepository;
        private readonly ICommonRepository<NewsCategory> _newsCategoryRepository;
        private readonly ICommonRepository<TopicNews> _topicRepository;
        private readonly ICommonRepository<Language> _languageRepository;
        private readonly ICommonRepository<NewsTopic> _newsTopicRepository;
        private readonly ICommonRepository<VideoNews> _videoNewsRepository;

        private readonly ICommonUoW _commonUoW;
        private readonly IMapper _mapper;

        public DateTime currentDate = DateTime.Now;
        public NewsServices(ICommonRepository<News> newsRepository, ICommonUoW commonUoW, IMapper mapper, ICommonRepository<VideoNews> videoNewsRepository,
             ICommonRepository<NewsCategory> newsCategoryRepository, ICommonRepository<TopicNews> topicRepository, 
             ICommonRepository<Language> languageRepository, ICommonRepository<NewsTopic> newsTopicRepository)
        {
            _newsRepository = newsRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
            _newsCategoryRepository = newsCategoryRepository;
            _topicRepository = topicRepository;
            _languageRepository = languageRepository;
            _newsTopicRepository = newsTopicRepository;
            _videoNewsRepository = videoNewsRepository;
        }

        public IQueryable<NewsCategory> GetNewsCategories()
        {
            var listCategories = _newsCategoryRepository.FindAll(x=>x.Iddomain==1);
            return listCategories;
        }
        public IQueryable<TopicNews> GetNewsTopic()
        {
            var listTopic = _topicRepository.FindAll(x => x.Iddomain == 1);
            return listTopic;
        }
        public IQueryable<Language> GetNewsLanguage()
        {
            var listTopic = _languageRepository.FindAll();
            return listTopic;
        }
        

        public Dictionary<int, NewsTopicTotal> GetNewsTopicTotal()
        {
            Dictionary<int, NewsTopicTotal> data = new Dictionary<int, NewsTopicTotal>();
            var listTopic = _topicRepository.FindAll().Where(x => x.Iddomain == 1).Select(x => new { x.Id, x.Topic }).ToList();
            var newsTopic = _newsTopicRepository.FindAll().ToList();
            for (var i = 0; i < listTopic.Count; i++)
            {
                var total = newsTopic.Where(x=>x.IdNewsTopic == listTopic[i].Id).Count();
                data.Add(i + 1, new NewsTopicTotal() { Id = listTopic[i].Id, Topic = listTopic[i].Topic, Total = total, Slug = listTopic[i].Topic.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower()}); 
            }

            return data;
        }
        public string GetNewsTopicTitle(int Id)
        {
            var title = _topicRepository.FindAll(x => x.Id == Id).SingleOrDefault().Topic;
            return title;
        }

        public Dictionary<int, VideoNewsDto> GetTotalByCate()
        {
            var data = _videoNewsRepository.FindAll(x => x.Deleted == 0 && x.Status == 1).OrderBy(x => x.Id).ToList();
            var listCategories = _newsCategoryRepository.FindAll().Select(x => new { x.Id, x.Category }).ToList();
            Dictionary<int, VideoNewsDto> result = new Dictionary<int, VideoNewsDto>();
            if (listCategories != null)
            {
                for (var i = 0; i < listCategories.Count; i++)
                {
                    var total = data.Where(x => x.IdCategory == listCategories[i].Id).Count();
                    result.Add(i + 1, new VideoNewsDto() { Id = listCategories[i].Id, Category = listCategories[i].Category, Total = total, Slug = listCategories[i].Category.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower() });
                }
            }
            return result;
        }
    }

    
}

