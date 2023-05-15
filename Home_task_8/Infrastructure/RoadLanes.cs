using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_7
{
    internal class RoadLanes: ICloneable, IEquatable<RoadLanes>
    {
        //Лівий і правий поворотні світлофори: чи вони існують (для лівого необхідний ще індекс)
        private (bool, uint) _hasLeftRotary;
        private bool _hasRightRotary;
        //Напрямок "звідки" для всіх смуг у групі однаковий
        private GeographicalDirection _part;
        //Світлофор із своїм таймером і номером смуги
        private Dictionary<TrafficLight, (ColoursTimer, uint)> _lanes;
        public GeographicalDirection Part
        {
            get { return _part; }
        }
        public Dictionary<TrafficLight, (ColoursTimer, uint)> Lanes
        {
            get { return new Dictionary<TrafficLight, (ColoursTimer, uint)>(_lanes); }
        }
        public RoadLanes(GeographicalDirection part)
        {
            _hasLeftRotary = (false, 0);
            _hasRightRotary = false;
            _part = part;
            _lanes = new();
        }
        public void AddLane(TrafficLight trafficLight, ColoursTimer timer, uint lane, bool possibleMoveLeft = true)
        //Додати нову смугу
        {
            if (trafficLight != null)
            {
                if (trafficLight.Direction.From == _part)
                {
                    uint[] lanesNumber = _lanes.Select(t => t.Value.Item2).ToArray();
                    //Перевірити: чи все гаразд з поворотами та смугою
                    if (Validator.CheckTrafficLightOnLane(lanesNumber, trafficLight, lane, ref _hasLeftRotary, ref _hasRightRotary))
                    {
                        _lanes.Add(trafficLight, (timer, lane));
                        //Якщо додали смугу лівіше ніж існуючий лівий поворот, і при цьому не можна посунути лівий поворот, то помилка
                        if (lane > _hasLeftRotary.Item2 && _hasLeftRotary.Item1 && !possibleMoveLeft)
                        {
                            throw new InvalidDataException("There is the problem with left rotary (you have added new line left)");
                        }
                        return;
                    }
                    //Якщо схема прямолінійна, й при цьому немає лівого повороту (або є, але можна посунути), то додамо нову смугу крайню зліва
                    if (trafficLight.Direction.Scheme == DirectionScheme.Rectilinear && (!_hasLeftRotary.Item1 || possibleMoveLeft))
                    {
                        if (_hasLeftRotary.Item1)
                        {
                            //Посуваємо лівий поворот на новий край
                            var tempPair = _lanes.Select(t => t).Where(t => t.Value.Item2 == _hasLeftRotary.Item2).Last();
                            _lanes[tempPair.Key] = (tempPair.Value.Item1, (uint)_lanes.Count);
                            _hasLeftRotary.Item2 = (uint)_lanes.Count;                         
                        }
                        _lanes.Add(trafficLight, (timer, (uint)_lanes.Count));
                        return;
                    }
                    throw new InvalidDataException("Incorrect number of lane for rotary light or you have added new line after left rotary");
                }
                throw new ArgumentException("Incorrect direction \"from\" in traffic light you want to add");
            }
            throw new NullReferenceException();
        }
        public TrafficLight this[int index] //Передаємо посилання на елемент за ключем (сам світлофор)
        {
            get { return _lanes.ElementAt(index).Key; }
        }
        public object Clone()
        {
            RoadLanes copy = new RoadLanes(_part);
            copy._hasLeftRotary = _hasLeftRotary;
            copy._hasRightRotary= _hasRightRotary;
            copy._lanes = new Dictionary<TrafficLight, (ColoursTimer, uint)>(_lanes);
            return copy;
        }
        public override int GetHashCode()
        {
            return _part.GetHashCode();
        }
        public bool Equals(RoadLanes? other) //Унікальність за напрямком "від" для групи
        {
            return other != null ? _part.Equals(other._part) : false;
        }
        public override string ToString()
        {
            SortLanes();
            int maxLaneLength = _lanes.Select(t => t.Value.Item2).Max().ToString().Length;
            StringBuilder sb = new();
            StringBuilder sbForm = new();
            sb.Append("Direction: " + _part.ToString() + "\n");
            sbForm.Append($"{{{0},-{maxLaneLength}}} || " +
                $"{{{1},-{Direction.DIRECTRION_NAME_MAX_LENGTH}}} || " +
                $"{{{2},-{Direction.DIRECTRION_NAME_MAX_LENGTH}}} || " +
                $"{{{3},-{Direction.SCHEME_NAME_MAX_LENGTH}}} || " +
                $"{{{4},-{ColoursTimer.COLOUR_NAME_MAX_LENGTH}}}");
            string format = sbForm.ToString();
            foreach(var item in _lanes)
            {
                sb.AppendFormat(format, item.Value.Item2, item.Key.Direction.From, item.Key.Direction.To, item.Key.Direction.Scheme, item.Key.Colour);
                sb.Append("\n");
            }
            return sb.ToString();
        }
        private void SortLanes() //Посортувати за номером смуги (для візуалу)
        {
            _lanes.OrderBy(n => n.Value.Item2);
        }
    }
}