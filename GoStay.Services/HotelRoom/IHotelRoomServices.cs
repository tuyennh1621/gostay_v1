
using GoStay.Common;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Service
{
    public interface IHotelRoomServices
    {
        public string? EditHotelRoom(HotelRoom data, int[] view, int[] service);
        //public PagingList<HotelRoom> GetHotelRoomList(FilterBase filter);
        public HotelRoom GetHotelRoomById(int Id);
        public IQueryable<HotelRoom> GetAllHotelRoom();
        public IQueryable<Palletbed> GetListPalletbed();

        public string? DeleteHotelRoom(int Id);
        public string? AddNewHotelRoom(HotelRoom data, int[] view, int[] service);
        public IQueryable<HotelRoom> GetHotelRoomList(FilterBase filter);
        public int[]? GetListRoomView(int IdRoom);
        public int[]? GetListRoomService(int IdRoom);

    }
}
