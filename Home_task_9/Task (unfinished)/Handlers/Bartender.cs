using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_9
{
    internal class Bartender : Cook
    {
        public Bartender(string surname) : base (surname) { }
        public override (Order, Guid?) Handle(Order request)
        {
            if (request.DishType == DishType.Drink)
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
