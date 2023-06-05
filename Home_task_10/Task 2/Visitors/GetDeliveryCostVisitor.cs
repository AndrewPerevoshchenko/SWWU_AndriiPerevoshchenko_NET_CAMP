
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal class GetDeliveryCostVisitor : IGoodsVisitor //Наш візітор
    {
        private readonly double _baseCost; //Базова вартість (без коефіцієнтів)
        private readonly SortedDictionary<uint, double> _weightCoeffs; //Таблиця: вага - коефіцієнт
        private readonly SortedDictionary<uint, double> _dimensionsCoeffs; //Таблиця: габарити - коефіцієнт
        public GetDeliveryCostVisitor(double baseCost, SortedDictionary<uint, double> weightCoeffs, SortedDictionary<uint, double> dimensionCoeffs) 
        {
            _baseCost = baseCost > 0 ? baseCost : 0;
            _weightCoeffs = weightCoeffs != null ? new SortedDictionary<uint, double>(weightCoeffs) : new();
            _dimensionsCoeffs = dimensionCoeffs != null ? new SortedDictionary<uint, double>(dimensionCoeffs) : new();
            _weightCoeffs.Add(uint.MaxValue, _weightCoeffs.Last().Value);
            _dimensionsCoeffs.Add(uint.MaxValue, _dimensionsCoeffs.Last().Value);
        }
        private double FindBaseCost(double weight, in Dimensions measurements) //Пошук базової ціни (за розміром та вагою)
        {
            double weightCoeff = 1;
            double dimensionsCoeff = 1;
            foreach (var pair in _weightCoeffs)
            {
                if (pair.Key >= weight)
                {
                    weightCoeff = pair.Value;
                    break;
                }
            }
            foreach (var pair in _dimensionsCoeffs)
            {
                if (pair.Key >= measurements.GetVolume())
                {
                    dimensionsCoeff = pair.Value;
                    break;
                }
            }
            return (_baseCost * dimensionsCoeff) * weightCoeff;
        }
        public double VisitClothesDelivery(Clothes clothes) //Посилка: одежа
        {
            return FindBaseCost(clothes.Weight, clothes.Measurements);
        }
        public double VisitElectronicsDelivery(Electronics electronics) //Посилка: електроніка (не забуваємо за коефіцієнт перевищення розміру)
        {
            double baseCost = FindBaseCost(electronics.Weight, electronics.Measurements);
            return electronics.Measurements.GetVolume() > electronics.VolumeStandart ? baseCost * electronics.ExceedingSizeCoefficient : baseCost;
        }
        public double VisitFoodsDelivery(Foods foods) //Посилка: продукти (не забуваємо за коефіцієнт терміновості)
        {
            return FindBaseCost(foods.Weight, foods.Measurements) * foods.UrgencyCoefficient;
        }
    }
}
