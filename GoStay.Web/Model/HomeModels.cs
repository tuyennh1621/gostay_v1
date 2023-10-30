using GoStay.DataDto.Apis;
using GoStay.DataDto.Banner;
using GoStay.DataDto.News;
using GoStay.DataDto.TourDto;

namespace GoStay.Web.Model
{
    public class HomeModels
    {
        public string? CheckinDate { get; set; }
        public string? CheckoutDate { get; set; }
        public List<HotelHomePageDto>? ListTopSliderHotel { get; set; }
        public BannerPageDto BannerHomePage { get; set; }
        public List<DataDto.Apis.HotelHomePageDto> ListHotelHomepage { get; set; }
        public List<SearchTourDto> ListTourHomepage { get; set; }
        public List<NewSearchOutDto> ListTopNews { get; set; }
        public List<VideoNewsDto> video { get; set; } // Video
    }
}
