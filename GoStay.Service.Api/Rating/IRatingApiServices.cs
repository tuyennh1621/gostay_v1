
using GoStay.Common;
using GoStay.DataDto.RatingDto;

namespace GoStay.Service.Api.Rating
{
    public interface IRatingApiServices
    {
        
        public Common.ResponseBase<UpdateRatingResponse> ReviewOrUpdateScore(RatingOrUpdateDto dto);
        public Common.ResponseBase<GetRatingDto> GetRatingByUser(int hotelId, int userId);
        Common.ResponseBase<List<GetRatingByHotelDto>> GetRatingByHotel(int hotelId);
        public Common.ResponseBase<List<UserBoxReview>> GetUserBoxReview(int idHotel);
        public Common.ResponseBase<int> CheckOrdered(int hotelId, int userId);
        public Common.ResponseBase<string> UpdateStatusRating(int Id, byte Status);
        public Common.ResponseBase<List<RatingAdminDto>> GetListRating(int? HotelId, byte? Status, string? NameSearch, int PageIndex, int PageSize);


    }
}