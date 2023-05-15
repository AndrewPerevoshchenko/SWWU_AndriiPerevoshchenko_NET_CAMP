using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal abstract class Scheme: ICloneable //Абстрактний клас схем, куди можна додавати нові
    {
        protected HashSet<RoadLanes> _lanes;
        protected Scheme(HashSet<RoadLanes> roadLanes) 
        {
            _lanes = new HashSet<RoadLanes>(roadLanes) ?? new HashSet<RoadLanes>(); 
        }
        public abstract void CheckSystemPeriodical(); //Перевірити періодичність 
        public abstract void CheckSystemConditionals(); //Перевірити додаткові умови для самої схеми
        public abstract object Clone();
    }
}