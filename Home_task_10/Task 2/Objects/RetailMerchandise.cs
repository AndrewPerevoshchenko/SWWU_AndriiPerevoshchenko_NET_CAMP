using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal abstract class RetailMerchandise //Абстракція: товар
    {
        private string _name; //Назва товару
        private double _weight; //Вага товару
        private Dimensions _measurements; //Габарити товару
        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }
        public Dimensions Measurements
        {
            get { return _measurements; }
            protected set { _measurements = value; }
        }
        public double Weight
        {
            get { return _weight; }
            protected set { _weight = value > 0 ? value : 0; }
        }
        public RetailMerchandise(string name = "", double weight = 0, Dimensions dimensions = new Dimensions())
        {
            Name = name;
            Weight = weight;
            Measurements = dimensions;
        }
        public abstract double Accept(IGoodsVisitor visitor);
        public override string ToString()
        {
            return $"{_name} || {_weight} || {_measurements}";
        }
    }
}
