using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    public class PersonalEnergy //Дані однієї особи
    {
        public const uint QUARTER_AMOUNT = 4;
        private uint _flat;
        private string _address;
        private string _lastName;
        private ElectricityMeter[] _measurements;
        public uint Flat
        {
            get { return _flat; }
        }
        public string Address
        {
            get { return _address; }
        }
        public string LastName
        {
            get { return _lastName; }
        }
        public ElectricityMeter[] Measurements
        {
            get { return _measurements; }
        }
        public PersonalEnergy(List<ElectricityMeter> electricityMeter, uint flat = 0, string address = "", string lastName = "")
        {
            _measurements = new ElectricityMeter[QUARTER_AMOUNT];
            if (electricityMeter.Count >= QUARTER_AMOUNT)
            {
                for (int i = 0; i < QUARTER_AMOUNT; ++i)
                {
                    _measurements[i] = new ElectricityMeter(electricityMeter[i]);
                }
            }
            _flat = flat;
            _address = address;
            _lastName = lastName;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int maxLength = "Measurements 0 ".Length;
            string format = $"{{0,-{maxLength}}}{{1}}";
            sb.AppendFormat(format, "Flat", "| " + _flat + "\n");
            sb.AppendFormat(format, "Address", "| " + _address + "\n");
            sb.AppendFormat(format, "Last name", "| " + _lastName + "\n");
            for (int i = 0; i < _measurements.Length; ++i)
            {
                sb.AppendFormat(format, $"Measurements {i+1}", "| " + _measurements[i].ToString() + "\n");
            }
            return sb.ToString();
        }
    }
}
