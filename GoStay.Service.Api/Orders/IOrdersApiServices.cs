using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.OrderDto;

namespace GoStay.Service.Api.Hotels
{
    public interface IOrdersApiServices
    {
        public string CreateOrder(OrderDto order);
        //public string AddOrderDetail(AddOrderDetailParam dto);
        //public string UpdateOrderDetail(InsertOrderDetailDto orderDetail);
        public string RemoveOrderDetail(int IdOrderDetail);
        public string UpdateStatusOrder(UpdateStatusOrderParam dto);
        public string UpdatePTTTOrder(UpdatePTTTOrderParam dto);
        public string UpdateUserIDbySession(UpdateUserIdBySessionParam param);
        public string UpdateMoreInfoOrder(UpdateMoreInfoOrderParam info);
        public List<OrderDetail> GetOrderDetailbyOrder(int order);
        public List<Order> GetOrderbyUserID(int IDUser);
        public Order GetOrderbySession(string session);
        public OrderGetInfoDto CheckOrder(OrderDto order);
        public OrderGetInfoDto GetOrderbyId(int Id);
        public ResponseBase<int> OrderExisting(int UserId, int IdHotel, int IdRoom);

        public List<ChartOdertByDayDto> GetOrderByMonth(int month, int year, int status);
        public List<ChartOdertByDayDto> GetOrderTotalMoneyByMonth(int month, int year, int status);
        public List<ChartOdertByDayDto> GetOrderRoomByMonth(int month, int year, int status);
        public List<OrderSearchOutDto> GetListOrderSearch(OrderSearchParam param);
        public string UpdatePrePayment(UpdatePrePaymentOrderParam dto);
        public string UpdateTotalAmount(UpdateTotalAmountOrderParam dto);
        public ResponseBase<RoomOrderDto> GetRoomInOrder(int IdRoom);
        public ResponseBase<List<string>> GetBookedDateRoom(int IdRoom);

    }
}