using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal delegate string ColourWork();
    internal class CrossroadsSimulator : ICloneable //Симулятор одного перехрестя
    {
        private Logs _logs; //логування
        private Crossroads _crossroads;
        private Scheme _scheme;
        private double _timer; //власний таймер
        public event ColourWork? ColourChanged; //подія щодо зміни кольору
        public Logs Logs
        {
            get { return (Logs)_logs.Clone(); }
        }
        public CrossroadsSimulator(Crossroads crossroads, Scheme scheme)
        {
            _logs = new Logs();
            _crossroads = (Crossroads)crossroads.Clone();
            _timer = 0;
            _scheme = (Scheme)scheme.Clone();
        }
        private (double[,], double[], Dictionary<int, (TrafficLight, double[])>) MakeTimersTable()
        //Дістаємо булівську таблицю, унікальні таймінги, словник: айді світлофора, світлофор та власні таймінги в періоді
        {
            SortedSet<double> timings = new SortedSet<double>();
            var groups = Validator.MakeOppositeGroups(_crossroads.GetRoadLanesOriginal().ToList());
            if (groups.Count == 0)
            {
                throw new InvalidDataException("There are no groups of the lanes in the simulator");
            }
            Dictionary<int, (TrafficLight, double[])> trafficLightsAccess = new();
            int itID = 0;
            bool startsFromRed = false;
            for (int i = 0; i < groups.Count; ++i)
            {
                for (int j = 0; j < groups[i].Count; ++j)
                {
                    int k = 0;
                    foreach (var pair in groups[i][j].Lanes)
                    {
                        //Лише для першої групи буде зелений колір. Інакше: червоний
                        pair.Key.Colour = i == 0 ? TrafficColour.Green : TrafficColour.Red;
                        var currentTimings = new List<double>(pair.Value.Item1.findTimingPeriodicity(startsFromRed)); //таймінги за принципом: сума попередніх і даного - тобто таймер індивідуальний
                        foreach (double time in currentTimings)
                        {
                            timings.Add(time);
                        }
                        trafficLightsAccess.Add(itID, (pair.Key, currentTimings.ToArray()));
                        ++itID;
                        ++k;
                    }
                }
                startsFromRed = true;
            }
            double[,] table = new double[itID, timings.Count];
            double[] timingsArray = timings.ToArray();
            for (int i = 0; i < table.GetLength(0); ++i)
            {
                for (int j = 0; j < table.GetLength(1); ++j)
                {
                    if (Array.IndexOf(trafficLightsAccess[i].Item2, timingsArray[j]) != -1)
                    {
                        table[i, j] = 1; //Одинички - там де наш світлофор змінюватиме колір (у нашому випадку це чотири рази)
                    }
                }
            }
            return (table, timingsArray, trafficLightsAccess);
        }
        private void ChangeColours(IEnumerable<int> trafficLightIndexes, Dictionary<int, (TrafficLight, double[])> trafficLightsAccess)
        //Змінити колір у вибраних світлофорах (маємо айдішники та чудо-словник)
        {
            foreach (int i in trafficLightIndexes)
            {
                trafficLightsAccess[i].Item1.ColourNext();
            }
            ColourChanged?.Invoke(); //Інвокаємо подію
        }
        public void Start(double timeCeiling) //Запуск симулятора (сейлінг - межа)
        {
            if (timeCeiling < 0)
            {
                throw new InvalidDataException("Incorrect timing (time cannot be negative)");
            }
            (var tableID, var uniqueTimings, var trafficLightsTimersPairs) = MakeTimersTable();
            double[] timingsDifference = new double[uniqueTimings.Length];
            timingsDifference[0] = uniqueTimings[0];
            for (int i = 1; i < uniqueTimings.Length; ++i) //Рахуємо різницю в таймінгах між змінами
            {
                timingsDifference[i] = uniqueTimings[i] - uniqueTimings[i - 1];
            }
            ToString(); //Виклик для нульового таймера
            while (_timer < timeCeiling)
            {
                for (int j = 0; j < tableID.GetLength(1); ++j) //j-цикл - фактично один період зміни кольорів
                {
                    if (_timer + timingsDifference[j] > timeCeiling) //зупиняємо, якщо наступний таймінг виходить за межу
                    {
                        return;
                    }
                    _timer += timingsDifference[j];
                    List<int> coloursChanged = new();
                    for (int i = 0; i < tableID.GetLength(0); ++i)
                    {
                        if (tableID[i, j] == 1)
                        {
                            coloursChanged.Add(i); //айді світлофорів, які змінять колір
                        }
                    }
                    ChangeColours(coloursChanged, trafficLightsTimersPairs); //змінюємо колір, власне
                }
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("TIME: " + _timer.ToString() + "\n");
            sb.Append(_crossroads.ToString());
            string result = sb.ToString();
            _logs.AddLogs(result + "\n"); //логуємо зміни
            return result;
        }
        public object Clone()
        {
            CrossroadsSimulator copy = new CrossroadsSimulator(_crossroads, _scheme); //немає сенсу копіювання логів та таймеру
            return copy;
        }
    }
}