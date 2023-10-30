using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Apis;
using GoStay.DataDto.RatingDto;
using GoStay.Service.Api.Base;
using System;

namespace GoStay.Service.Api.Rating
{
    public class RatingApiServices : ApiServiceBase, IRatingApiServices
    {

        public Common.ResponseBase<UpdateRatingResponse> ReviewOrUpdateScore(RatingOrUpdateDto dto)
        {
            var response = Post<RatingOrUpdateDto, UpdateRatingResponse>("rating/Rating", dto);

            return response;
        }

        public Common.ResponseBase<GetRatingDto> GetRatingByUser(int hotelId, int userId)
        {
            var response = Get<GetRatingDto>("rating/rating-by-user", new KeyValuePair<string, object>("hotelId", hotelId),
                new KeyValuePair<string, object>("userId", userId));
            return response;
        }

        public Common.ResponseBase<int> CheckOrdered(int hotelId, int userId)
        {
            var response = Get<int>("rating/ordered", new KeyValuePair<string, object>("hotelId", hotelId),
                new KeyValuePair<string, object>("userId", userId));
            return response;
        }

        public Common.ResponseBase<List<GetRatingByHotelDto>> GetRatingByHotel(int hotelId)
        {
            var response = Get< List<GetRatingByHotelDto> > ("rating/rating-by-hotel", new KeyValuePair<string, object>("hotelId", hotelId));
            return response;
        }
        public Common.ResponseBase<List<UserBoxReview>> GetUserBoxReview(int idHotel)
        {
            var response = Get<List<UserBoxReview>>("rating/user-box-review"
                , new KeyValuePair<string, object>("idHotel", idHotel)
                );
            return response;
        }

        public Common.ResponseBase<string> UpdateStatusRating(int Id, byte Status)
        {
            var response = Get<string>("rating/update-status-rating"
                , new KeyValuePair<string, object>("Id", Id)
                , new KeyValuePair<string, object>("Status", Status)
                );
            return response;
        }
        public Common.ResponseBase<List<RatingAdminDto>> GetListRating(int? HotelId, byte? Status,string? NameSearch, int PageIndex, int PageSize)
        {
            var response = Get<List<RatingAdminDto>>("rating/list-rating"
                , new KeyValuePair<string, object>("HotelId", HotelId)
                , new KeyValuePair<string, object>("Status", Status)
                , new KeyValuePair<string, object>("NameSearch", NameSearch)
                , new KeyValuePair<string, object>("PageIndex", PageIndex)
                , new KeyValuePair<string, object>("PageSize", PageSize)

                );
            return response;
        }
    }
}