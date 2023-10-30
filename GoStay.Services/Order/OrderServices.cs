using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.OrderDto;
using Microsoft.EntityFrameworkCore;

namespace GoStay.Service
{
    public class OrderServices : IOrderServices
    {
        private readonly ICommonRepository<Order> _OrderRepository;
        private readonly ICommonRepository<User> _userRepository;

        private readonly ICommonRepository<OrderDetail> _OrderDetailRepository;
        private readonly ICommonUoW _commonUoW;

        public OrderServices(ICommonRepository<Order> OrderRepository,ICommonRepository<OrderDetail> OrderRoomRepository,
            ICommonUoW commonUoW, ICommonRepository<User> userRepository)
        {
            _OrderDetailRepository = OrderRoomRepository;
            _OrderRepository = OrderRepository;
            _commonUoW = commonUoW;
            _userRepository = userRepository;
        }

        public IQueryable<Order> GetListOrder(OrderSearchParam ordersearch)
        {
            if (ordersearch == null)
            {
                var listOrder = _OrderRepository.FindAll(x => x.IsDeleted == false)
                .Include(x => x.IdPtthanhToanNavigation)
                .Include(x => x.StatusNavigation)
                .Include(x => x.IdUserNavigation)
                ;
                return listOrder;

            }
            else
            {

                var listOrder = _OrderRepository.FindAll(x => x.IsDeleted == false)
                .Include(x => x.IdPtthanhToanNavigation)
                .Include(x => x.StatusNavigation)
                .Include(x => x.IdUserNavigation)
                .Where (x => x.IdUserNavigation.UserId == ordersearch.UserId );
                return listOrder;

            }
        }
        public UserInfoDto GetUserInfo(int id)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                UserInfoDto result = new UserInfoDto()
                {
                    Address = user.Address,
                    Email = user.Email,
                    MobileNo = user.MobileNo,
                    Password = user.Password,
                    UserId = user.UserId,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Picture = user.Picture
                };
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
