using GoStay.Common;
using GoStay.DataDto.News;

namespace GoStay.Web.ViewsModel
{
    public class NewsViewsModel
    {
        public ResponseBase<List<NewSearchOutDto>> topNews { get; set; } // tin tuc moi nhat
        public ResponseBase<Dictionary<int, List<NewSearchOutDto>>> listCategoriesNews { get; set; }
        public List<NewSearchOutDto> foodNews { get; set; } // am thuc
        public List<NewSearchOutDto> saleslNews { get; set; }   // tin khuyen mai
        public List<NewSearchOutDto> festivallNews { get; set; }   // văn hóa lễ hội
        public List<NewSearchOutDto> courtesyNews { get; set; } // tin tức
        public List<NewSearchOutDto> cultureNews { get; set; }  // van hoa phong tuc
        public List<NewSearchOutDto> interviewNews { get; set; } // phong su anh
        public List<NewSearchOutDto> travelNews { get; set; } // tin tuc du lich
        public ResponseBase<List<NewSearchOutDto>> topCultureNews { get; set; } // top van hoa phong tuc
        public ResponseBase<List<NewSearchOutDto>> topFoodNews { get; set; } // top am thuc
        public ResponseBase<List<VideoNewsDto>> video { get; set; } // Video
    }
}
