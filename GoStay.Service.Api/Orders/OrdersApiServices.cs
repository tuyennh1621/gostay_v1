using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.OrderDto;
using GoStay.Service.Api.Base;
using System;

namespace GoStay.Service.Api.Hotels
{
    public class OrdersApiServices : ApiServiceBase, IOrdersApiServices
    {
        public string CreateOrder(OrderDto order)
        {
            var response = Post<OrderDto, string>("orders/order", order);
            return response.Data;
        }

        public OrderGetInfoDto CheckOrder(OrderDto order)
        {
            var response = Post<OrderDto, OrderGetInfoDto>("orders/check-order", order);
            return response.Data;
        }

        public string RemoveOrderDetail(int IdOrderDetail)
        {
            var response = Delete<string>("orders/detail", new KeyValuePair<string, object>("IdOrderDetail", IdOrderDetail));
            return response.Data;
        }
        public string UpdateStatusOrder(UpdateStatusOrderParam dto)
        {
            var response = Put<UpdateStatusOrderParam, string>("orders/status", dto);
            return response.Data;
        }
        public string UpdatePrePayment(UpdatePrePaymentOrderParam dto)
        {
            var response = Put<UpdatePrePaymentOrderParam, string>("orders/prepayment", dto);
            return response.Data;
        }
        public string UpdateTotalAmount(UpdateTotalAmountOrderParam dto)
        {
            var response = Put<UpdateTotalAmountOrderParam, string>("orders/totalamount", dto);
            return response.Data;
        }
        public string UpdatePTTTOrder(UpdatePTTTOrderParam dto)
        {
            var response = Put<UpdatePTTTOrderParam, string>("orders/pttt", dto);
            return response.Data;
        }
        public string UpdateMoreInfoOrder(UpdateMoreInfoOrderParam info)
        {
            var response = Put<UpdateMoreInfoOrderParam, string>("orders/order", info);
            return response.Data;
        }
        public string UpdateUserIDbySession(UpdateUserIdBySessionParam param)
        {
            var response = Put<UpdateUserIdBySessionParam, string>("orders/userid-by-session", param);
            return response.Data;
        }
        
        public List<OrderDetail> GetOrderDetailbyOrder(int order)
        {
            var response = Get<List<OrderDetail>>("orders/detail-by-order", new KeyValuePair<string, object>("order", order));
            return response.Data;
        }
        public List<Order> GetOrderbyUserID(int IDUser)
        {
            var response = Get<List<Order>>("orders/order-by-userid", new KeyValuePair<string, object>("IDUser", IDUser));
            return response.Data;
        }
        public Order GetOrderbySession(string session)
        {
            var response = Get<Order>("orders/order-by-session", new KeyValuePair<string, object>("session", session));
            return response.Data;
        }

        public OrderGetInfoDto GetOrderbyId(int Id)
        {
            var response = Get<OrderGetInfoDto>("orders/order-by-id", new KeyValuePair<string, object>("Id", Id));
            return response.Data;
        }
        public ResponseBase<int> OrderExisting(int UserId, int IdHotel, int IdRoom)
        {
            var response = Get<int>("orders/check-existing", new KeyValuePair<string, object>("UserId", UserId),
                                    new KeyValuePair<string, object>("IdHotel", IdHotel),
                                    new KeyValuePair<string, object>("IdRoom", IdRoom));
            return response;
        }

        public List<ChartOdertByDayDto> GetOrderByMonth(int month, int year, int status)
        {
            var response = Get<List<ChartOdertByDayDto>>("orders/order-in-month",
                new KeyValuePair<string, object>("month", month),
                new KeyValuePair<string, object>("year", year),
                new KeyValuePair<string, object>("status", status));

            return response.Data;
        }
        public List<ChartOdertByDayDto> GetOrderTotalMoneyByMonth(int month, int year, int status)
        {
            var response = Get<List<ChartOdertByDayDto>>("orders/money-in-month",
                new KeyValuePair<string, object>("month", month),
                new KeyValuePair<string, object>("year", year),
                new KeyValuePair<string, object>("status", status));
            return response.Data;
        }
        public List<ChartOdertByDayDto> GetOrderRoomByMonth(int month, int year, int status)
        {
            var response = Get<List<ChartOdertByDayDto>>("orders/room-in-month", 
                new KeyValuePair<string, object>("month", month),
                new KeyValuePair<string, object>("year", year),
                new KeyValuePair<string, object>("status", status));
            return response.Data;
        }
        public List<OrderSearchOutDto> GetListOrderSearch(OrderSearchParam param)
        {
            var response = Post<OrderSearchParam, List<OrderSearchOutDto>>("orders/list-order-search", param);
            return response.Data;
        }
        public ResponseBase<RoomOrderDto> GetRoomInOrder(int IdRoom)
        {
            var response = Get<RoomOrderDto> ("orders/list-room-in-order", new KeyValuePair<string, object>("IdRoom", IdRoom));
            return response;
        }
        public ResponseBase<List<string>> GetBookedDateRoom(int IdRoom)
        {
            var response = Get<List<string>>("orders/booked-date",
                new KeyValuePair<string, object>("IdRoom", IdRoom));
            return response;
        }

    }
}