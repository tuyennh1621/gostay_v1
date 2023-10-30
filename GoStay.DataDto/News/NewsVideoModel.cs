using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.News
{
    public class NewsVideoModel
    {
        public string Video { get; set; } = null!;
        public int? IdCategory { get; set; }
        public int? LangId { get; set; }
        public string? Title { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public int? Status { get; set; }
        public string? PictureTitle { get; set; }
        public string? Descriptions { get; set; }
    }
}
