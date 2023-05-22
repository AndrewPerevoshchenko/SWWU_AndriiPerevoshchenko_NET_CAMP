using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_9
{
    internal class Confectioner : Cook
    {
        public Confectioner(string surname) : base(surname) { }
        public override (Order, Guid?) Handle(Order request)
        {
            if (request.DishType == DishType.Sweet)
            {
                return (request, ID);
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
