using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClothesWebNET.Models;
using ClothesWebNET.Pattern.OrderProcessStrategy;

namespace ClothesWebNET.Areas.Admin.Controllers
{
    public class BillController : Controller
    {
        private CLOTHESEntities db = new CLOTHESEntities();


        [HttpGet]
        public JsonResult GetAllBill()
        {
            if (Session["SESSION_GROUP_ADMIN"] != null)
            {
                ProductDTODetail productDTO = new ProductDTODetail();

                var bills = db.Bills.Include(b => b.Users);
                var listProduct = from p in bills
                                  select new BillData()
                                  {
                                      idBill = p.idBill,
                                      idUser = p.idUser,
                                      Ship = p.Shipping,
                                      Total = p.subTotal,
                                      PTTT = p.paymentMethod,
                                      status = true,
                                      createdAt = p.createdAt,
                                      Qty = p.totalMoney,
                                      nameUser = p.customerName,
                                      email = p.email,
                                      phone = p.phone,

                                  };
                return Json(listProduct.ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json("không đủ quyền", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetDetailBill(string idBill)
        {

            if (Session["SESSION_GROUP_ADMIN"] != null)
            {
                if (idBill == null)
                {
                    return Json("ERR");
                }

                Bill bill = db.Bills.Find(idBill);

                if (bill == null)
                {
                    return Json("Không có hóa đơn này");
                }
                else
                {

                    var billList = (from detail in db.DetailBIll
                                    where detail.idBill == idBill
                                    join bills in db.Bills on detail.idBill equals bills.idBill
                                    join product in db.Products on detail.idProduct equals product.idProduct
                                    select new ItemDetail()
                                    {
                                        nameBook = bill.customerName,
                                        phone = bill.phone,
                                        address = bill.address,
                                        idBill = idBill,
                                        Total = bills.totalMoney + bills.Shipping,
                                        shipping=bill.Shipping,
                                        subTotal=bill.totalMoney,
                                        idDetailBill = detail.idDetailBill,
                                        nameProduct = product.nameProduct,
                                        idProduct = detail.idProduct,
                                        qty = detail.qty,
                                        price = product.price,
                                        statusId=bill.OrderStatus.orderStatusID,
                                        statusName=bill.OrderStatus.statusName,
                                        intoMoney = (int)product.price * (int) detail.qty,


                                    });;


                    return Json(billList);

                }
            }
            return Json("không đủ quyền");

        }

        [HttpPost]
        public JsonResult UpdateBill(string ibBill, string nameBook, string phone, string address, bool status)
        {

            if (Session["SESSION_GROUP_ADMIN"] != null)
            {
                if (ibBill == null)
                {
                    return Json("ERR");
                }

                Bill bill = db.Bills.Find(ibBill);

                if (bill != null)
                {
                    bill.customerName = nameBook;
                    bill.phone = phone;
                    bill.address = address;
                    bill.orderStatusID = "123";
                    db.SaveChanges();
                    return Json("success");
                }

            }
            return Json("không đủ quyền");



        }


        [HttpPost]
        public JsonResult OrderStatus(string idBill, string status)
        {
            if (Session["SESSION_GROUP_ADMIN"] != null)
            {
                if (idBill == null)
                {
                    return Json(new { status = false, mess = "Mã đơn hàng không hợp lệ" }, JsonRequestBehavior.AllowGet);
                }

                var dataBill=db.Bills.Find(idBill);
                if (dataBill != null)
                {
                    string currentStatus = dataBill.OrderStatus.orderStatusID;
                    if( currentStatus == status)
                        return Json(new { status = true, mess = "Thành công",msg="Không thay đổi" }, JsonRequestBehavior.AllowGet);
                    else
                    {

                        IOrderProcess acceptOrder = new AcceptOrder();
                        IOrderProcess cancelOrder = new CancelOrder();
                        IOrderProcess failedOrder = new FailedOrder();
                        IOrderProcess packageOrder = new PackageOrder();
                        IOrderProcess shippingOrder = new ShippingOrder();
                        IOrderProcess successOrder = new SuccessOrder();
                        try
                        {
                            if (
                                status == ProductOrderStatus.accept.ToString()&& acceptOrder.OrderProcess(currentStatus)
                                || status == ProductOrderStatus.cancel.ToString() && cancelOrder.OrderProcess(currentStatus)
                                || status == ProductOrderStatus.fail.ToString() && failedOrder.OrderProcess(currentStatus)
                                || status == ProductOrderStatus.package.ToString() && packageOrder.OrderProcess(currentStatus)
                                || status == ProductOrderStatus.shipping.ToString() && shippingOrder.OrderProcess(currentStatus)
                                || status == ProductOrderStatus.success.ToString() && successOrder.OrderProcess(currentStatus)
                                )
                            {
                                UpdateBillStatus(dataBill, status);
                                return Json(new { status = true, mess = "Thành công" }, JsonRequestBehavior.AllowGet);
                            }

                            else
                            {
                                return Json(new { status = false, mess = "Trạng thái không hợp lệ", msg = "invalid" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (Exception ex)
                        {
                            return Json(new { status = false, mess = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                        }

                       

                    }
                

                }
                return Json(new { status = false, mess = "Mã đơn hàng không tồn tại" }, JsonRequestBehavior.AllowGet);



            }
            return Json(new {status=false, mess="Không có quyền"}, JsonRequestBehavior.AllowGet );
        }

        private static Dictionary<string, object> GetData()
        {
            return new Dictionary<string, object>();
        }
     
        void UpdateBillStatus(Bill bill, string status)
        {
            bill.orderStatusID = status;
            _ = db.SaveChanges();
        }

        public ActionResult Index()
        {
            if (Session["SESSION_GROUP_ADMIN"] != null)
            {
                var bills = db.Bills.OrderByDescending(x => x.createdAt).Include(b => b.Users);
                return View(bills.ToList());
            }
            return Redirect("~/login");
        }

        // GET: Admin/Bills/Details/5
        public ActionResult Details(string id)
        {
            if (Session["SESSION_GROUP_ADMIN"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Bill bill = db.Bills.Find(id);
                if (bill == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    var billList = (from s in db.Bills
                                    where s.idBill == id
                                    select s);

                    var query = billList.Include(p => p.DetailBIll).Include(s => s.OrderStatus);
                    ViewBag.list = query.ToList();
                    ViewBag.currentStatus = query.First().OrderStatus;
                    var orderStatus = new List<OrderStatus>();

                    foreach(var item in db.OrderStatus)
                    {
                        if (item.orderStatusID != query.First().OrderStatus.orderStatusID)
                            orderStatus.Add(item);
                    }

                 
                    ViewBag.orderStatus = orderStatus.ToList();
                    return View(query.ToList());
                }

            }
            return Redirect("~/login");
        }

        // GET: Admin/Bills/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["SESSION_GROUP_ADMIN"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Bill bill = db.Bills.Find(id);
                if (bill == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idUser = new SelectList(db.Users, "idUser", "idPermission", bill.idUser);
                return View(bill);
            }
            return Redirect("~/login");
        }
        // POST: Admin/Bills/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idBill,idUser,Shipping,Total,PTTT,status,createdAt,totalQty,nameBook,email,phone,address")] Bill bill)
        {
            if (Session["SESSION_GROUP_ADMIN"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(bill).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.idUser = new SelectList(db.Users, "idUser", "idPermission", bill.idUser);
                return View(bill);
            }
            return Redirect("~/Home");
        }

      
    }
}
