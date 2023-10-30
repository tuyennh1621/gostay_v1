using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Album;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public interface IAlbumServices
    {
        public string? AddNewAlbum(AlbumDto data);
        public string? EditAlbum(Album FormData);
        public PagingList<Album> GetAlbumList(FilterBase filter);
        public IQueryable<Album> GetAllAlbum(int type,int Id);
        public string? DeleteAlbum(int Id);
        public Album GetAlbumByID(int? Id);
        //public Album GetAlbumRoom(int Id);
        //public string GetAlbumName(int Id);
    }
}
