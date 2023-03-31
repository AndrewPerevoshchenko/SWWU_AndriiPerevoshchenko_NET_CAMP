using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class Pump
    {
        public const double POWER_STANDART = 1;
        private readonly double _power;
        private bool _isOn = false;
        public double Power
        {
            get { return _power; }
        }
        public bool IsOn
        {
            set { _isOn = value; }
            get { return _isOn; }
        }
        public Pump(double power)
        {
            _power = power > 0 ? power : POWER_STANDART;
        }
        public Pump(in Pump other)
        {
            if(other != null)
            {
                _power = other._power;
                _isOn = other._isOn;
            }  
        }
        public override string ToString()
        {
            return $"{_power} {_isOn}";
        }
    }
}
