using GoStay.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public interface IMyTypedClientServices
    {
        public  UploadImagesResponse PostImgAndGetData(List<IFormFile> files, int width, string? Obj_Id, int type);
        public UploadImagesResponse PostVideoAndGetData(List<IFormFile> files, string title, int type, int IdUser);


    }
}
