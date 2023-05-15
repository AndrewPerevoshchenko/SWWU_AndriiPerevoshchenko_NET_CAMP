using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal class Crossroads: ICloneable
    {
        //Нам потрібні лише унікальні за напрямком "від" (RoadLanes реалізовує IEqutable)
        private HashSet<RoadLanes> _roadLanesSet;
        public HashSet<RoadLanes> RoadLanesSet
        {
            get { return new HashSet<RoadLanes>(_roadLanesSet); }
        }
        public Crossroads()
        {
            _roadLanesSet = new();
        }
        public HashSet<RoadLanes> GetRoadLanesOriginal()
        {
            return _roadLanesSet;
        }
        public void AddLanes(RoadLanes roadLanes)
        {
            //Додати нову групу смуг
            if (roadLanes != null)
            {
                if (!Validator.CheckRoadLanesFullness(roadLanes))
                {
                    throw new InvalidDataException("There is not full road lanes group");
                }
                _roadLanesSet.Add(roadLanes);
                return;
            }
            throw new NullReferenceException();
        }
        public object Clone()
        {
            Crossroads copy = new Crossroads();
            copy._roadLanesSet = new(_roadLanesSet);
            return copy;
        }
        public override string ToString()
        {
            StringBuilder sb = new();
            foreach(var item in _roadLanesSet)
            {
                sb.Append(item.ToString() + "\n");
            }
            return sb.ToString();
        }
    }
}