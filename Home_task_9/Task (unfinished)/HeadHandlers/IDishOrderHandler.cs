using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_9
{
    internal interface IDishOrderHandler
    {
        IDishOrderHandler SetNext(IDishOrderHandler handler);
        (Order, Guid?) Handle(Order request);
    }
}
