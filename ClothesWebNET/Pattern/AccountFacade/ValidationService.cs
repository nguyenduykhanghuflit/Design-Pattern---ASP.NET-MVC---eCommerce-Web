using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothesWebNET.Pattern.AccountFacade
{
    public interface IValidationService
    {
        Dictionary<string,object> Validation(string username, string password);
    }
    public class ValidationService : IValidationService
    {


        public Dictionary<string, object> Validation(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return new Dictionary<string, object>() { 
                      {"valid", false},
                      {"mess", "Username và password không được để trống"}
                };
            }
            else if (username.Length<5||username.Length>12)
            {
                return new Dictionary<string, object>() {
                      {"valid", false},
                      {"mess", "Username phải từ 6  đến 12 kí tự"}
                };
            }
            else
            {
                return new Dictionary<string, object>() {
                      {"valid", true},
                      {"mess", "Hợp lệ"}
                };
            }    
           
        }
    }
}