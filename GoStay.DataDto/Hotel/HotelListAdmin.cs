using GoStay.DataAccess.Entities;

namespace GoStay.DataDto.HotelDto
{
    public class HotelListAdmin
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<Hotel> ListHotel { get; set; }

    }
    public class GetHotelAdminParam
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int IdProvince { get; set; }
        public string? NameSearch { get; set; }
    }
}
