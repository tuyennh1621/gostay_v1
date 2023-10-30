using GoStay.DataDto.Statistical;

using GoStay.Service.Api.Base;

namespace GoStay.Service.Api.Hotels
{
    public class StatisticalApiServices : ApiServiceBase, IStatisticalApiServices
    {

        public ChartDto GetValueChart()
        {
            var response = Get<ChartDto>("statisticals/hotel-chart");

            return response.Data;
        }
        public RoomByDayDto GetRoomInMonthByDay(int month, int year)
        {
            var response = Get<RoomByDayDto>("statisticals/room-by-day",new KeyValuePair<string, object>("month", month),new KeyValuePair<string, object>("year", year) );
            return response.Data;
        }
    }
}