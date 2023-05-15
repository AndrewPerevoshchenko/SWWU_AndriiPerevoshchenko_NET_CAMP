using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Reflection;
namespace Home_task_7
{
    internal struct ColoursTimer
    {
        public const double MIN_TIMER = 1;
        public const int COLOURS_CHANGING_AMOUNT = 4;
        public const uint COLOUR_NAME_MAX_LENGTH = 8;
        private double R_Y;
        private double Y_G;
        private double G_Y;
        private double Y_R;
        public double RedToYellow
        {
            get { return R_Y; }
            set { R_Y = value > MIN_TIMER ? value : MIN_TIMER; }
        }
        public double YellowToGreen
        {
            get { return Y_G; }
            set { Y_G = value > MIN_TIMER ? value : MIN_TIMER; }
        }
        public double GreenToYellow
        {
            get { return G_Y; }
            set { G_Y = value > MIN_TIMER ? value : MIN_TIMER; }
        }
        public double YellowToRed
        {
            get { return Y_R; }
            set { Y_R = value > MIN_TIMER ? value : MIN_TIMER; }
        }
        public ColoursTimer(double red_yellow, double yellow_green, double green_yellow, double yellow_red)
        {
            RedToYellow = red_yellow;
            YellowToGreen = yellow_green;
            GreenToYellow = green_yellow;
            YellowToRed = yellow_red;
        }
        public List<double> findTimingPeriodicity(bool startsFromRed = true)
        //Таймери кожного світлофора в періоді (наступний дорівнює сумі попередніх плюс дане число)
        {
            List<double> result = new List<double>(COLOURS_CHANGING_AMOUNT);
            if (startsFromRed)
            {
                result.Add(R_Y);
                result.Add(result.Last() + Y_G);
                result.Add(result.Last() + G_Y);
                result.Add(result.Last() + Y_R);
            }
            else
            {
                result.Add(G_Y);
                result.Add(result.Last() + Y_R);
                result.Add(result.Last() + R_Y);
                result.Add(result.Last() + Y_G);
            }
            return result;
        }
    }
}