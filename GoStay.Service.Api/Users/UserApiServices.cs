using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Authen;
using GoStay.DataDto.Users;
using GoStay.Service.Api.Base;

namespace GoStay.Service.Api.Users
{
    public class UserApiServices : ApiServiceBase, IUserApiServices
    {
        public List<User> GetAllUser()
        {
            var response = Get<List<User>>("user/all-user");
            return response.Data;
        }
        public User SetAuthor(SetAuthorParam param)
        {
            var response = Post<SetAuthorParam,User>("user/set-author",param);
            return response.Data;
        }
        public User UserLogin(string email, string? password, Common.Enums.UserType enumType)
        {
            var response = Get<User>("user/login"
                , new KeyValuePair<string, object>("email", email)
                , new KeyValuePair<string, object>("password", password)
                , new KeyValuePair<string, object>("enumType", enumType));
            return response.Data;
        }

        public User CheckUserByPhone(string phoneNumber)
        {
            var response = Get<User>("user/check-user-by-phone", new KeyValuePair<string, object>("phoneNumber", phoneNumber));
            return response.Data;
        }
        public User RegisterUserPhone(User user)
        {
            var response = Post<User,User>("user/register-user-phone", user);
            return response.Data;
        }
        public User RegisterUserEmail(User user)
        {
            var response = Post<User,User>("user/register-user-email", user);
            return response.Data;
        }

        public User CheckUserByEmail(string email)
        {
            var response = Get<User>("user/check-user-by-email", new KeyValuePair<string, object>("email", email));
            return response.Data;
        }
        public User CheckUserByID(int Id)
        {
            var response = Get<User>("user/check-user-by-id", new KeyValuePair<string, object>("Id", Id));
            return response.Data;
        }
        public User UpdateInfo(User user)
        {
            var response = Post<User,User>("user/update-info", user);
            return response.Data;
        }
        public User? CheckUserByAccount(string email, string password)
        {
            var response = Get<User?>("user/check-user-by-account"
                , new KeyValuePair<string, object>("email", email)
                , new KeyValuePair<string, object>("password", password));
            return response.Data;
        }

        public AuthenticateResponse CheckUserByAccountAndGetToken(string email, string password)
        {
            var response = Get<AuthenticateResponse>("user/check-user-by-account-and-get-token"
                , new KeyValuePair<string, object>("email", email)
                , new KeyValuePair<string, object>("password", password));
            return response.Data;
        }

        public ResponseBase<bool> CheckUserPermision(CheckPermisionParam param)
        {
            var response = Post<CheckPermisionParam,bool>("user/check-user-permision",param);
            return response;
        }
        public ResponseBase<List<int>> GetUserPermission(int Userid)
        {
            var response = Get<List<int>>("user/user-permission"
                , new KeyValuePair<string, object>("Userid", Userid));
            return response;
        }
    }
}