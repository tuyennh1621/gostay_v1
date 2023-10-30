using GoStay.DataAccess.Entities;
using GoStay.DataDto.Apis;
using GoStay.DataDto.HotelDto;
using GoStay.Service.Api.Base;
using System.Globalization;
using System.Web.Razor.Tokenizer.Symbols;

namespace GoStay.Service.Api.Hotels
{
    public class HotelApiServices : ApiServiceBase, IHotelApiServices
    {

        public List<DataDto.Apis.HotelHomePageDto> GetListHotelTopFlashSale()
        {
            var response = Get<List<DataDto.Apis.HotelHomePageDto>>("hotels/hotel-top-flash-sale", new KeyValuePair<string, object>("number", 15));

            return response.Data;
        }
        public HotelDetailSummaryDto GetHotelDetailSummary(int hotelId, int userId)
        {
            var response = Get<HotelDetailSummaryDto>("hotels/hotel-detail-summary", 
                new KeyValuePair<string, object>("hotelId", hotelId),
                new KeyValuePair<string, object>("userId", userId)
                );
            return response.Data;
        }
        public HotelDetailDto GetHotelDetail(int hotelId)
        {
            var response = Get<HotelDetailDto>("hotels/hotel-detail", new KeyValuePair<string, object>("hotelId", hotelId));
            return response.Data;
        }
        public List<DataDto.Apis.HotelHomePageDto> GetListHotelForSearch(HotelSearchRequest request)
        {
            var response = Post<HotelSearchRequest, List<DataDto.Apis.HotelHomePageDto>>("hotels/hotel-search", request);
            return response.Data;
        }

        public GoStay.Common.ResponseBase<List<SuggestResultDto>> GetListSuggestHotel(string keyword)
        {
            var response = Get<List<SuggestResultDto>>("hotels/hotel-suggest", new KeyValuePair<string, object>("search", keyword));
            return response;
        }

        public GoStay.Common.ResponseBase<List<SuggestResultDto>> GetListNearHotel(int NumTop,float Lat, float Lon)
        {
            var response = Get<List<SuggestResultDto>>("hotels/hotel-near",
                new KeyValuePair<string, object>("NumTop", NumTop),
                new KeyValuePair<string, object>("Lat", Lat.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, object>("Lon", Lon.ToString(CultureInfo.InvariantCulture)));
            return response;
        }

        public List<TypeHotel> GetAllTypeHotel()
        {
            var response = Get<List<TypeHotel>>("hotels/type-hotel");
            return response.Data;
        }
        public List<DataAccess.Entities.Service> GetServicesSearch(int type)
        {
            var response = Get<List<DataAccess.Entities.Service>>("hotels/services-search", new KeyValuePair<string, object>("type", type));
            return response.Data;
        }
        public Common.ResponseBase<List<PictureRoomDto>> GetPicturesRoom(int IdRoom)
        {
            var response = Get<List<PictureRoomDto>>("hotel/pictures-room", new KeyValuePair<string, object>("IdRoom", IdRoom));
            return response;
        }
        public Common.ResponseBase<List<PictureRoomDto>> GetPicturesHotel(int IdHotel)
        {
            var response = Get<List<PictureRoomDto>>("hotel/pictures-hotel", new KeyValuePair<string, object>("IdHotel", IdHotel));
            return response;
        }
        public Common.ResponseBase<List<ServiceRoomDto>> GetServicesRoom(int IdRoom)
        {
            var response = Get<List<ServiceRoomDto>>("hotel/services-room", new KeyValuePair<string, object>("IdRoom", IdRoom));
            return response;
        }

        public Common.ResponseBase<string> SetMapHotel(SetMapHotelRequest request)
        {
            var response = Put<SetMapHotelRequest, string>("hotel/set-map", request);

            return response;
        }

        public Common.ResponseBase<List<DataDto.Apis.HotelHomePageDto>> GetHotelHomePage(int IdProvince)
        {
            var response = Get<List<DataDto.Apis.HotelHomePageDto>>("hotels/hotel-homepage", new KeyValuePair<string, object>("IdProvince", IdProvince));
            return response;
        }
        public Common.ResponseBase<List<RoomAdminDto>> GetListRoomAdmin(RequestGetListRoomAdmin request)
        {
            var response = Post<RequestGetListRoomAdmin, List<RoomAdminDto>>("hotel/list-room-admin", request);
            return response;
        }

        public Common.ResponseBase<string> ChangeRoomStatus(int IdRoom, int Status)
        {
            var response = Get<string>("hotel/change-room-status", 
                new KeyValuePair<string, object>("IdRoom", IdRoom),
                new KeyValuePair<string, object>("Status", Status));

            return response;
        }
        public Common.ResponseBase<string> ChangeStatusRoom(int IdRoom, int RoomStatus)
        {
            var response = Get<string>("hotel/change-status-room",
                new KeyValuePair<string, object>("IdRoom", IdRoom),
                new KeyValuePair<string, object>("RoomStatus", RoomStatus));

            return response;
        }
    }
}