using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClothesWebNET.Models;

namespace ClothesWebNET.Pattern.AccountFacade
{
    public class LoginFacade
    {
        private readonly ValidationService _validationService;
        private readonly  AuthenticationUser _user;
       
        public LoginFacade()
        {
            _validationService = new ValidationService();
            _user = new AuthenticationUser();
        }
        public Dictionary<string,object> Login(string username, string password)
        {
             Dictionary<string, object> validate=_validationService.Validation(username, password);
            if (!(bool)validate["valid"])
            {
                return validate;
            }
            CLOTHESEntities db = new CLOTHESEntities();
            Dictionary<string, object> authen = _user.Authenticated(username,password,db);
            if (!(bool)validate["valid"])
            {
                return authen;
            }
            return authen;

            //handleLogin
            
        }


    }
}