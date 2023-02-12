using ClothesWebNET.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ClothesWebNET.ChainOfResposibility.OrderHandlers
{
    public class CreateBillHandler:Handler
    {
        private CLOTHESEntities db = new CLOTHESEntities();
        public override string Handle(Order order)
        {
          
            Guid idBill = Guid.NewGuid();
            int subtotal = 0;
            foreach (var item in order.listProduct)
            {
                subtotal += item.price * item.qty;
            }

            Bill bill = new Bill();
            bill.idBill = idBill.ToString();
            bill.phone = order.phone;
            bill.address = order.address;
            bill.email = order.email;
            bill.customerName = order.customerName;
            bill.createdAt = DateTime.Now;
            bill.ward = order.ward;
            bill.province = order.province;
            bill.district = order.district;
            bill.paymentMethod = "CODE";
            bill.quantity = order.listProduct.Count();
            bill.customerType = order.idUserReal != "" ? "Thành viên" : "Vãng lai";
            bill.idUser = order.idUserReal != "" ? order.idUserReal : null;
            bill.note = order.note;
            bill.subTotal = subtotal;
            bill.totalMoney = subtotal + 50;
            bill.Shipping = 50;
            bill.orderStatusID = ProductOrderStatus.watting.ToString();

            try
            {
                db.Bills.Add(bill);
                db.SaveChanges();
                foreach (var item in order.listProduct)
                {
                    Guid idDetailBill = Guid.NewGuid();
                    DetailBIll detailBIll = new DetailBIll();
                    detailBIll.idDetailBill = idDetailBill.ToString();
                    detailBIll.idBill = bill.idBill;
                    detailBIll.price = item.price;
                    detailBIll.attributes = item.attributes;
                    detailBIll.qty = item.qty;
                    detailBIll.idProduct = item.idProduct;
                    detailBIll.nameProduct = item.nameProduct;
                    detailBIll.imageProduct = item.imageProduct;
                    detailBIll.attributeValueId = item.attributeValueId;
                    db.Entry(detailBIll).State = System.Data.Entity.EntityState.Modified;
                    db.DetailBIll.Add(detailBIll);

                }
                db.SaveChanges();
                return "oke";
            }
            catch (Exception ex)
            {
                return $"Tạo lỗi {ex.Message}, ${ex.InnerException}";
            }
        }
    }
}