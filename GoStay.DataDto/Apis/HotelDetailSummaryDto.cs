using GoStay.DataDto.RatingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Apis
{
    public class HotelDetailSummaryDto
    {
        public HotelDetailDto HotelDetailDto { get; set; }
        public List<UserBoxReview> UserBoxReviews { get; set; }
    }
}
