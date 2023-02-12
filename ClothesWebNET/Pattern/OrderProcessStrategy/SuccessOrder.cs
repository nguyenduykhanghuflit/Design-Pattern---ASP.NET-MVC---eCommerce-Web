using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClothesWebNET.Models;

namespace ClothesWebNET.Pattern.OrderProcessStrategy
{
    public class SuccessOrder:IOrderProcess
    {
        public bool OrderProcess(string currentStatus)
        {
            return CanUpdateStatusSucces(currentStatus);
        }
        private bool CanUpdateStatusSucces(string currentStatus)
        {
            if (!string.IsNullOrEmpty(currentStatus))
                return currentStatus == ProductOrderStatus.shipping.ToString();

            return false;
        }
    }
}