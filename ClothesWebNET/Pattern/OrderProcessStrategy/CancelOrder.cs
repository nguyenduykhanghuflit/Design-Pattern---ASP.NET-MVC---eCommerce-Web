using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClothesWebNET.Models;

namespace ClothesWebNET.Pattern.OrderProcessStrategy
{
    public class CancelOrder: IOrderProcess
    {
        public bool OrderProcess(string currentStatus)
        {
            return CanUpdateStatusCancel(currentStatus);
        }

        private bool CanUpdateStatusCancel(string currentStatus)
        {
            if (!string.IsNullOrEmpty(currentStatus))
                return currentStatus == ProductOrderStatus.watting.ToString();

            return false;
        }
    }
}