using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal class Clothes : RetailMerchandise
    {
        public Clothes(string name = "", double weight = 0, Dimensions measurements = new Dimensions()) : base(name, weight, measurements) { }
        public override double Accept(IGoodsVisitor visitor)
        {
            return visitor.VisitClothesDelivery(this);
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
