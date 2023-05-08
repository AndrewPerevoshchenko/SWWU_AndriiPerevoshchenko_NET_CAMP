using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Home_task_7
{
    //Ідея доволі проста: час перемикання між кольорами світлофорів у даному випадку буде єдиним для всіх світлофорів. З жовтого на червоний та з жовтого на зелений - час може відрізнятися
    internal struct ColoursTimer //Час перемикання між кольорами світлофорів
    {
        public const double MIN_TIMER = 1;
        private static double R_Y; //З червоного на жовтий
        private static double Y_G; //З жовтого на зелений
        private static double G_Y; //Із зеленого на жовтий
        private static double Y_R; //З жовтого на червоний
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
        private List<string> _logString; //Результуючі стрічки   
        private List<TrafficLight> _trafficLights; //Світлофори (набір)
        private ColoursTimer _coloursTimer; //Таймер світла (один єдиний у даному випадку). Якщо розширювати задачу, то 
        private double _timerAdded; //Останній таймінг, який був доданий (для лог-стрічки потрібне саме поле, а не змінна, що повертатиметься функцією).
        public ColoursTimer ColoursTimer
        {
            get { return _coloursTimer; }
        }
        public double _timer; //Власне, таймер
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
        public string StartSchemeTOS(double timerCeil) //Схема, що дозволяє регулювати дві групи світлофорів (протилежні - синхронні)
        {
            var groupedList = Validator.GroupByDirection(_trafficLights); //Формування двох груп світлофорів
            if (!Validator.CheckOppositeSynchonization()) throw new Exception("Opposite traffic lights are not synchronized!"); //Чи синхронізовані протилежні
            if (groupedList.Count != 2) throw new InvalidDataException("Incorrect amount of the groups");
            foreach(var trafficLight in groupedList[0]) //Нехай світлофори першої групи будуть мати до включення жовтий колір, який йде перед зеленим
            {
                trafficLight.Colour = TrafficColour.YellowBG;
            }
            foreach(var trafficLight in groupedList[1]) //А другої групи - перед червоним
            {
                trafficLight.Colour = TrafficColour.YellowBR;
            }
            groupedList[0][0].ColourChanged += ToString; //Підписуємося на івенти (хард-кодом, але тут цикли оптимально)
            groupedList[0][1].ColourChanged += ToString;
            groupedList[1][0].ColourChanged += ToString;
            groupedList[1][1].ColourChanged += ToString;
            double timerFirst = 0;
            double timerSecond = 0;
            ToString(); //Викликаємо перший ToString() вручну
            timerFirst = ColourNextGroup(groupedList[0]); //Час, що буде отриманий від переключання з наступного кольору на після наступного
            timerSecond = ColourNextGroup(groupedList[1]); //Аналогічно
            double tempTimer = 0;
            bool synchronized = false; //Змінна, що відповідає за потребу додавання різниці таймінгів (жовтий може включатися по-різному, а от зелений і червоний будуть синхронізовані в даній схемі)
            while (_timer < timerCeil) 
            {               
                if (timerFirst <= timerSecond)
                {
                    _timer += timerFirst; 
                    if (_timer > timerCeil) break;
                    tempTimer = timerFirst; //Запам'ятовуємо таймінг, який використовуватиметься в наступній стрічці
                    timerFirst = ColourNextGroup(groupedList[0]); //Майбутній таймінг
                    if (!synchronized) //Якщо не дійшли синхронізації, то треба різницю таймінгів додати
                    {
                        synchronized = true;
                        _timer += (timerSecond - tempTimer);
                    }
                    else
                    {
                        synchronized = false; //Інакше - вимикаємо синхронізацію, адже цей етап уже пройшли
                    }
                    if (_timer > timerCeil) break;
                    timerSecond = ColourNextGroup(groupedList[1]); //Новий таймінг для другої групи
                }
                else //Усе аналогічно
                {
                    _timer += timerSecond;
                    if (_timer > timerCeil) break;
                    tempTimer = timerSecond;
                    timerSecond = ColourNextGroup(groupedList[1]);
                    if (!synchronized)
                    {
                        synchronized = true;
                        _timer += (timerFirst - tempTimer);
                    }
                    else
                    {
                        synchronized = false;
                    }
                    if (_timer > timerCeil) break;
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
        private double ColourNextGroup(List<TrafficLight> group) //Зміна кольорів для всієї групи
        {
            double temp = group[0].ColourNext(); //Змінна, щоб повернути час наступного перемикання
            for (int i = 1; i < group.Count; ++i)
            {
                group[i].ColourNext();
            }
            return temp;
        }
        public override string ToString() //Функція, яка викликається подією в тому числі
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbFormat = new StringBuilder();
            sb.Append("Time: " + _timer + "\n");
            sbFormat.Append($"{{{0}, -{"Direction from".Length}}}"); //Для форматування
            for (int i = 0; i < _trafficLights.Count; ++i)
            {
                sbFormat.Append($"  ||  {{{i+1}, -{TrafficLight.DIRECTION_MAX_LENGTH_STR + 3}}}"); //Також для форматування (константу 3 треба було винести)
            }
            string format = sbFormat.ToString(); //Сам формат
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
            if (_timer == _timerAdded) //Якщо новий запис стосується того самого таймінгу, то старий видалити
            {
                _logString.Remove(_logString.Last());                
            }
            _logString.Add(sb.ToString());
            _timerAdded = _timer;
            return sb.ToString();
        }
    }
}
