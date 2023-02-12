using ClothesWebNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothesWebNET.Pattern.AccountFacade
{
    public interface IAuthenticationUser
    {
        Dictionary<string, object> Authenticated(string username, string password, CLOTHESEntities db );
    }
    public class AuthenticationUser: IAuthenticationUser
    {
        public Dictionary<string, object> Authenticated(string username, string password, CLOTHESEntities db)
        {

            var findUsername=db.Users.FirstOrDefault(u=>u.username==username );
            if (findUsername== null)
            {
              
                return new Dictionary<string, object>() {
                      {"valid", false},
                      {"mess", "Username không tồn tại"}
                };

            }
            else
            {
                bool checkPassword = findUsername.password == password;
                if(!checkPassword) return new Dictionary<string, object>() {
                      {"valid", false},
                      {"mess", "Sai mật khẩu"}
                };
                return new Dictionary<string, object>() {
                      {"valid", true},
                      {"mess", "Có thể đăng nhập"}
                };

            }
        }
    }
}