using GoStay.DataDto.Statistical;


namespace GoStay.Service.Api.Hotels
{
    public interface IStatisticalApiServices
    {
        ChartDto GetValueChart();
        RoomByDayDto GetRoomInMonthByDay(int month, int year);
    }
}