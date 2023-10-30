using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Apis;
using GoStay.DataDto.HotelDto;

namespace GoStay.Service.Api.HotelPartner
{
    public interface IHotelPartnerApiServices
    {
        public Common.ResponseBase<int> AddRoom(RoomAddDto roomDto);
        public Common.ResponseBase<PagingList<HotelDto>> GetHotelList(RequestGetListHotel request);


    }
}