using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal enum TrafficColour: byte
    {
        Red,
        YellowBR,
        YellowBG,
        Green
    }
    internal enum GeographicalDirection: sbyte
    {
        North = 1,
        South = -1,
        West = 2,
        East = -2,      
        NorthWest = 3,
        SouthEast = -3,
        NorthEast = 4,
        SouthWest = -4
    }
    public delegate string ColourWork();
    internal class TrafficLight: IComparable<TrafficLight>
    {
        public static int DIRECTION_MAX_LENGTH_STR { get; private set; }
        private (GeographicalDirection, GeographicalDirection) _direction;
        private TrafficColour _colour;
        public (GeographicalDirection, GeographicalDirection) Direction
        {
            get { return _direction; }
        }
        public TrafficColour Colour
        {
            get { return _colour; }
            set { _colour = value; }
        }
        public event ColourWork? ColourChanged;
        public TrafficLight(in GeographicalDirection directionFrom, in GeographicalDirection directionTo, in TrafficColour trafficColour = TrafficColour.Red)
        {
            if ((sbyte)directionFrom == -(sbyte)directionTo)
            {
                _direction = (directionFrom, directionTo);
                _colour = trafficColour;
            }
            else
            {
                _direction = (directionFrom, (GeographicalDirection)(-(sbyte)directionFrom));
                _colour = trafficColour;
            }
            if (directionFrom.ToString().Length > DIRECTION_MAX_LENGTH_STR)
            {
                DIRECTION_MAX_LENGTH_STR = directionFrom.ToString().Length;
            }
            if (directionTo.ToString().Length > DIRECTION_MAX_LENGTH_STR)
            {
                DIRECTION_MAX_LENGTH_STR = directionTo.ToString().Length;
            }           
        }
        public double ColourNext()
        {
            double result = -1;
            switch (_colour)
            {
                case TrafficColour.Red:
                    _colour = TrafficColour.YellowBG;
                    result = ColoursTimer.YellowToGreen;
                    break;
                case TrafficColour.YellowBG:
                    _colour = TrafficColour.Green;
                    result = ColoursTimer.GreenToYellow;
                    break;
                case TrafficColour.Green:
                    _colour = TrafficColour.YellowBR;
                    result = ColoursTimer.YellowToRed;
                    break;
                case TrafficColour.YellowBR:
                    _colour = TrafficColour.Red;
                    result = ColoursTimer.RedToYellow;
                    break;
                default:
                    result = 0;
                    break;
            }
            ColourChanged?.Invoke();
            return result;
        }
        public int CompareTo(TrafficLight? other)
        {
            if (other != null)
            {
                return ((sbyte)Direction.Item1).CompareTo((sbyte)other.Direction.Item1);
            }
            throw new NullReferenceException();
        }
    }
}
