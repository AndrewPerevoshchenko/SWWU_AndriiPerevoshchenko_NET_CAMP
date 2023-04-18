using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal class Goods : Component
    {
        public Goods(string name, double length, double width, double height) : base(name, length, width, height)
        {

        }

        public override string ToString()
        {
            return "GOODS: " + base.ToString() + "\n";
        }
    }
}
