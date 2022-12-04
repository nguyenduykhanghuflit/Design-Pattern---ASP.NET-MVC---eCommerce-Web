using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothesWebNET.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        // GET: Admin/Post
        public ActionResult Index()
        {

            if (Session["SESSION_GROUP_ADMIN"] != null)
            {
               
                return View();
            }
            return Redirect("~/login");
          
        }

        public ActionResult Edit()
        {

            if (Session["SESSION_GROUP_ADMIN"] != null)
            {
                
                return View();
            }
            return Redirect("~/login");
        }
        public ActionResult Add()
        {

            if (Session["SESSION_GROUP_ADMIN"] != null)
            {
               
                return View();
            }
            return Redirect("~/login");
        }
        public ActionResult Delete()
        {
            if (Session["SESSION_GROUP_ADMIN"] != null)
            {

                return View();
            }
            return Redirect("~/login");
        }

       
       
       

    }
}