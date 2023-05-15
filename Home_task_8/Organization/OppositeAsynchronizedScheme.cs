using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal class OppositeAsynchronizedScheme : Scheme
    {
        public OppositeAsynchronizedScheme(HashSet<RoadLanes> roadLanes) : base(roadLanes) 
        {
            CheckSystemConditionals();
            CheckSystemPeriodical();
        }
        public override void CheckSystemConditionals()
        {
            if (!Validator.CheckOppositeGroups(_lanes.ToList()))
            {
                throw new InvalidDataException("There are not opposite groups of road lanes");
            }
            if (!Validator.CheckOppositeGroupTimersAsynchronization(_lanes.ToList()))
            {
                throw new InvalidDataException("There are problems with forming opposite lanes groups or timers given are incorrect");
            }
        }
        public override void CheckSystemPeriodical()
        {
            if (!Validator.CheckTimersPeriodicity(_lanes.ToList()))
            {
                throw new InvalidDataException("There are issues with periodicity of timers");
            }
        }
        public override object Clone()
        {
            OppositeAsynchronizedScheme copy = new OppositeAsynchronizedScheme(_lanes);
            copy._lanes = _lanes;
            return copy;
        }
    }
}