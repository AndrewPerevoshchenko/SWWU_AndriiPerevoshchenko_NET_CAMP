using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Home_task_7
{
    internal static class Validator
    {
        public static bool CheckOppositeDirections(in GeographicalDirection from, in GeographicalDirection to)
        //Перевірка: чи напрямки є протилежними
        {
            return Math.Abs((sbyte)from - (sbyte)to) == Enum.GetNames(typeof(GeographicalDirection)).Length / 2;
        }
        public static bool CheckPerpendicularDirections(in GeographicalDirection from, in GeographicalDirection to)
        //Перевірка: чи напрямки є перпендикулярними
        {
            int geoSize = Enum.GetNames(typeof(GeographicalDirection)).Length;
            return Math.Abs((sbyte)from - (sbyte)to) == geoSize / 4 || Math.Abs((sbyte)from - (sbyte)to) == geoSize - geoSize / 4;
        }
        private static bool CheckRotaryAccess(in IEnumerable<uint> values, in TrafficLight trafficLight, uint wishedLane, ref (bool, uint) hasLeftRotary, ref bool hasRightRotary)
        //Перевірка, що стосується повороту направо чи наліво
        {
            sbyte from = (sbyte)trafficLight.Direction.From;
            sbyte to = (sbyte)trafficLight.Direction.To;
            int quarterGeoSize = Enum.GetNames(typeof(GeographicalDirection)).Length / 4;
            if ((from > quarterGeoSize && to == from - quarterGeoSize) || (from <= quarterGeoSize && to == from + quarterGeoSize * 3))
            {
                //Усе про правий поворот і можливі перевірки
                if (!hasRightRotary && wishedLane == 0)
                {
                    hasRightRotary = true;
                    return true;
                }
            }
            else if ((from < quarterGeoSize * 3 + 1 && to == from + quarterGeoSize) || (from >= quarterGeoSize + 1 && to == from - quarterGeoSize * 3))
            {
                //Лівий поворот і його особливості
                uint max = values.Max();
                if (!hasLeftRotary.Item1 && wishedLane >= max)
                {
                    hasLeftRotary.Item2 = wishedLane;
                    hasLeftRotary.Item1 = true;
                    return true;
                }
            }
            return false;
        }
        public static bool CheckTrafficLightOnLane(in IEnumerable<uint> values, in TrafficLight trafficLight, uint wishedLane, ref (bool, uint) hasLeftRotary, ref bool hasRightRotary)
        //Перевірка світлофорів, які вже можуть знаходитися на заданій смузі
        {
            int amount = values.Select(t => t).Where(t => t == wishedLane).Count();
            switch (trafficLight.Direction.Scheme)
            {
                case DirectionScheme.Rectilinear:
                    return amount == 0;
                case DirectionScheme.Rotary:
                    if (amount <= 1)
                    {
                        if (CheckRotaryAccess(values, trafficLight, wishedLane, ref hasLeftRotary, ref hasRightRotary))
                        {
                            return true;
                        }
                    }
                    return false;
                default:
                    return false;                   
            }
        }
        public static bool CheckOppositeGroups(in List<RoadLanes> roadLanes)
        //Перевірка протилежних груп
        {
            foreach (RoadLanes group in roadLanes)
            {
                int tempIndex = roadLanes.FindIndex(t => CheckOppositeDirections(group.Part, t.Part) == true);
                if (tempIndex == -1)
                {
                    return false;
                }
            }
            return true;
        }
        public static List<List<RoadLanes>> MakeOppositeGroups(List<RoadLanes> roadLanesGroup)
        //Формування протилежних за напрямком груп
        {
            List<List<RoadLanes>> groups = new();
            int geoSize = Enum.GetNames(typeof(GeographicalDirection)).Length;
            for (int i = 1; i <= geoSize / 2; ++i)
            {
                int indexFirst = roadLanesGroup.FindIndex(n => (int)n.Part == i);
                int indexOpposite = roadLanesGroup.FindIndex(n => (int)n.Part == geoSize / 2 + i); 
                if (indexFirst != -1 && indexOpposite != -1)
                {
                    groups.Add(new List<RoadLanes>()
                    {
                        roadLanesGroup[indexFirst],
                        roadLanesGroup[indexOpposite]
                    });
                }
                else if (indexFirst == -1 ^ indexOpposite == -1)
                {
                    throw new InvalidDataException("There is no opposite groups of lanes");
                }
            }
            return groups;
        }
        public static bool CheckOppositeGroupTimersAsynchronization(in List<RoadLanes> roadLanesGroup)
        //Перевірка: чи максимум із таймінгу зелених кольорів однієї групи смуг не перевищує суму жовтих та червоних кольорів усіх інших груп
        //У випадку перевищення, буде ситуація, що рух на перпендикулярних дорогах буде доступний одночасно (а це неможливо за умовою)
        {
            List<List<RoadLanes>> groups = Validator.MakeOppositeGroups(roadLanesGroup);
            Dictionary<int, double> checkerMaxGreen = new();
            for (int i = 0; i < groups.Count; ++i)
            {
                double maxGreen = groups[i].Max(n => n.Lanes.Values.Max(t => t.Item1.GreenToYellow));
                checkerMaxGreen.Add(i, maxGreen);
            }
            double summaryMaxGreen = checkerMaxGreen.Sum(t => t.Value);
            for (int i = 0; i < groups.Count; ++i)
            {
                double summaryOther = summaryMaxGreen - checkerMaxGreen[i];
                foreach(RoadLanes lanes in groups[i])
                {
                    foreach (var pair in lanes.Lanes.Values)
                    {
                        if (pair.Item1.YellowToRed + pair.Item1.YellowToGreen + pair.Item1.RedToYellow < summaryOther)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public static bool CheckTimersPeriodicity(in List<RoadLanes> roadLanesGroup)
        //Світлофор має період Червоний-жовтий-зелений-жовтий-червоний (чи починаючи із зеленого), а значить, щоб не виникало суперечок
        //у плані зміщення таймінгів (відносно одного світлофора інші змінюють колір завжди однаково - так має бути) треба перевірити:
        //чи сума таймінгів у періоді (тобто сума таймерів переключань кожного з кольорів) у всіх є еквівалентною (в розширенні достатньо буде кратності)
        {
            List<ColoursTimer> timers = new();
            foreach (RoadLanes item in roadLanesGroup)
            {
                timers.AddRange(item.Lanes.Select(t => t.Value.Item1).ToList());   
            }
            if (timers.Count != 0)
            {
                double timingConst = timers[0].RedToYellow + timers[0].GreenToYellow + timers[0].YellowToRed + timers[0].YellowToGreen;
                IEnumerator<ColoursTimer> it = timers.GetEnumerator();
                while(it.MoveNext())
                {
                    if (timingConst != it.Current.YellowToGreen + it.Current.YellowToRed + it.Current.GreenToYellow + it.Current.RedToYellow)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool CheckRoadLanesFullness(in RoadLanes roadLanes)
        //Чи всі смуги в одній групі заповнені - перевірка
        {
            uint[] lines = roadLanes.Lanes.Values.Select(t => t.Item2).ToArray();
            uint maxElement = lines.Max();
            for (uint i = 0; i < maxElement; ++i)
            {
                if (Array.IndexOf(lines, i) == -1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}