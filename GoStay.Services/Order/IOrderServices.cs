
using AutoMapper.Configuration.Conventions;
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.OrderDto;

namespace GoStay.Service
{
    public interface IOrderServices
    {
        public IQueryable<Order> GetListOrder(OrderSearchParam ordersearch);
        public UserInfoDto GetUserInfo(int id);

    }
}
