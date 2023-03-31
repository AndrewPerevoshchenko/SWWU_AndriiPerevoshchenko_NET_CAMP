using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;

namespace Task_1
{
    internal class WaterTower
    {
        private const double MAX_STANDART = 1;
        private const double RELEASE_STANDART = 1;
        private readonly double _maxWaterLevel;
        private readonly double _releaseSpeed;
        private double _currentLevel;
        private Pump _pump;
        private const int VISUALISER_STOP = 100;
        private double _timer = 0;
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
        public WaterTower(Pump pump, double maxWaterLevel = MAX_STANDART, double releaseSpeed = RELEASE_STANDART)
        {
            _maxWaterLevel = maxWaterLevel > 0 ? maxWaterLevel : MAX_STANDART;
            _releaseSpeed = releaseSpeed > 0 ? releaseSpeed : RELEASE_STANDART; 
            _currentLevel = _maxWaterLevel;
            _pump = pump != null ? new Pump(pump): new Pump(Pump.POWER_STANDART);
            _timer = 0;
        }
        public WaterTower(in WaterTower other)
        {
            if(other != null)
            {
                _maxWaterLevel = other._maxWaterLevel;
                _releaseSpeed = other._releaseSpeed;
                _currentLevel = other._currentLevel;
                _pump = new Pump(other._pump);
                _timer = other._timer;
            }
        }
        private bool isEmpty()
        {
            return _currentLevel == 0;
        }
        public bool ReleaseWater(double willing)
        {
            if(willing > _maxWaterLevel || willing <= 0)
            {
                return false;
            }
            if(willing > _currentLevel)
            {
                willing -= _currentLevel;
                _timer += _currentLevel / _releaseSpeed;
                SimulatorForm.checkSystem();
                Thread.Sleep(VISUALISER_STOP);
                _currentLevel = 0;
                AutomaticalFillTower();
                _currentLevel -= willing;               
                _timer += willing / _releaseSpeed;
                SimulatorForm.checkSystem();      
                Thread.Sleep(VISUALISER_STOP);
            }
            else
            {
                _currentLevel -= willing;
                _timer += willing / _releaseSpeed;
                SimulatorForm.checkSystem();
                Thread.Sleep(VISUALISER_STOP);
            }
            if (isEmpty())
            {
                AutomaticalFillTower();
            }
            return true;
        }
        private bool AutomaticalFillTower()
        {
            if(_currentLevel == 0)
            {
                _pump.IsOn = true;
                double remainder = _maxWaterLevel - _pump.Power;
                while(_currentLevel <= remainder)
                {
                    _currentLevel += _pump.Power;
                    ++_timer;
                    SimulatorForm.checkSystem();
                    Thread.Sleep(VISUALISER_STOP);
                }
                _currentLevel += _maxWaterLevel - _currentLevel;
                _timer += (_maxWaterLevel - _currentLevel) / _pump.Power;
                SimulatorForm.checkSystem();
                Thread.Sleep(VISUALISER_STOP);
                _pump.IsOn = false;
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            return $"{_maxWaterLevel} {_currentLevel} {_timer} {_pump}";
        }
    }
}
