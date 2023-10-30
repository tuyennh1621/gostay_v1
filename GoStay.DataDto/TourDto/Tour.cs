using GoStay.DataAccess.Entities;

namespace GoStay.DataDto.TourDto
{
    
    public class TourDetailDto
    {
        public int Id { get; set; }
        public int IdTours { get; set; }

        public byte? IdStyle { get; set; }
        public string? Title { get; set; }
        public string? Details { get; set; }
        public byte Stt { get; set; }

    }
    public class TourAddDto
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }

        public string? TourName { get; set; }
        public byte IdTourStyle { get; set; }
        public byte IdTourTopic { get; set; }
        public string StartDateString { get; set; }
        public int IdDistrictFrom { get; set; }
        public int[] IdDistrictTo { get; set; }
        public double Price { get; set; }
        public byte? Discount { get; set; }
        public string? Content { get; set; }
        public int TourSize { get; set; }
        public string? Locations { get; set; }
        public int Style { get; set; }
        public double ActualPrice { get; set; }
        public int[]? Vehicle { get; set; }
        public int? Rating { get; set; }
        public string? Descriptions { get; set; }
        public int? IdStartTime { get; set; }

    }

    public class CompareTourParam
    {
        public string IdTour { get; set; }
        public int IdUser { get; set; }
        public string Session { get; set; }
    }
    public class TourInCompareDto
    {
        public int Id { get; set; }
        public string TourName { get; set; }
        public int Rating { get; set; }
        public int TourSize { get; set; }
        public DateTime StartDate { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public double ActualPrice { get; set; }
        public string? DistrictFrom { get; set; }
        public string? ProvinceFrom { get; set; }
        public string StartTime { get; set; }
        public string TourStyle { get; set; }
        public string TourTopic { get; set; }

        public string Titles
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    Title = new List<string>();
                else
                {
                    Title = value.Split(';').ToList();

                }
            }
        }
        public List<string>? Title { get; set; } = new List<string>();
        public string? DistrictTos
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    DistrictTo = new List<string>();
                else
                {
                    DistrictTo = value.Split(';').ToList();

                }
            }
        }

        public List<string>? DistrictTo { get; set; } = new List<string>();

        public string? ProvinceTos
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    ProvinceTo = new List<string>();
                else
                {
                    ProvinceTo = value.Split(';').ToList();

                }
            }
        }

        public List<string>? ProvinceTo { get; set; } = new List<string>();

        private string Urls
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    Pictures = new List<string>();
                else
                {
                    Pictures = value.Split(';').ToList();

                }
            }
        }

        public List<string> Pictures { get; set; } = new List<string>();
        public string Slug { get; set; }
    }
    public class DataTourAdminDto
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public IQueryable<Tour> Tours { get; set; }
    }
}
