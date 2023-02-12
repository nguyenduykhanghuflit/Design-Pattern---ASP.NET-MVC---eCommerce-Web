using ClothesWebNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothesWebNET.ChainOfResposibility.OrderHandlers
{
    public class ProducValidHandler:Handler
    {
        private CLOTHESEntities db = new CLOTHESEntities();
        public override string Handle(Order order)
        {
            var listProduct = order.listProduct;

            for(int i = 0; i < listProduct.Count; i++)
            {
                var product=listProduct[i];
                var findProduct = db.Products.FirstOrDefault(p=>p.idProduct==product.idProduct);
                if (findProduct == null)
                {
                    return $"Sản phẩm {product.nameProduct} không tồn tại";
                }
            }    
            //nếu đã xử lý được hoặc không xử lý được thì cho next
            //chạy qua Handler.cs gọi Handle, tại đây vì đã setNext ở controller nên nó sẽ gọi tiếp handler tương ứng
           return base.Handle(order);
        }
    }
}