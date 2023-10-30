using GoStay.Common;
using GoStay.Data.Ticket;

namespace GoStay.Service.Api.Ticket
{
    public interface IOrdersTicketApiServices
    {
        ResponseBase<string> UpdateStatus(UpdateStatus param);
        ResponseBase<OrderTicketShowDto> CreateOrderTicket(CreateOrderTicketParam param);
        ResponseBase<OrderTicketShowDto> CheckOrderTicket(CreateOrderTicketParam order);
        ResponseBase<OrderTicketShowDto> GetOrderTicketbyId(int Id);
        ResponseBase<List<OrderTicketAdminDto>> GetAllOrderTicket(int? UserId,int pageIndex, int pageSize);
    }
}