using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_9
{
    internal class Order: IEqualityComparer<Order>
    {
        public const uint COOKING_TIME_STANDART = 1;
        private Guid _ID = Guid.NewGuid();
        private readonly string _name;
        private readonly DishType _dishType;
        private readonly uint _amount;
        private readonly double _cookingTime;
        public Guid ID
        {
            get { return _ID; }
        }
        public string Name
        {
            get { return _name; }
        }
        public DishType DishType
        {
            get { return _dishType; }
        }
        public uint Amount
        {
            get { return _amount; }
        }
        public double CookingTime
        {
            get { return _cookingTime; }
        }
        public Order(string name, DishType dishType, uint amount = 1, double cookingTime = COOKING_TIME_STANDART)
        {
            _name = name != String.Empty ? name : throw new InvalidDataException("Dish name cannot be empty");
            _dishType = dishType;
            _amount = amount;
            _cookingTime = cookingTime > 0 ? cookingTime : COOKING_TIME_STANDART; 
        }

        public bool Equals(Order? x, Order? y)
        {
            if (x != null && y != null)
            {
                return x.ID.Equals(y.ID);
            }
            return false;
        }

        public int GetHashCode([DisallowNull] Order obj)
        {
            return obj.ID.GetHashCode();
        }
    }
}
