using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClothesWebNET.Models;

namespace ClothesWebNET.Controllers
{
    public class RegisterController : Controller
    {
        private CLOTHESEntities db = new CLOTHESEntities();
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        //POST:Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string fullName, string username, string password,int phone, string address)
        {

            if (ModelState.IsValid)
            {
               Guid guid= Guid.NewGuid();

                Users user = db.Users.FirstOrDefault(u => u.username == username);
                if (user != null)
                {
                    ViewBag.UsernameErr = "Tên đăng nhập đã tồn tại";
                    return View();
                }
                else
                {
                    if (username.Length < 5 || username.Length > 12)
                    {
                        ViewBag.PasswordErr = "Mật khẩu phải từ 5 đến 12 kí tự";
                        return View();
                    }
                    else
                    {
                     
                        try
                        {
                            Users newUser = new Users();
                            string emptyString = "Chưa cập nhật";
                            newUser.idUser = guid.ToString();
                            newUser.idPermission = "R02";
                            newUser.fullName = fullName;
                            newUser.username = username;
                            newUser.password = password;
                            newUser.gender = true;
                            newUser.identityCard = 123456;
                            newUser.address = address;
                            newUser.email = emptyString;
                            newUser.phone = phone;
                            newUser.district = emptyString;
                            newUser.province = emptyString;
                            newUser.ward = emptyString;
                            newUser.URLAvatar = emptyString;
                            db.Users.Add(newUser);
                            db.SaveChanges();
                            return Redirect("~/login");
                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorMessage = ex.Message;
                            ViewBag.ErrDetail=ex.StackTrace;
                            return View();
                        }
                    }
                }
             
                
            }
            else
            {
 
                return View();
            }

        }
    }
}