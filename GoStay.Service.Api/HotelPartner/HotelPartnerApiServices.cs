using GoStay.Common;
using GoStay.DataDto.HotelDto;
using GoStay.Service.Api.Base;

namespace GoStay.Service.Api.HotelPartner
{
    public class HotelPartnerApiServices : ApiServiceBase, IHotelPartnerApiServices
    {

        public Common.ResponseBase<int> AddRoom(RoomAddDto roomDto)
        {
            var response = Post<RoomAddDto, int>("hotel/add-room", roomDto);
            return response;
        }
        public Common.ResponseBase<PagingList<HotelDto>> GetHotelList(RequestGetListHotel request)
        {
            var response = Post<RequestGetListHotel, PagingList<HotelDto>>("hotel/list", request);
            return response;
        }
    }
}