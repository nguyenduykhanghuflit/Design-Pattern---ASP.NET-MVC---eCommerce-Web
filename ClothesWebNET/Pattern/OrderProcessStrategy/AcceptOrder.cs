using ClothesWebNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothesWebNET.Pattern.OrderProcessStrategy
{
    public class AcceptOrder : IOrderProcess
    {
        public bool OrderProcess(string currentStatus)
        {
           return CanUpdateStatusSuccess(currentStatus);
        }

        private bool CanUpdateStatusSuccess(string currentStatus)
        {
            if(!string.IsNullOrEmpty(currentStatus))
               return currentStatus == ProductOrderStatus.watting.ToString();
            
            return false;
        }
    }
}