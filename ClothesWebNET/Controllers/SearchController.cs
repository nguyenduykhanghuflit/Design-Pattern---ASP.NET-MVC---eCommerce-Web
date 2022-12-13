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
    public class SearchController : Controller
    {
        private CLOTHESEntities db = new CLOTHESEntities();

        // GET:  http://localhost:46418/search/indexq?=tin
        public ActionResult Index(string q)
        {
            // queryparamater = "glasses";
            ProductDTODetail productDTO = new ProductDTODetail();


            q = q.ToLower();

            var productList=db.spGetProductByKeyword(q);


            List<ProductDTO> result = (from product in productList
                                       let listImage = db.ImageProduct.Where(img => img.idProduct == product.idProduct).ToList()
                                       select new ProductDTO(product.price, product.nameProduct, product.idProduct, listImage)
                                     ).ToList();

            /*     var productList = (from s in db.Products
                                    where s.nameProduct.ToLower().Contains(q)
                                    select s);

                 var query = productList.Include(p => p.ImageProduct);
                */
            ViewBag.result = result.ToList();
            return View();


        }

    }
}
