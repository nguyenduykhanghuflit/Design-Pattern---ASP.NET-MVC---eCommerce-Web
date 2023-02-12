using ClothesWebNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothesWebNET.Pattern.OrderProcessStrategy
{
    public class ShippingOrder: IOrderProcess
    {
        public bool OrderProcess(string currentStatus)
        {
            return CanUpdateStatusShipping(currentStatus);
        }

        private bool CanUpdateStatusShipping(string currentStatus)
        {
            if (!string.IsNullOrEmpty(currentStatus))
                return currentStatus == ProductOrderStatus.package.ToString();

            return false;
        }
    }
}