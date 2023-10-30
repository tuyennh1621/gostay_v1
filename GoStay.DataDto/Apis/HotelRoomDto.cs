using GoStay.DataDto.HotelDto;
namespace GoStay.DataDto.Apis
{
    public class HotelRoomDto
    {
        // room infor
        public int Id { get; set; }
        public int Idhotel { get; set; }
        public string? Name { get; set; }
        public decimal? RoomSize { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public int? RoomStatus { get; set; }
        public byte? RemainNum { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        public byte? Palletbed { get; set; }
        public string? PalletbedText { get; set; }
        public string? ViewRoom { get; set; }
        public List<string> Pictures { get; set; } = new List<string>();
        public List<ServiceRoomDto> Services { get; set; }
        //đơn
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public byte? MinNight { get; set; }
        public int? DeadLinePreOrder { get; set; }
        //giá
        public decimal PriceValue { get; set; }
        public double Discount { get; set; }
        public decimal NewPrice { get; set; }
        public decimal CurrentPrice { get; set; }

    }

}
