
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoStay.Service
{
    public class PicturesServices : IPicturesServices
    {
        private readonly ICommonRepository<Picture> _PictureRepository;

        private readonly ICommonUoW _commonUoW;
        public PicturesServices(ICommonRepository<Picture> PictureRepository, ICommonUoW commonUoW)
        {
            _PictureRepository = PictureRepository;
            _commonUoW = commonUoW;
        }

        public PagingList<Picture> GetPicturesList(FilterBase filter,int type)
        {
            if (filter.IdObj == 0)
            {
                var listPictures = _PictureRepository.FindAll(x => x.Deleted != 1 && x.Type == type).OrderByDescending(x => x.Id);

                return listPictures.ConvertToPaging(filter.PageSize, filter.PageIndex);
            }
            else
            {
                var listPictures = _PictureRepository.FindAll(x => x.IdType == filter.IdObj&& x.Deleted != 1 && x.Type == type)
																.OrderByDescending(x => x.Id);

                return listPictures.ConvertToPaging(filter.PageSize, filter.PageIndex);
            }
        }

        public int GetPictureIdbyUrl(string Url)
        {
            try
            {
                var Id = _PictureRepository.FindAll(x => x.Url == Url).SingleOrDefault().Id;
                return Id;
            }
            catch
            {
                return 0;
            }
        }

        public string? AddNewPicture(Picture data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _PictureRepository.Insert(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditPicture(Picture data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _PictureRepository.Update(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeletePicture(int Id)
        {
            try
            {
                var entity = _PictureRepository.GetById(Id);
                _commonUoW.BeginTransaction();
                _PictureRepository.SoftDelete(entity);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.DeleteFail.Value}";
            }
        }
    }
}
