using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public class VideoNewsServices : IVideoNewsServices
    {
        private readonly ICommonRepository<VideoNews> _videoNewsRepository;

        private readonly ICommonUoW _commonUoW;
        public VideoNewsServices(ICommonRepository<VideoNews> VideoNewsRepository, ICommonUoW commonUoW)
        {
            _videoNewsRepository = VideoNewsRepository;
            _commonUoW = commonUoW;
        }

        public PagingList<VideoNews> GetVideoNewsList(FilterBase filter, int type, int idUser)
        {
            if (filter.IdObj == 0)
            {
                var listPictures = _videoNewsRepository.FindAll(x => x.Deleted != 1 && x.Status == 0 && x.IdUser == idUser).OrderByDescending(x => x.Id);

                return listPictures.ConvertToPaging(filter.PageSize, filter.PageIndex);
            }
            else
            {
                var listPictures = _videoNewsRepository.FindAll(x => x.Status == 0 && x.Deleted != 1)
                                                                .OrderByDescending(x => x.Id);

                return listPictures.ConvertToPaging(filter.PageSize, filter.PageIndex);
            }
        }

        public int GetPictureIdbyUrl(string Url)
        {
            try
            {
                var Id = _videoNewsRepository.FindAll(x => x.Video == Url).SingleOrDefault().Id;
                return Id;
            }
            catch
            {
                return 0;
            }
        }

        public string? AddVideoNews(VideoNews data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _videoNewsRepository.Insert(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditPicture(VideoNews data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _videoNewsRepository.Update(data);
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
                var entity = _videoNewsRepository.GetById(Id);
                _commonUoW.BeginTransaction();
                _videoNewsRepository.SoftDelete(entity);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.DeleteFail.Value}";
            }
        }
        public string? UpdateClick(int Id)
        {
            try
            {
                var entity = _videoNewsRepository.GetById(Id);
                entity.Click = (entity.Click != null) ? (entity.Click + 1 ): 1;
                _commonUoW.BeginTransaction();
                _videoNewsRepository.Update(entity);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }
    }
}
