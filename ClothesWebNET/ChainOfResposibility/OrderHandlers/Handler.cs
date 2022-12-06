using ClothesWebNET.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesWebNET
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);
        string Handle(Order order);
    }

    public abstract class Handler : IHandler
    {
        private IHandler Next { get; set; }
        //những thằng khác sẽ orveride lại Handle và thực hiện xử lý, nếu xử lý không được sẽ gọi lại Handle này
        //vì đã setNext ở controller nên nó sẽ Handle tiếp hoặc không
        public virtual string Handle(Order order)
        {
          
            return Next?.Handle(order);
        }

        //những thằng kế thừa ở controller khi gọi setNext sẽ nhảy vào đây
        public IHandler SetNext(IHandler handler)
        {
            Next = handler;
            return Next;
        }
    }
}
