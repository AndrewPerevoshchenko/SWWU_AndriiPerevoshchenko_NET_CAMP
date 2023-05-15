using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal enum TrafficColour : byte
    {
        Red,
        YellowBR,//жовтий перед червоним
        YellowBG,//жовтий перед зеленим
        Green
    }
    internal class TrafficLight : IComparable<TrafficLight>, ICloneable
    //Світлофор з поворотом у мене це не щось окреме, дочірнє чи ще якесь - це такий самий світлофор, який може містити три кольори, але просто має інший тип напрямку
    {
        private Direction _direction;
        private TrafficColour _colour;
        public Direction Direction
        {
            get { return _direction; }
        }
        public TrafficColour Colour
        {
            get { return _colour; }
            set { _colour = value; }
        }
        public TrafficLight(in GeographicalDirection directionFrom, in GeographicalDirection directionTo, in DirectionScheme directionType, in TrafficColour trafficColour = TrafficColour.Red)
        {
            _direction = new Direction(directionFrom, directionTo, directionType);
            _colour = trafficColour;
        }
        public void ColourNext()
        {
            switch (_colour)
            {
                case TrafficColour.Red:
                    _colour = TrafficColour.YellowBG;
                    break;
                case TrafficColour.YellowBG:
                    _colour = TrafficColour.Green;
                    break;
                case TrafficColour.Green:
                    _colour = TrafficColour.YellowBR;
                    break;
                case TrafficColour.YellowBR:
                    _colour = TrafficColour.Red;
                    break;
                default:
                    break;
            }
        }
        public int CompareTo(TrafficLight? other)
        {
            if (other != null)
            {
                return ((sbyte)Direction.From).CompareTo((sbyte)other.Direction.From);
            }
            throw new NullReferenceException();
        }

        public object Clone()
        {
            TrafficLight copy = new TrafficLight(Direction.From, Direction.To, Direction.Scheme, Colour);
            return copy;
        }
    }
}