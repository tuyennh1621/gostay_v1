namespace GoStay.DataDto.HotelDto
{
    public partial class RoomAddDto
    {

        public int Id { get; set; }
        public int? Idhotel { get; set; }
        public string? Name { get; set; }
        public decimal? RoomSize { get; set; }
        public string? Description { get; set; }
        public int? RoomStatus { get; set; }

        public int? Status { get; set; }
        public decimal? PriceValue { get; set; }
        public double? Discount { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        public byte? Palletbed { get; set; }
        public int[]? ViewRoom { get; set; }
        public int[]? ServiceRoom { get; set; }
        public int? IdUser { get; set; }

    }
    public partial class RoomEditDto
    {
        public int Id { get; set; }
        public int? Idhotel { get; set; }
        public string? Name { get; set; }
        public decimal? RoomSize { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public int? RoomStatus { get; set; }

        public decimal? PriceValue { get; set; }
        public double? Discount { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        public byte? Palletbed { get; set; }
        public string? ViewRoom { get; set; }
        public string? ServiceRoom { get; set; }
        public int? IdUser { get; set; }

    }

    public class PictureRoomDto
    {
        public int Id { get; set; }
        public string? UrlOut { get; set; }
    }
    public class ServiceRoomDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameEng { get; set; }
        public string? NameChi { get; set; }
        public string? NameLang
        {
            get
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "en-US":
                        return NameEng;
                    case "zh-CN":
                        return NameChi;
                }
                return Name;
            }
        }
        public byte? AdvantageLevel { get; set; }
        public string? Icon { get; set; }
    }
    public class RequestGetListRoomAdmin
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int? IdHotel { get; set; }
        public int? IdUser { get; set; }
        public int? RoomStatus { get; set; }
        public string? RoomName { get; set; }
        public string? HotelName { get; set; }

    }
    public class RoomAdminDto
    {
        public int Id { get; set; }
        public decimal NewPrice { get; set; }
        public decimal Price_Value { get; set; }
        public float Discount { get; set; }
        public decimal RoomSize { get; set; }
        public string Name { get; set; }
        public byte NumMature { get; set; }
        public byte NumChild { get; set; }
        public string SearchKey { get; set; }
        public int Status { get; set; }
        public int RoomStatus { get; set; }

        public int IDUser { get; set; }
        public string HotelName { get; set; }
        public string UserName { get; set; }
        public string Urls { get; set; }
        public int Total { get; set; }

        public bool Selected(int value)
        {
            if (value == Status)
                return true;
            else
                return false;
        }

    }
}
