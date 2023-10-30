using GoStay.DataAccess.Entities;
using GoStay.DataDto.Apis;
using GoStay.DataDto.HotelDto;

namespace GoStay.Service.Api.Hotels
{
    public interface IHotelApiServices
    {

        List<DataDto.Apis.HotelHomePageDto> GetListHotelTopFlashSale();
        HotelDetailDto GetHotelDetail(int hotelId);
        List<DataDto.Apis.HotelHomePageDto> GetListHotelForSearch(HotelSearchRequest request);
        GoStay.Common.ResponseBase<List<SuggestResultDto>> GetListSuggestHotel(string keyword);
        public List<TypeHotel> GetAllTypeHotel();
        public List<DataAccess.Entities.Service> GetServicesSearch(int type);
        public Common.ResponseBase<List<ServiceRoomDto>> GetServicesRoom(int IdRoom);
        public Common.ResponseBase<List<PictureRoomDto>> GetPicturesRoom(int IdRoom);
        public Common.ResponseBase<List<PictureRoomDto>> GetPicturesHotel(int IdHotel);
        public Common.ResponseBase<string> SetMapHotel(SetMapHotelRequest request);
        public Common.ResponseBase<List<DataDto.Apis.HotelHomePageDto>> GetHotelHomePage(int IdProvince);
        public GoStay.Common.ResponseBase<List<SuggestResultDto>> GetListNearHotel(int NumTop,float Lat, float Lon);
        public Common.ResponseBase<List<RoomAdminDto>> GetListRoomAdmin(RequestGetListRoomAdmin request);

        public Common.ResponseBase<string> ChangeRoomStatus(int IdRoom, int Status);
        public Common.ResponseBase<string> ChangeStatusRoom(int IdRoom, int RoomStatus);

        public HotelDetailSummaryDto GetHotelDetailSummary(int hotelId, int userId);
    }
}