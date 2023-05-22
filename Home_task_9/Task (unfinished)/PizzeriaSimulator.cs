using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_9
{
    internal class PizzeriaSimulator
    {
        private OrdersGroup _ordersGroup;
        private List<Cook> _cooks;
        public PizzeriaSimulator()
        {
            _cooks = new List<Cook>();
            _ordersGroup = new OrdersGroup();
        }
        public PizzeriaSimulator(string cooksFilePath, string ordersFilePath)
        {
            _cooks = FileWorker.ReadCooksFromFileEmployingRandomly(cooksFilePath);
            _ordersGroup = FileWorker.MakeOrdersGroupFromFile(ordersFilePath);
            Cook temp = _cooks.First();
            IEnumerator<Cook> it = _cooks.GetEnumerator();
            it.MoveNext();
            while (it.MoveNext())
            {
                temp.SetNext(it.Current);
                temp = it.Current;
            }
        }
        public void StartProcess()
        {
            if (_ordersGroup.Orders.Count != 0 && _cooks.Count != 0) 
            {
                List<(Guid?, double)> cooksBusiness = new();
                foreach (var item in _cooks)
                {
                    cooksBusiness.Add((item.ID, 0));
                }
                Order temp = _ordersGroup.Orders.Dequeue();
                for (int i = 0; i < temp.Amount; ++i)
                {
                    var result = _cooks.First().Handle(temp);
                    if (result != (null, null))
                    {
                    }                    
                }
            }
        }
    }
}
