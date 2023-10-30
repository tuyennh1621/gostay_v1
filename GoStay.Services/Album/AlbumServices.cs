using AutoMapper;
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.Repositories;
using GoStay.DataDto.Album;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public class AlbumServices : IAlbumServices
    {
        private readonly ICommonRepository<Album> _AlbumRepository;
        private readonly ICommonUoW _commonUoW;
        private IMapper _mapper;
        public AlbumServices(ICommonRepository<Album> AlbumRepository, ICommonUoW commonUoW, IMapper mapper)
        {
            _AlbumRepository = AlbumRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
        }
        public IQueryable<Album> GetAllAlbum(int type ,int Id=0 )
        {
            if (Id == 0)
            {
                var listAlbum = _AlbumRepository.FindAll(x => x.Deleted != 1 && x.TypeAlbum == type);
                return listAlbum;
            }
            else
            {
                var listAlbum = _AlbumRepository.FindAll(x => x.IdType == Id && x.TypeAlbum == type&&  x.Deleted != 1);
                return listAlbum;
            }
        }

        public Album GetAlbumByID(int? Id)
        {
            var Album = _AlbumRepository.FindAll(x=>x.Id == Id&&x.Deleted !=1)
                 .FirstOrDefault();
            return Album ;
        }
        public int GetmaxID(int IDHotel)
        {
            var max = _AlbumRepository.FindAll(x => x.IdType == IDHotel).Max();
            return max.Id;
        }
        //public Album GetAlbumRoom(int Id)
        //{
        //    var Album = _AlbumRepository.FindAll(x => x.Id == Id && x.Deleted != 1)
        //            .Include(x => x.IdGalleryRoomNavigation.IdRoomNavigation.Name).FirstOrDefault();

        //    return Album;
        //}

        public PagingList<Album> GetAlbumList(FilterBase filter)
        {
            var listAlbum = _AlbumRepository.FindAll(x => x.Deleted != 1);

            return listAlbum.ConvertToPaging(filter.PageSize, filter.PageIndex);
        }

        public string? AddNewAlbum(AlbumDto data)
        {
            try
            {
                var album = _mapper.Map<AlbumDto,Album>(data);
                _commonUoW.BeginTransaction();
                _AlbumRepository.Insert(album);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditAlbum(Album data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _AlbumRepository.Update(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeleteAlbum(int id)
        {
            try
            {
                var entity = _AlbumRepository.GetById(id);
                _commonUoW.BeginTransaction();
                _AlbumRepository.SoftDelete(entity);
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
