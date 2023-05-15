using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal enum DirectionScheme : sbyte
    {
        Rectilinear = 0,
        Rotary = 1
    }
    internal enum GeographicalDirection : sbyte //напрямки за годинниковою стрілкою
                                                //(була ідея зробити два види: для чотирьох і
                                                //восьмох напрямків через інтерфейс чи абстракцію, але достатньо загального випадку)
    {
        North = 1,
        NorthEast = 2,
        East = 3,
        SouthEast = 4,
        South = 5,
        SouthWest = 6,
        West = 7,
        NorthWest = 8
    }
    internal struct Direction //Структура самого напрямку
    {
        public const uint DIRECTRION_NAME_MAX_LENGTH = 9;
        public const uint SCHEME_NAME_MAX_LENGTH = 11;
        private GeographicalDirection _from;
        private GeographicalDirection _to;
        private DirectionScheme _scheme;
        public GeographicalDirection From
        {
            get { return _from; }
        }
        public GeographicalDirection To
        {
            get { return _to; }
        }
        public DirectionScheme Scheme
        {
            get { return _scheme; }
        }
        public Direction(in GeographicalDirection from, in GeographicalDirection to, in DirectionScheme scheme)
        {
            _scheme = scheme;
            _from = from;
            sbyte halfGeoSize = (sbyte)(Enum.GetNames(typeof(GeographicalDirection)).Length / 2);
            sbyte quarterGeoSize = (sbyte)(halfGeoSize / 2);
            switch (scheme)
            {
                default:
                case DirectionScheme.Rectilinear:
                    if (Validator.CheckOppositeDirections(from, to))
                    {
                        _to = to;
                    }
                    else
                    {
                        _to = (sbyte)from <= halfGeoSize ? from + halfGeoSize : from - halfGeoSize;
                    }
                    break;
                case DirectionScheme.Rotary:
                    if (Validator.CheckPerpendicularDirections(from, to))
                    {
                        _to = to;
                    }
                    else
                    {
                        _to = (sbyte)from <= halfGeoSize + quarterGeoSize ? from + quarterGeoSize : from - quarterGeoSize - halfGeoSize;
                    }
                    break;
            }
        }
    }
}