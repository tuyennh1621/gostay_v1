
using DAL.Entities;
using GoStay.Common.Configuration;
using GoStay.DataAccess.Entities;
using GoStay.Web.Model;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;



namespace goStayCore.CommonCode
{
    public  class SessionManager :  ControllerBase, ISessionManager
    {
        private readonly IHttpContextAccessor _contx;
        private string secret = AppConfigs.SecretKey;

        private readonly IConfiguration _configuration;
        public SessionManager(IHttpContextAccessor contx,  IConfiguration configuration)
        {
            this._contx = contx;
            this._configuration = configuration;
        }


        public User? GetLoginAdminFromSession()
        {

            User? user = new User();

            var Users = _contx != null && _contx.HttpContext != null && _contx.HttpContext.Session != null ? _contx.HttpContext.Session.GetString(AppConfigs.CurrentUserCK) : null;
            if (!String.IsNullOrEmpty(Users))
            {
                user = JsonConvert.DeserializeObject<User>(Users);

                return user;
            }
            else
            {

                return null;
            }

        }
        public User? GetLoginAdminFromSessionAdmin()
        {

            User? user = new User();

            var Users = _contx != null && _contx.HttpContext != null && _contx.HttpContext.Session != null ? _contx.HttpContext.Session.GetString(AppConfigs.CurrentUserAdmin) : null;
            if (!String.IsNullOrEmpty(Users))
            {
                user = JsonConvert.DeserializeObject<User>(Users);

                return user;
            }
            else
            {

                return null;
            }

        }
        public UserGostay GetUserGostay()
        {
            if (_contx.HttpContext != null && _contx.HttpContext.Session != null)
            {
                var cookieValue = _contx.HttpContext.Request.Cookies[AppConfigs.CurrentUserCK];
                if (!string.IsNullOrEmpty(cookieValue))
                {
                    UserGostay? userg = new UserGostay();
                    userg = JsonConvert.DeserializeObject<UserGostay>(cookieValue);
                    userg.UserId = Int32.Parse(DecryptPassword(userg.UserVerify));
                    if (userg != null)
                    {
                        userg.SesstionID = _contx.HttpContext.Session.Id;
                        userg.isLogined = true;
                        return userg;
                    }
                    
                }

                return new UserGostay
                {
                    UserId = 1,
                    UserName = "Anonymus",
                    isLogined = false,
                    SesstionID = _contx.HttpContext.Session.Id
                };
            }
            return new UserGostay
            {
                UserId = 1,
                UserName = "Anonymus",
                isLogined = false,
                SesstionID = ""
            };
        }

        public User? GetLoginTeacherFromSession()
        {

            User? user = new User();

            var Users = _contx != null && _contx.HttpContext != null && _contx.HttpContext.Session != null ? _contx.HttpContext.Session.GetString("Teacher") : null;
            if (Users != null && !String.IsNullOrEmpty(Users))
            {
                user = JsonConvert.DeserializeObject<User>(Users);

                return user;
            }
            else
            {

                return null;
            }
        }
        public User GetGostayUserFromSession()
        {

            User user = new User() { UserId = 1};
            if (_contx != null && _contx.HttpContext != null)
            {
                var cookieValue = _contx.HttpContext.Request.Cookies[AppConfigs.CurrentUserCK];
                if (!string.IsNullOrEmpty(cookieValue))
                {
                    user = JsonConvert.DeserializeObject<User>(cookieValue);
                    var result = DecryptPassword(user.UserVerify);
                    user.UserId = Int32.Parse(result);
                }
            }
            return user;

        }
        public void SetMenusInSession(string? DashboardLangShortName)
        {
            string? lang = String.IsNullOrEmpty(DashboardLangShortName) ? "en" : DashboardLangShortName;

        }

        public void SetUserDataInSession(User model)
        {
            //set session
            var user = JsonConvert.SerializeObject(model);
        
        }
        public string EncryptPassword(string strId)
        {
            if (string.IsNullOrEmpty(strId))
            {
                return "";
            }
            else
            {
                try
                {
                    strId = secret + "_" + strId;
                    byte[] data = ASCIIEncoding.ASCII.GetBytes(strId);
                    string uId = Convert.ToBase64String(data);
                    return uId;
                }
                catch (Exception)
                {
                    
                    return "";
                }
               
            }
           
        }
        public string DecryptPassword(string strId)
        {
            if (string.IsNullOrEmpty(strId))
            {
                return "";
            }
            else
            {
                try
                {
                    byte[] data = Convert.FromBase64String(strId);
                    string veri = ASCIIEncoding.ASCII.GetString(data);
                    var start = secret.Length +1;
                    var end = veri.Length - start;

                    string uId = veri.Substring(start, end);
                    return uId;
                }
                catch (Exception)
                {
                    return "";
                }
                
            }
           

            //byte[] salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG
            //input = secret + "_" + input;
            //using (var rng = RandomNumberGenerator.Create())
            //{
            //    rng.GetBytes(salt);
            //}
            //string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            //    password: input,
            //    salt: salt,
            //    prf: KeyDerivationPrf.HMACSHA1,
            //iterationCount: 10000,
            //    numBytesRequested: 256 / 8
            //));
            //return encryptedPassw;
            //return new HashSalt { Hash = encryptedPassw, Salt = salt };
        }
    }

    public interface ISessionManager
    {
        public User? GetLoginAdminFromSession();
        public User? GetLoginAdminFromSessionAdmin();
        public User GetGostayUserFromSession();
        public User? GetLoginTeacherFromSession();
        public UserGostay GetUserGostay();
        // public students? GetLoginStudentFromSession();
        public void SetUserDataInSession(User model);
        public void SetMenusInSession(string? DashboardLangShortName);
        public string EncryptPassword(string strId);
        public string DecryptPassword(string strId);
    }
}