using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class User
    {
        private double _consumption;
        private double _received = 0;
        public double Consumption
        {
            get { return _consumption; }
            set { _consumption = value > 0 ? value : 0; }
        }
        public double Received
        {
            get { return _received; }
            set { _received = value > 0 ? value : 0; }
        }
        public User(double consumption)
        {
            if(consumption > 0)
            {
                _consumption = consumption;
            }
        }
        public User(in User other)
        {
            if(other != null)
            {
                _consumption = other._consumption;
                _received = other._received;
            }
        }
        public override string ToString()
        {
            return $"{_consumption} {_received}";
        }
    }
}
