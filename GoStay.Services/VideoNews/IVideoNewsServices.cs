using GoStay.Common;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public interface IVideoNewsServices
    {
        public PagingList<VideoNews> GetVideoNewsList(FilterBase filter, int type, int idUser);
        public int GetPictureIdbyUrl(string Url);

        public string? AddVideoNews(VideoNews data);
        public string? EditPicture(VideoNews data);
        public string? DeletePicture(int Id);
        public string? UpdateClick(int Id);
    }
}
