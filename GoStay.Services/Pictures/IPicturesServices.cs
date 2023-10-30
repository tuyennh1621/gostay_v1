
using GoStay.Common;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public interface IPicturesServices
    {
        public PagingList<Picture> GetPicturesList(FilterBase filter, int type);
        public int GetPictureIdbyUrl(string Url);

        public string? AddNewPicture(Picture data);
        public string? EditPicture(Picture data);
        public string? DeletePicture(int Id);

    }
}
