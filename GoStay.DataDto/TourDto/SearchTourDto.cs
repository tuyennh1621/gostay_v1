

namespace GoStay.DataDto.TourDto
{
    public class SearchTourDto
    {
        public int Id { get; set; }
        public int IdTourStyle { get; set; }
        public int IdTourTopic { get; set; }
        public string TourName { get; set; }
        public string Slug { get; set; }
        public string TourStyle { get; set; }
        public string TourTopic { get; set; }
        public int Rating { get; set; }
        public int Style { get; set; }

        public int TourSize { get; set; }

        public string? ProvinceFrom { get; set; }
        public List<string>? ProvinceTo { get; set; }

        public int IdDistrictFrom { get; set; }
        public string DistrictFrom { get; set; }

        public List<int> IdDistrictTo { get; set; }
        public List<string> DistrictTo { get; set; }

        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }

        public double Price { get; set; }
        public int Discount { get; set; }
        public double ActualPrice { get; set; }
        public int Total { get; set; }
        public int TotalPicture { get; set; }

        public List<string>? Pictures { get; set; }
    }
    public class InfoTourInList
    {
        public int Id { get; set; }
        public string TourName { get; set; }
        public string Slug { get; set; }
        public string TourStyle { get; set; }
        public string TourTopic { get; set; }
        public int Rating { get; set; }
        public int TourSize { get; set; }
        public string ProvinceFrom { get; set; }
        public List<string>? ProvinceTo { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public double ActualPrice { get; set; }
        public int Total { get; set; }
        public int TotalPicture { get; set; }
        public List<string>? Pictures { get; set; }
    }

}
