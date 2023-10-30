
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public interface IBannerServices
    {
        public string? AddNewBanner(Banner data);
        public string? EditBanner(Banner FormData);
        public string? DeleteBanner(int Id);


    }
}
