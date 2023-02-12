using ClothesWebNET.Models;
using ClothesWebNET.Pattern.ModelsView;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothesWebNET.Controllers
{
    public class HomeController : Controller
    {
        private CLOTHESEntities db = new CLOTHESEntities();

        // GET: Home
        public ActionResult Index()
        {

            ProductVM productVM = new ProductVM();

            ViewBag.newProduct = productVM.GetProductNew();
            ViewBag.productHotList = productVM.GetProductHot();
       

            return View();

        }
        public ActionResult Address()
        {
            return View();

        }

        [HttpGet]

        public JsonResult GetUserInfo()
        {
            if (Session["USER_SESSION"] != null)
            {
                if (Request.Cookies["username"] != null)
                {
                    string username = Request.Cookies["username"].Value;

                    return this.Json(new
                    {
                        Result = (from obj in db.Users where obj.username == username select new { fullName = obj.fullName, email = obj.email, phone = obj.phone, password = "daykhongphaipasswordhihihihihi", gender = obj.gender, indentity = obj.identityCard, address = obj.address, avt = obj.URLAvatar })
                    }, JsonRequestBehavior.AllowGet
                 );


                }
                else return Json("Xóa hết cookie và thử lại", JsonRequestBehavior.AllowGet);
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyInfo()
        {

            if (Session["USER_SESSION"] != null)
            {
                if (Request.Cookies["username"] != null&& Request.Cookies["user"] != null)
                {
                    string username = Request.Cookies["username"].Value;
                    string user = Request.Cookies["user"].Value;
                    var data = (from u in db.Users where u.username == username select u);
                    return View(data.ToList());
                }
                else
                {
                    DeleteCookies();
                    return Redirect("/login");
                }
            }
            else return Redirect("/login");

        }


        [HttpPost]
        public ActionResult EditInfo(string fullName,string email,int phone,string address,int identityCard,string password)
        {

            if (Session["USER_SESSION"] != null)
            {
                if (Request.Cookies["username"] != null && Request.Cookies["user"] != null)
                {
                    string username = Request.Cookies["username"].Value;
                    string user = Request.Cookies["user"].Value;
                    db.Users.Find(user).username = username + "1";
                
                    
                    HttpCookie us = new HttpCookie("username");
                    us.Value = username;
                    us.Expires = DateTime.Now.AddDays(1);

             
                    Response.Cookies.Add(us);


                    db.Users.Find(user).fullName = fullName;


                    db.Users.Find(user).email = email;


                    db.Users.Find(user).phone = phone;
                    db.Users.Find(user).address = address;
                    db.Users.Find(user).password = password;


                    db.SaveChanges();
                    db.Users.Find(user).username = username;
                    db.SaveChanges();

                    return Redirect("/home/myinfo");
                }
                else
                {
                    DeleteCookies();
                    return Redirect("/login");
                }
            }
            else return Redirect("/login");

        }
        public void DeleteCookies()
        {
            Session["USER_SESSION"] = null;
            Session["SESSION_GROUP"] = null;

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                HttpCookie asp = Request.Cookies["ASP.NET_SessionId"];
                asp.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(asp);
                Session.Clear();

                HttpCookie us = Request.Cookies["username"];

                HttpCookie user = Request.Cookies["user"];


                if (us != null)
                {
                    us.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(us);
                }
                if(user != null)
                {
                    user.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(user);

                }


            }
            if (Request.Cookies["username"] != null && Request.Cookies["password"] != null)
            {
                Session.Clear();


                HttpCookie us = Request.Cookies["username"];

                HttpCookie user = Request.Cookies["user"];


                if (us != null)
                {
                    us.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(us);
                }
                if (user != null)
                {
                    user.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(user);

                }
                HttpCookie pass = Request.Cookies["password"];
                pass.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(pass);

            }
        }


        public ActionResult MyBill(string status)
        {

            if (Session["USER_SESSION"] != null)
            {
                if (Request.Cookies["username"] != null && Request.Cookies["user"] != null)
                {
                   
                    string user = Request.Cookies["user"].Value;
                    var data = (from bill in db.Bills orderby bill.createdAt descending
                                where bill.idUser == user
                                select bill);

                    if(!string.IsNullOrEmpty(status))
                    {
                        data = (from bill in db.Bills
                                orderby bill.createdAt descending
                                where bill.idUser == user && bill.orderStatusID == status
                                select bill);

                        if(data!=null)
                            return View(data.ToList());

                    }   
                    /*  data.Include("DetailBill").Include("Product").Include("ImageProduct");*/




                    return View(data.ToList());
                }
                else
                {
                    DeleteCookies();
                    return Redirect("/login");
                }
            }
            else return Redirect("/login");

        }
        public ActionResult Statistical()
        {

            if (Session["USER_SESSION"] != null)
            {
                if (Request.Cookies["username"] != null && Request.Cookies["user"] != null)
                {

                    string user = Request.Cookies["user"].Value;
                

                    var data=db.spThongKeDonHangVaSoTienUser(user);


                    return View(data.ToList());
                }
                else
                {
                    DeleteCookies();
                    return Redirect("/login");
                }
            }
            else return Redirect("/login");

        }

        public RedirectToRouteResult CancelOrder(string idBill)
        {

            if (Session["USER_SESSION"] != null)
            {
                if (Request.Cookies["username"] != null && Request.Cookies["user"] != null)
                {
                    string user = Request.Cookies["user"].Value;
                    var data=db.Bills.Find(idBill);

                    if (data != null && user.Equals(data.idUser) && data.orderStatusID == ProductOrderStatus.watting.ToString())
                    {
                        data.orderStatusID = ProductOrderStatus.cancel.ToString();
                        foreach(var item in data.DetailBIll)
                        {
                            var idAtt = item.attributeValueId;
                            var qty = item.qty;
                            var dataStock = db.AttributeDetail.Find(idAtt);
                            dataStock.stock-=qty;
                        }
                        _ = db.SaveChanges();



                        return RedirectToAction("MyBill", "Home");
                    }
                    else
                    {
                        DeleteCookies();
                        return RedirectToAction("Index", "Login");
                    }


                }
                else
                {
                    DeleteCookies();
                    return RedirectToAction("Index", "Login");
                }
            }
            return RedirectToAction("Index", "Login");

        }

    }
}