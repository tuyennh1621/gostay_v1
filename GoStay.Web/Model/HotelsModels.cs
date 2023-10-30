using GoStay.DataDto.TourDto;
using GoStay.DataDto.Apis;
using GoStay.DataDto.OrderDto;
using GoStay.DataDto.RatingDto;

namespace GoStay.Web.Model
{

    public class HotelsModels
    {
        public List<HotelHomePageDto>? hotels { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int RemainCount { get; set; }
        public HotelSearchParaRequest? Para { get; set; }
    }

    public class OrdersModels
    {
        public OrderDto? Order { get; set; }
    }
    public class ToursModels
    {
        public List<SearchTourDto>? tours { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int RemainCount { get; set; }
        public SearchTourParam? Para { get; set; }
    }
    public class MainSearchModels
    {
        public string? strPlace { get; set; }
        public int? TT { get; set; }
        public int? QH { get; set; }
        public int? PX { get; set; }
        public int? IDH { get; set; }
        public int? style { get; set; }
        public int? NumRoom { get; set; }
        public int? NumAdult { get; set; }
        public int? NumChildren { get; set; }

    }
    public class TicketSearchModels
    {
        public bool? IsHomePage { get; set; }
        public  Dictionary<string, string> startPoints = new Dictionary<string, string>();
        public  Dictionary<string, string> endPoints = new Dictionary<string, string>();
    }
}
