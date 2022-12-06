using ClothesWebNET.ChainOfResposibility.OrderHandlers;
using ClothesWebNET.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothesWebNET.Controllers
{
    public class CheckoutController : Controller
    {
        private CLOTHESEntities db = new CLOTHESEntities();
        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult HandleOrder(Order order)
        {

            //List Product
            //check xem list product này có trong database hay không -> next
            //check xem có product nào hết số lượng hay không --> next
            //create bill -> next
            //create detail bill -> end


            //create bill


            //create detailBill
            if (ModelState.IsValid)
            {
                var idUserReal = "";
                if (Request.Cookies["user"] != null)
                {
                    idUserReal = Request.Cookies["user"].Value;
                }
                order.idUserReal = idUserReal;
                var handler = new ProducValidHandler();

                handler.SetNext(new InventoryHandler())
                    .SetNext(new CreateBillHandler());
                  

            string mess=  handler.Handle(order);
                if(mess== "oke")
                return Json(new { success = true, mess }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, mess }, JsonRequestBehavior.AllowGet);
                


            }
            else
            {
                var mess = new Hashtable();
                foreach (var pair in ModelState)
                {
                    if (pair.Value.Errors.Count > 0)
                    {
                        mess[pair.Key] = pair.Value.Errors.Select(error => error.ErrorMessage).ToList();
                    }
                }
                return Json(new { success = false, mess }, JsonRequestBehavior.AllowGet);
            }
           

        }

   
    }
}