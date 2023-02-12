using ClothesWebNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothesWebNET.Pattern.CatalogSingeton
{
    public sealed class CatalogSingleton
    {
     
        public static CatalogSingleton Instance { get; }=new CatalogSingleton();
        public List<Types> listType { get; set; } =new List<Types>();
        private CatalogSingleton() { }

        public void Init(CLOTHESEntities db)
        {
            if(listType.Count == 0)
            {
                var catalog = db.Types.ToList();
                foreach(var type in catalog)
                {
                    listType.Add(type);
                }
            }
        }
    }
}