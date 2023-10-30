namespace GoStay.DataDto.Apis
{
    public class HotelHomePageDto
    {
        public int Id { get; set; }
        public string HotelName { get; set; }
        public string Slug { get; set; }
        public string TinhThanh { get; set; }
        public string QuanHuyen { get; set; }
        public int? Rating { get; set; }
        public decimal? AvgNight { get; set; }
        public double Review_score { get; set; }
        public double? Lat_map { get; set; }
        public double? Lon_map { get; set; }
        public double? Discount { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal ActualPrice { get; set; }
        public int? NumberReviewers { get; set; }
        public int? PalletbedRoom { get; set; }
        public int Total { get; set; }
        public int TotalPicture { get; set; }

        public long? IntDate { get; set; }

        public List<string> Pictures { get; set; } = new List<string>();
        public DateTime LastOrderTime { get; set; }
        public TimeSpan LastOrderDateTemp
        {
            get
            {
                return (DateTime.Now - LastOrderTime);
            }
               
        }
        public string? LastOrderDate { get; set; }

        public int IdRoom { get; set; }
        public double DailyPrice { get; set; }
        public decimal DailyBasePrice
        {
            get;set;
        }
        public string? TopService { get; set; }

    }
}
