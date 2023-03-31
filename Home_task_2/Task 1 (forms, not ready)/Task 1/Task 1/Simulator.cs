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
        public WaterTower WaterTower
        {
            get { return _waterTower; }
        }
        public Simulator(double maxLevel, double releaseLevel, in Pump pump, in User user)
        {
            _pump = new Pump(pump);
            _waterTower = new WaterTower(_pump, maxLevel, releaseLevel);
            _user = new User(user);
        }
        public void MeetNeeds()
        {
            if(_user.Consumption > 0)
            {
                bool check;
                while (_user.Consumption > _waterTower.MaxWaterLevel) {
                    check = _waterTower.ReleaseWater(_waterTower.MaxWaterLevel);
                    if(check)
                    {
                        _user.Consumption -= _waterTower.MaxWaterLevel;
                        _user.Received += _waterTower.MaxWaterLevel;
                    }
                }
                check = _waterTower.ReleaseWater(_user.Consumption);
                if(check)
                {
                    _user.Received += _user.Consumption;
                    _user.Consumption = 0;
                }
            }
        }
        public Simulator(in Simulator other)
        {
            if (other != null)
            {
                _user = new User(other._user);
                _waterTower = new WaterTower(other._waterTower);
                _pump = new Pump(other._pump);
            }
        }
        public override string ToString()
        {
            return $"{_pump} {_waterTower} {_user}";
        }
    }
}
