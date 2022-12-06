using ClothesWebNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothesWebNET.ChainOfResposibility.OrderHandlers
{
    public class InventoryHandler:Handler
    {
        private CLOTHESEntities db = new CLOTHESEntities();
        public override string Handle(Order order)
        {
            var listProduct = order.listProduct;

            for (int i = 0; i < listProduct.Count; i++)
            {
                var product = listProduct[i];
                int stock = product.qty;
                string attributeId = product.attributeId;
                string attributeValueId = product.attributeValueId;
                var stockDB = db.AttributeDetail.FirstOrDefault(s=>s.id== attributeValueId);
                if (stockDB != null)
                {
                    int stockRes = (int)stockDB.stock;
                    if (stock> stockRes)
                    {
                        return $"Lỗi Sản phẩm {product.nameProduct} chỉ còn {stockRes} sản phẩm";
                    }
                   
                }
                else
                {
                    
                        return $"Lỗi Sản phẩm {product.nameProduct} không có thuộc tính {attributeValueId}";
                    
                }
               
            }
            //nếu đã xử lý được hoặc không xử lý được thì cho next
            //chạy qua Handler.cs gọi Handle, tại đây vì đã setNext ở controller nên nó sẽ gọi tiếp handler tương ứng
            return base.Handle(order);
        }
    }
}