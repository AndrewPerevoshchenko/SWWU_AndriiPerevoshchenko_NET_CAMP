using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Task_2 //Паттерн: компонувальник
{
    internal struct Dimensions
    {
        public double _length;
        public double _width;
        public double _height;
        public Dimensions(double length = 0, double width = 0, double height = 0)
        {
            if (length > 0 && width > 0 && height > 0)
            {
                _length = length;
                _width = width;
                _height = height;
            }
        }
        public override string ToString()
        {
            return $"length: {_length}, width: {_width}, height: {_height}";
        }
    }
    internal abstract class Component //Абстрактний клас для відділів та товарів
    {
        protected string _name;
        protected Dimensions _dimensions;
        public string Name
        {
            get { return _name; }
        }
        public Dimensions Dimensions
        {
            get { return _dimensions; }
            set { _dimensions = value; }
        }
        public Component(string name, double length = 0, double width = 0, double height = 0)
        {
            _name = name;
            _dimensions = new Dimensions(length, width, height);
        }
        public virtual void Add(in Component component) { }
        public override string ToString()
        {
            return _name + " | " + _dimensions + "\n";
        }
    }
}