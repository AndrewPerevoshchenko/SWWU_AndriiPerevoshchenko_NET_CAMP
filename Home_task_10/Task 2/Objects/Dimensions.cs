using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal struct Dimensions //Структура, що визначає габарити
    {
        private double _length;
        private double _width;
        private double _height;
        public double Length
        {
            get { return _length; }
            set { _length = value > 0 ? value : 0; }
        }
        public double Width
        {
            get { return _width; }
            set { _width = value > 0 ? value : 0; }
        }
        public double Height
        {
            get { return _height; }
            set { _height = value > 0 ? value : 0; }
        }
        public Dimensions(double length = 0, double width = 0, double height = 0)
        {
            Length = length;
            Width = width;
            Height = height;
        }
        public double GetVolume() //Пошук об'єму товару
        {
            return _length * _width * _height;
        }
        public override string ToString()
        {
            return $"{Length} {Width} {Height}";
        }
    }
}
