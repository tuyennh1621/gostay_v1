using GoStay.DataDto.Statistical;


namespace GoStay.DataDto.OrderDto
{
    public class OrderChartDto
    {
        public OrderChartDto()
        {
            GetOrderByMonth = new List<ChartOdertByDayDto>();
            GetOrderTotalMoneyByMonth = new List<ChartOdertByDayDto>();
            GetOrderRoomByMonth = new List<ChartOdertByDayDto>();
        }
        public List<ChartOdertByDayDto> GetOrderByMonth { get; set; }
        public List<ChartOdertByDayDto> GetOrderTotalMoneyByMonth { get; set; }
        public List<ChartOdertByDayDto> GetOrderRoomByMonth { get; set; }
        

    }

    public class ChartOdertByDayDto
    {
        public int Day { get; set; }
        public int Value { get; set; }
    }
    public class ChartOdertValue
    {
        public ChartOdertValue()
        {
            ChartOdertDetailValues = new List<ChartOdertDetailValue>();
        }
        public DateTime Date { get; set; }
        public int ID { get; set; }
        public List<ChartOdertDetailValue> ChartOdertDetailValues { get; set; }
    }

    public class ChartOdertDetailValue
    {
        public decimal? Price { get; set; }
        public Double? Discount { get; set; }
        public Double ActualPrice
        {
            get
            {
                if (Price == null)
                    return 0;
                else if (Discount == null)
                    return (Double)Price;
                return (double)((double)Price  - ((double)Price * (double)Discount) / 100);
            }
        }
    }
}
