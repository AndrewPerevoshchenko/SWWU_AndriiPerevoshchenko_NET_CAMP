using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal class Store
    {
        private List<RetailMerchandise> _merchandises; //Список товарів
        public Store()
        {
            _merchandises = new();
        }
        public List<RetailMerchandise> Merchandises
        {
            get { return new List<RetailMerchandise>(_merchandises);}
        }
        public void AddMerchandise(RetailMerchandise merchandise)
        {
            if (merchandise != null)
            {
                _merchandises.Add(merchandise);
            }
        }
        public void RemoveMerchandise(RetailMerchandise merchandise)
        {
            if (merchandise != null)
            {
                _merchandises.Remove(merchandise);
            }
        }
        public List<double> Accept(IGoodsVisitor visitor)
        {
            List<double> result = new List<double>();
            foreach (RetailMerchandise merchandise in _merchandises)
            {
                result.Add(merchandise.Accept(visitor));
            }
            return result;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (RetailMerchandise item in _merchandises)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }
    }
}
