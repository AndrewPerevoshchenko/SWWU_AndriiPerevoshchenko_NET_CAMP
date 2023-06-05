using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class PaymentSystemCardTemplate: IEquatable<PaymentSystemCardTemplate>, ICloneable
    {
        private string _paymentSystemName; //Назва платіжної системи
        private HashSet<string> _accesibleBegins; //Припустимі початки номерів карток
        private HashSet<uint> _accessibleLengths; //Припустимі довжини номерів карток
        public string CardType
        {
            get { return _paymentSystemName; }
        }
        public HashSet<string> AccesibleBegins
        {
            get { return new HashSet<string>(_accesibleBegins); }
        }
        public HashSet<uint> AccesibleLengths
        {
            get { return new HashSet<uint>(_accessibleLengths); }
        }
        public PaymentSystemCardTemplate(string company, HashSet<string> begins, HashSet<uint> lengths)
        {
            _paymentSystemName = company != String.Empty ? company : throw new InvalidDataException("Name of the card cannot be empty");
            _accesibleBegins = new HashSet<string>();
            _accessibleLengths = new HashSet<uint>();
            if (begins.Count == 0) throw new InvalidDataException("Collection of the begins is empty");
            if (lengths.Count == 0) throw new InvalidDataException("Collection of the lengths is empty");
            foreach (string begin in begins)
            {
                _accesibleBegins.Add(begin);
            }
            foreach (uint length in lengths)
            {
                _accessibleLengths.Add(length);
            }
        }

        public object Clone()
        {
            return new PaymentSystemCardTemplate(_paymentSystemName, new HashSet<string>(_accesibleBegins), new HashSet<uint>(_accessibleLengths));
        }

        public bool Equals(PaymentSystemCardTemplate? other)
        {
            if (other != null)
            {
                return _paymentSystemName == other._paymentSystemName;
            }
            return false;
        }
    }
}
