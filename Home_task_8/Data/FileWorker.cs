using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal static class FileWorker //Клас для роботи з файлами
    {
        private static Scheme createScheme(int index, HashSet<RoadLanes> rl) //Створення схеми (можна додавати case в розширенні)
        {
            switch (index)
            {
                default:
                    return new OppositeAsynchronizedScheme(rl);
            }
        }
        private static (Crossroads, Scheme) ReadCrossroadsSchemePairFromFile(string path) //З одного файлу дістаємо перехрестя та приєднуємо схему
        {
            Crossroads crossroads = new Crossroads();
            if (File.Exists(path))
            {
                string[] strings = File.ReadAllLines(path);
                if (strings.Length > 0) 
                {
                    bool.TryParse(strings[0], out bool leftPossibility);                    
                    List<RoadLanes> addedLanes = new();
                    for (int i = 2; i < strings.Length; ++i) 
                    {
                        string[] str = strings[i].Split(" | ", StringSplitOptions.RemoveEmptyEntries);
                        if (str.Length > 0)
                        {
                            Enum.TryParse(str[0], out GeographicalDirection from);
                            Enum.TryParse(str[1], out GeographicalDirection to);
                            Enum.TryParse(str[2], out DirectionScheme directionScheme);
                            Enum.TryParse(str[3], out TrafficColour colour);
                            uint.TryParse(str[4], out uint lane);
                            double.TryParse(str[5], out double RY);
                            double.TryParse(str[6], out double YG);
                            double.TryParse(str[7], out double GY);
                            double.TryParse(str[8], out double YR);
                            int addedIndex = addedLanes.FindIndex(n => n.Part == from);
                            if (addedIndex == -1)
                            {
                                RoadLanes roadLanes = new RoadLanes(from);
                                roadLanes.AddLane(new TrafficLight(from, to, directionScheme, colour), new ColoursTimer(RY, YG, GY, YR), lane, leftPossibility);
                                addedLanes.Add(roadLanes);
                            }
                            else
                            {
                                addedLanes[addedIndex].AddLane(new TrafficLight(from, to, directionScheme, colour), new ColoursTimer(RY, YG, GY, YR), lane, leftPossibility);
                            }
                        }
                    }
                    foreach (RoadLanes rl in addedLanes)
                    {
                        crossroads.AddLanes(rl);
                    }
                    if (Assembly.GetAssembly(typeof(Scheme)) == null)
                    {
                        throw new NotImplementedException("There are no schemes");
                    }
                    //Це для того, щоб визначити: чи назва в стрічці strings[1] відповідає реально існуючій назві підкласу абстракції Scheme
                    Type[] schemes = Assembly.GetAssembly(typeof(Scheme)).GetTypes().Where(t => t.IsClass && t.IsSubclassOf(typeof(Scheme))).ToArray();
                    int indexScheme = Array.FindIndex(schemes, n => n.Name == strings[1]);
                    Scheme scheme = createScheme(indexScheme, crossroads.RoadLanesSet);
                    return (crossroads, scheme);
                }
                throw new InvalidDataException("File is empty");
            }
            throw new FileNotFoundException("File is not found");
        }
        public static SimulatorsGroup CreateCrossroadsGroupSimulator(params string[] paths) //Створити по всіх посиланнях n-ну кількість симуляцій одного перехрестя
        {
            SimulatorsGroup simulatorsGroup = new();
            foreach(string path in paths) 
            {
                var pair = ReadCrossroadsSchemePairFromFile(path);
                CrossroadsSimulator crossroadsSimulator = new CrossroadsSimulator(pair.Item1, pair.Item2);
                simulatorsGroup.AddSimulator(crossroadsSimulator);
            }
            return simulatorsGroup;
        }
    }
}