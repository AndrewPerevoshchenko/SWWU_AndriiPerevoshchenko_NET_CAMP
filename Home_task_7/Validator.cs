using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal static class Validator //Не зовсім валідатор, скоріше - допоміжний функціональний клас
    {
        public static bool CheckOppositeDirections(in GeographicalDirection from, in GeographicalDirection to) //Перевірка протилежних напрямків
        {
            return (sbyte)from == -(sbyte)to;
        }
        public static bool CheckOppositeSynchonization() //Перевірка: чи синхронізовані за часом проміжки "Зелений-жовтий-червоний" та "Червоний-жовтий-зелений"
        {
            return ColoursTimer.GreenToYellow + ColoursTimer.YellowToRed == ColoursTimer.RedToYellow + ColoursTimer.YellowToGreen;
        }
        public static List<List<TrafficLight>> GroupByDirection(List<TrafficLight> trafficLights) //Групування за напрямками
        {
            List<List<TrafficLight>> trafficGroups = new List<List<TrafficLight>>();
            for (sbyte i = 1; i <= (sbyte)GeographicalDirection.NorthEast; ++i)
            {
                List<TrafficLight> temp = trafficLights.Select(t => t).Where(t => Math.Abs((sbyte)t.Direction.Item1) == i).ToList();
                if (temp.Count != 0)
                {
                    trafficGroups.Add(temp);
                }               
            }
            return trafficGroups;
        }
    }
}
