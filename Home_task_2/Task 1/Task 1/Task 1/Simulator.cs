using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class Simulator
    {
        private User _user;
        private WaterTower _waterTower;
        private Pump _pump;
        public Simulator(double maxLevel, in Pump pump, in User user)
        {
            _pump = new Pump(pump);
            _waterTower = new WaterTower(maxLevel, _pump);
            _user = new User(user);
        }
        public void MeetNeeds() //Задоволення потреб користувача
        {
            if(_user.Consumption > 0) 
            {
                bool check;
                while (_user.Consumption > _waterTower.MaxWaterLevel) { //Поки наші потреби більші за максимальний рівень води...
                    check = _waterTower.ReleaseWater(_waterTower.MaxWaterLevel); //...викликатимемо від максимума (клас WaterTower не може брати на себе в методі більше води, аніж у ньому може бути)
                    if(check)
                    {
                        _user.Consumption -= _waterTower.MaxWaterLevel; //Потреби зменшуємо на максимальне значення рівня
                        _user.Received += _waterTower.MaxWaterLevel; //Отримання збільшуємо
                    }
                }
                check = _waterTower.ReleaseWater(_user.Consumption); //Тут уже взяли, коли не максимальний рівень води
                if(check)
                {
                    _user.Received += _user.Consumption; //Отримання дорівнює початковим потребам
                    _user.Consumption = 0; //Потреби занулюються
                }
            }
        }
        public override string ToString()
        {
            return $"{_pump} {_waterTower} {_user}";
        }
    }
}
