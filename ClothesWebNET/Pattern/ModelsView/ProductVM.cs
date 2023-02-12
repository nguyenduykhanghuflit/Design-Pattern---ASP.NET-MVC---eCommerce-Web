using ClothesWebNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothesWebNET.Pattern.ModelsView
{
    public class ProductVM
    {
        private CLOTHESEntities db = new CLOTHESEntities();
        public List<ProductDTO> GetProductNew()
        {
            var newProductList = db.spGetNewProduct();
            List<ProductDTO> data = (from product in newProductList
                                            let listImage = db.ImageProduct.Where(p => p.idProduct == product.idProduct).ToList()
                                            let productDTO = new ProductDTO(product.price, product.nameProduct, product.idProduct, listImage)
                                            select productDTO).ToList();
            return data;
        }


        public List<ProductDTO> GetProductHot()
        {
            var listIdProductHot = db.spGetHotProduct();
            List<ProductDTO> data = (from product in listIdProductHot
                                            let listImage = db.ImageProduct.Where(img => img.idProduct == product.idProduct).ToList()
                                            select new ProductDTO(product.price, product.nameProduct, product.idProduct, listImage)).ToList();
            return data;
        }


        public List<ProductDTO> GetAllProduct(int? page) {
            if(page == null||page==0) page = 1;
           int size = 12;

            var listIdProductHot = db.spGetProduct(page,size);
            List<ProductDTO> data = (from product in listIdProductHot
                                     let listImage = db.ImageProduct.Where(img => img.idProduct == product.idProduct).ToList()
                                     select new ProductDTO(product.price, product.nameProduct, product.idProduct, listImage)).ToList();
            return data;
          
        }

        public List<ProductDTO> GetProductByType(int?page, string idType)
        {
            if (page == null || page == 0) page = 1;
            int size = 12;

            var listIdProductHot = db.spGetProductByTypeId(idType,page, size);
            List<ProductDTO> data = (from product in listIdProductHot
                                     let listImage = db.ImageProduct.Where(img => img.idProduct == product.idProduct).ToList()
                                     select new ProductDTO(product.price, product.nameProduct, product.idProduct, listImage)).ToList();
            return data;
        }

    }
}