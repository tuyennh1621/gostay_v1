
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.HotelDto;

namespace GoStay.Service
{
    public interface IHotelServices
    {
        public string? AddNewHotel(Hotel data);
        public string? EditHotel(Hotel FormData);
        public HotelListAdmin GetHotelList(GetHotelAdminParam request);
        public Hotel GetHotelById(int Id);
        public IQueryable<Hotel> GetAllHotel();
        public string? DeleteHotel(int Id);
        public PagingList<Hotel> GetHotelByTinhThanh(FilterBase filter);
        public string SetMapHotel(SetMapHotelRequest param);
    }

}
