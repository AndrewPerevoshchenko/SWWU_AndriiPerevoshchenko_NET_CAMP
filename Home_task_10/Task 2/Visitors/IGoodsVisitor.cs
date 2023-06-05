using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal interface IGoodsVisitor
    {
        double VisitClothesDelivery(Clothes clothes);
        double VisitElectronicsDelivery(Electronics electronics);
        double VisitFoodsDelivery(Foods foods);
    }
}
