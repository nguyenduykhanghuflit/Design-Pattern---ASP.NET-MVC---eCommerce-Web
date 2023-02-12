using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesWebNET.Pattern.OrderProcessStrategy
{
    internal interface IOrderProcess
    {
        bool OrderProcess(string currentStatus);
    }
}
