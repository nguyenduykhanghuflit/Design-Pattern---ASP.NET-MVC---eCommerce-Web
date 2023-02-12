using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClothesWebNET.Models;
namespace ClothesWebNET.Pattern.OrderProcessStrategy
{
    public class PackageOrder : IOrderProcess
    {
        public bool OrderProcess(string currentStatus)
        {
            return CanUpdateStatusPackage(currentStatus);
        }

        private bool CanUpdateStatusPackage(string currentStatus)
        {
            if (!string.IsNullOrEmpty(currentStatus))
                return currentStatus == ProductOrderStatus.accept.ToString();

            return false;
        }
    }
}