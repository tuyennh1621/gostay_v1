
namespace GoStay.DataDto.Statistical
{
    public class RoomByDayDto
    {
        public RoomByDayDto()
        {
            RoomByDayValue = new List<RoomByDayValue>();
        }
        public int TotalRoom { get; set; }
        public List<RoomByDayValue> RoomByDayValue { get; set; }
    }

    public class RoomByDayValue
    {
        public int Day { get; set; }

        public int Amount { get; set; }
    }
}
