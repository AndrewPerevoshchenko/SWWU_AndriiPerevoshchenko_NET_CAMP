using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_9
{
    internal class OrdersGroup
    {
        private Queue<Order> _orders;
        public Queue<Order> Orders
        {
            get { return new Queue<Order>(_orders); }
        }
        public OrdersGroup()
        {
            _orders = new Queue<Order>();
        }
        public void AddDishOrder(Order dishOrder)
        {
            if (dishOrder != null)
            {
                if (!_orders.Contains(dishOrder))
                {
                    _orders.Enqueue(dishOrder);
                }
                throw new InvalidDataException("This order had been already added");
            }
        }
    }
}
