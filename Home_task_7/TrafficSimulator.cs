using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal struct ColoursTimer
    {
        public const double MIN_TIMER = 1;
        private static double R_Y;
        private static double Y_G;
        private static double G_Y;
        private static double Y_R;
        public static double RedToYellow
        {
            get { return R_Y; }
            set { R_Y = value > MIN_TIMER ? value : MIN_TIMER; }
        }
        public static double YellowToGreen
        {
            get { return Y_G; }
            set { Y_G = value > MIN_TIMER ? value : MIN_TIMER; }
        }
        public static double GreenToYellow
        {
            get { return G_Y; }
            set { G_Y = value > MIN_TIMER ? value : MIN_TIMER; }
        }
        public static double YellowToRed
        {
            get { return Y_R; }
            set { Y_R = value > MIN_TIMER ? value : MIN_TIMER; }
        }
        public ColoursTimer(double red_yellow, double yellow_green, double green_yellow, double yellow_red)
        {
            R_Y = red_yellow;
            Y_G = yellow_green;
            G_Y = green_yellow;
            Y_R = yellow_red;
        }
    }
    internal class TrafficSimulator
    {
        private List<string> _logString;     
        private List<TrafficLight> _trafficLights;
        private ColoursTimer _coloursTimer;
        private double _timerAdded;
        public ColoursTimer ColoursTimer
        {
            get { return _coloursTimer; }
        }
        public double _timer;
        public TrafficSimulator(in ColoursTimer coloursTimer, params TrafficLight[] trafficLights)
        {
            _logString = new List<string>();
            _coloursTimer = coloursTimer;
            _trafficLights = new List<TrafficLight>();
            foreach (TrafficLight trafficLight in trafficLights)
            {
                _trafficLights.Add(trafficLight);
            }
            _timerAdded = -1;
        }
        
        /// <summary>
        /// Scheme, where only two groups of traffic lights with synchronized opposite blocks;
        /// [amount][structure][synchronization]: 
        /// T - two, O - opposite, S - synchronized
        /// </summary>
        /// <param name="StartSchemeTOS"></param>
        public string StartSchemeTOS(double timerCeil)
        {
            var groupedList = Validator.GroupByDirection(_trafficLights);
            if (!Validator.CheckOppositeSynchonization()) throw new Exception("Opposite traffic lights are not synchronized!");
            if (groupedList.Count != 2) throw new InvalidDataException("Incorrect amount of the groups");
            foreach(var trafficLight in groupedList[0])
            {
                trafficLight.Colour = TrafficColour.YellowBG;
            }
            foreach(var trafficLight in groupedList[1])
            {
                trafficLight.Colour = TrafficColour.YellowBR;
            }
            groupedList[0][0].ColourChanged += ToString;
            groupedList[0][1].ColourChanged += ToString;
            groupedList[1][0].ColourChanged += ToString;
            groupedList[1][1].ColourChanged += ToString;
            double timerFirst = 0;
            double timerSecond = 0;
            ToString();
            timerFirst = ColourNextGroup(groupedList[0]);
            timerSecond = ColourNextGroup(groupedList[1]);
            double tempTimer = 0;
            uint synchronized = 0;
            while (_timer <= timerCeil)
            {               
                if (timerFirst <= timerSecond)
                {
                    _timer += timerFirst;
                    tempTimer = timerFirst;
                    timerFirst = ColourNextGroup(groupedList[0]);
                    if (synchronized != 3)
                    {
                        ++synchronized;
                        _timer += (timerSecond - tempTimer);
                    }
                    else
                    {
                        synchronized = 0;
                    }
                    timerSecond = ColourNextGroup(groupedList[1]);
                }
                else
                {
                    _timer += timerSecond;
                    tempTimer = timerSecond;
                    timerSecond = ColourNextGroup(groupedList[1]);
                    if (synchronized != 3)
                    {
                        ++synchronized;
                        _timer += (timerFirst - tempTimer);
                    }
                    else
                    {
                        synchronized = 0;
                    }
                    timerFirst = ColourNextGroup(groupedList[0]);
                }
            }
            StringBuilder logStr = new StringBuilder();
            foreach (string str in _logString)
            {
                logStr.AppendLine(str);
            }
            return logStr.ToString();
        }
        private double ColourNextGroup(List<TrafficLight> group)
        {
            double temp = group[0].ColourNext();
            for (int i = 1; i < group.Count; ++i)
            {
                group[i].ColourNext();
            }
            return temp;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbFormat = new StringBuilder();
            sb.Append("Time: " + _timer + "\n");
            sbFormat.Append($"{{{0}, -{"Direction from".Length}}}");
            for (int i = 0; i < _trafficLights.Count; ++i)
            {
                sbFormat.Append($"  ||  {{{i+1}, -{TrafficLight.DIRECTION_MAX_LENGTH_STR}}}");
            }
            string format = sbFormat.ToString();
            List<string> tempList = new()
            {
                "Direction from"
            };
            tempList.AddRange(_trafficLights.Select(x => x.Direction.Item1.ToString()).ToList());
            sb.AppendFormat(format, tempList.ToArray());
            sb.Append("\n");
            tempList.Clear();
            tempList.Add("Direction to");
            tempList.AddRange(_trafficLights.Select(x => x.Direction.Item2.ToString()).ToList());
            sb.AppendFormat(format, tempList.ToArray());
            sb.Append("\n");
            tempList.Clear();
            tempList.Add("Colour");
            tempList.AddRange(_trafficLights.Select(x => x.Colour.ToString()).ToList());
            sb.AppendFormat(format, tempList.ToArray());
            sb.Append("\n");
            if (_timer == _timerAdded)
            {
                _logString.Remove(_logString.Last());                
            }
            _logString.Add(sb.ToString());
            _timerAdded = _timer;
            return sb.ToString();
        }
    }
}
