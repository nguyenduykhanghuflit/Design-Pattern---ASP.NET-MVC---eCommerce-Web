using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClothesWebNET.Models;
namespace ClothesWebNET.Pattern.AccountFacade
{
    public interface IAuthenticationAdmin
    {
       bool IsAdmin(Users users);
    }
    public class AuthenticationAdmin: IAuthenticationAdmin
    {
        public bool IsAdmin(Users users)
        {
            if(users.Permission.namePermission=="admin")
                return true;
            return false;
        }
    }
}