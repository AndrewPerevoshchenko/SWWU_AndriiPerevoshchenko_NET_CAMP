using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    public class ElectricityMeter //Одиничний стан лічильника
    {
        public const string COUNTER_FORMAT = "D6";
        public const string CULTURE_FORMAT = "uk-UA";
        private DateTime _removalDate; //Дата сплати
        private uint _counter; //Показник лічильника
        public uint Counter
        {
            get { return _counter; }
        }
        public DateTime RemovalDate
        {
            get { return _removalDate; }
        }
        public ElectricityMeter(in DateTime removalDate = new DateTime(), uint counter = 0)
        {
            _removalDate = new DateTime(removalDate.Year, removalDate.Month, removalDate.Day);
            _counter = counter;
        }
        public ElectricityMeter(in ElectricityMeter other)
        {
            _removalDate = new DateTime(other._removalDate.Year, other._removalDate.Month, other._removalDate.Day);
            _counter = other._counter;
        }
        public override string ToString()
        {
            return _removalDate.ToString("MMMM") + " " + _removalDate.ToString("dd.MM.yy", new CultureInfo(CULTURE_FORMAT)) + " " + _counter.ToString(COUNTER_FORMAT);
        }
    }
}

