using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Apis;

namespace GoStay.Web.Areas.Admin.Model
{
    public class PictureModels
    {
        public class AddPictureModels
        {
            public PagingList<Picture>? Picture { get; set; }
            public int IdType { get; set; }
            public int style { get; set; }
        }
    }
}
