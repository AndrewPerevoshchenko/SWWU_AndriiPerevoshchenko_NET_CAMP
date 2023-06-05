using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal class Electronics : RetailMerchandise
    {
        private readonly double _volumeStandart = 500; //Стандартний розмір для даного екземпляру
        private readonly double _exceedingSizeCoefficient = 1.1; //Коефіцієнт за перевищення розміру
        public double VolumeStandart
        {
            get { return _volumeStandart; }
        }
        public double ExceedingSizeCoefficient
        {
            get { return _exceedingSizeCoefficient; }
        }
        public Electronics(string name = "", double weight = 0, Dimensions measurements = new Dimensions(), double standartVolume = 500, double exccedingSizePercentages = 1.1) : base(name, weight, measurements)
        {
            _volumeStandart = standartVolume;
            _exceedingSizeCoefficient = exccedingSizePercentages > 0 ? exccedingSizePercentages : 1.1;        
        }
        public override double Accept(IGoodsVisitor visitor)
        {
            return visitor.VisitElectronicsDelivery(this);
        }
        public override string ToString()
        {
            return $"{base.ToString()} || {_volumeStandart} || {_exceedingSizeCoefficient}";
        }
    }
}
