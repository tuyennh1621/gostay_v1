using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Authen;
using GoStay.DataDto.Banner;
using GoStay.DataDto.Hành_Chính;
using GoStay.DataDto.TourDto;
using GoStay.DataDto.Users;

namespace GoStay.Service.Api.Users
{
    public interface IUserApiServices
    {
        User UserLogin(string email, string? password, Common.Enums.UserType enumType);
        User CheckUserByPhone(string phoneNumber);
        User RegisterUserPhone(User user);
        User RegisterUserEmail(User user);
        User CheckUserByEmail(string email);
        User CheckUserByID(int ID);
        User UpdateInfo(User user);
        User? CheckUserByAccount(string email, string password);
        List<User> GetAllUser();
        User SetAuthor(SetAuthorParam param);
        public ResponseBase<bool> CheckUserPermision(CheckPermisionParam param);
        public ResponseBase<List<int>> GetUserPermission(int Userid);
        public AuthenticateResponse CheckUserByAccountAndGetToken(string email, string password);


    }
}