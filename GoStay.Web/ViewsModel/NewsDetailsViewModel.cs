using GoStay.Common;
using GoStay.DataDto.News;

namespace GoStay.Web.ViewsModel
{
    public class NewsDetailsViewModel
    {
        public ResponseBase<NewsDetailDto>  newDetails { get; set; }
        public ResponseBase<List<NewSearchOutDto>> newTopic { get; set; }
    }
}
