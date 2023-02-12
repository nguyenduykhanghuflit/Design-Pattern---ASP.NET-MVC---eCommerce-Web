using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClothesWebNET.Models;

namespace ClothesWebNET.Pattern.OrderProcessStrategy
{
    public class FailedOrder : IOrderProcess
    {
        public bool OrderProcess(string currentStatus)
        {
            return CanUpdateStatusFailed(currentStatus);
        }
        private bool CanUpdateStatusFailed(string currentStatus)
        {
            if (!string.IsNullOrEmpty(currentStatus))
                return currentStatus == ProductOrderStatus.shipping.ToString();

            return false;
        }
    }
}