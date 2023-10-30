using GoStay.Common;
using GoStay.Data.Ticket;
using GoStay.DataAccess.Entities;
using GoStay.Service.Api.Base;

namespace GoStay.Service.Api.Ticket
{
    public class OrdersTicketApiServices : ApiServiceBase, IOrdersTicketApiServices
    {
        public ResponseBase<OrderTicketShowDto> CreateOrderTicket(CreateOrderTicketParam param)
        {
            var response = Post<CreateOrderTicketParam, OrderTicketShowDto>("ordersTicket/order-ticket", param);
            return response;
        }
        public ResponseBase<string> UpdateStatus(UpdateStatus param)
        {
            var response = Put<UpdateStatus, string>("ordersTicket/update-status", param);
            return response;
        }

        public ResponseBase<OrderTicketShowDto> CheckOrderTicket(CreateOrderTicketParam order)
        {
            var response = Post<CreateOrderTicketParam, OrderTicketShowDto>("OrdersTicket/check-order-ticket", order);
            return response;
        }
        public ResponseBase<OrderTicketShowDto> GetOrderTicketbyId(int Id)
        {
            var response = Get<OrderTicketShowDto>("ordersTicket/order-ticket-by-id", new KeyValuePair<string, object>("Id", Id));
            return response;
        }
        public ResponseBase<List<OrderTicketAdminDto>> GetAllOrderTicket(int? UserId,int pageIndex,int pageSize)
        {
            var response = Get<List<OrderTicketAdminDto>>("ordersTicket/all-order-ticket",
                new KeyValuePair<string, object>("pageIndex", pageIndex),
                new KeyValuePair<string, object>("pageSize", pageSize),
                new KeyValuePair<string, object>("UserId", UserId)
                );
            return response;
        }
    }
}