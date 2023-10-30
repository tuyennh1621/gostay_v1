using GoStay.Common;
using GoStay.DataDto.News;

namespace GoStay.Web.ViewsModel
{
    public class VideoNewsViewsModel
    {
        public ResponseBase<List<VideoNewsDto>> AllVideo { get; set; } // AllVideo
        public ResponseBase<List<VideoNewsDto>> AllVideoByCate { get; set; } // AllVideo theo danh muc
        public ResponseBase<VideoNewsDetailDto> VideoNewsDetail { get; set; } // chi tiet video
    }
}
