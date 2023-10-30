using GoStay.DataDto.ServiceDto;

namespace GoStay.DataDto.Apis
{
    public class HotelDetailDto
    {
        public int Id { get; set; }
        public string? HotelName { get; set; }
        public string? Slug { get; set; }
        public string? HotelAddress { get; set; }
        public int? Rating { get; set; }
        public decimal? AvgNight { get; set; }
        public double Review_score { get; set; }
        public decimal? ServiceScore { get; set; }
        public decimal? ValueScore { get; set; }
        public decimal? SleepQualityScore { get; set; }
        public decimal? CleanlinessScore { get; set; }
        public decimal? LocationScore { get; set; }
        public decimal? RoomsScore { get; set; }
        public string? Content { get; set; }
        public string? TinhThanh { get; set; }
        public string? QuanHuyen { get; set; }
        public string? TinhThanh_url { get; set; }
        public string? QuanHuyen_url { get; set; }
        public double? Lat_map { get; set; }
        public double? Lon_map { get; set; }
        public double? Discount { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal ActualPrice { get; set; }
        public int? NumberReviewers { get; set; }
        public List<HotelRoomDto> Rooms { get; set; }
        public List<ServiceDetailHotelDto> Services { get; set; }
        public List<string> Pictures { get; set; } = new List<string>();
        public int TotalPicture { get; set; }
        public int Ordered { get; set; }


    }
}
