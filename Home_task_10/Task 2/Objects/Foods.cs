using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal class Foods : RetailMerchandise
    {
        private readonly double _urgencyCoefficient; //Коефіцієнт терміновості (1, якщо звичайні терміни)
        public double UrgencyCoefficient
        {
            get { return _urgencyCoefficient; }
        }
        public Foods(string name = "", double weight = 0, Dimensions measurements = new Dimensions(), double urgencyPercentage = 1) : base(name, weight, measurements)
        {
            _urgencyCoefficient = urgencyPercentage > 0 ? urgencyPercentage : 1;
        }
        public override double Accept(IGoodsVisitor visitor)
        {
            return visitor.VisitFoodsDelivery(this);
        }
        public override string ToString()
        {
            return $"{base.ToString()} || {_urgencyCoefficient}";
        }
    }
}
