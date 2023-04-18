using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Task_1
{
    internal class WaterTower
    {
        private const double MAX_STANDART = 1;
        private readonly double _maxWaterLevel;
        private double _currentLevel;
        private Pump _pump;
        public double CurrentLevel
        {
            get { return _currentLevel; }
            set { 
                if(_currentLevel > value)
                {
                    _currentLevel = value;
                }
            }
        }
        public double MaxWaterLevel
        {
            get { return _maxWaterLevel; }
        }
        public WaterTower(double maxWaterLevel, Pump pump)
        {
            _maxWaterLevel = maxWaterLevel > 0 ? maxWaterLevel : MAX_STANDART;
            _currentLevel = _maxWaterLevel;
            _pump = pump != null ? new Pump(pump): new Pump(Pump.POWER_STANDART);
        }
        public WaterTower(in WaterTower other)
        {
            if(other != null)
            {
                _maxWaterLevel = other._maxWaterLevel;
                _currentLevel = other._currentLevel;
                _pump = new Pump(other._pump);
            }
        }
        private bool isEmpty()
        {
            return _currentLevel == 0;
        }
        public bool ReleaseWater(double willing) //Забираємо з башти необхідну кількість води
        {
            if(willing > _maxWaterLevel || willing <= 0) 
            {
                return false;
            }
            if(willing > _currentLevel) //Якщо бажання більше за реальну кількість, то забираємо всю, що маємо, а потім включаємо насос
            {
                willing -= _currentLevel; 
                _currentLevel = 0;
                AutomaticalFillTower(); //Автоматичне заповнення
                _currentLevel -= willing;      
            }
            else //Якщо бажання менше або рівне, то забираємо від каррента
            {
                _currentLevel -= willing;
            }
            if (isEmpty()) //Каррент міг обнулитися, тому, за потреби, автоматизацію підключаємо знову
            {
                AutomaticalFillTower();
            }
            return true;
        }
        private bool AutomaticalFillTower() //Заповнення водою від 0 до максимума
        {
            if(_currentLevel == 0)
            {
                _pump.IsOn = true;
                double remainder = _maxWaterLevel - _pump.Power; //Помпа заповнює певну кількість за одиницю часу (power), може залишитися шматочок
                while(_currentLevel <= remainder)
                {
                    _currentLevel += _pump.Power; //Додаємо за кожну одиницю часу водичку
                    // Найцікавіше питання, де час. Цей цикл виконається миттєво. Включити користувача в цей час наповнення не вдасться. Тому буде не хороша симуляція...
                }
                _currentLevel += _maxWaterLevel - _currentLevel; //А тепер дозаповнюємо те, що залишилося (ця величина менша за power і там часу треба менше одиниці)
                _pump.IsOn = false;
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            return $"{_maxWaterLevel} {_currentLevel} {_pump}";
        }
    }
}
