using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    public struct Point
    {
        private const string DOUBLE_FORMAT = "F2";
        private double _x;
        private double _y;
        public double X
        {
            get { return _x; }
        }
        public double Y
        {
            get { return _y; }
        }
        public Point(double x = 0, double y = 0)
        {
            _x = x;
            _y = y;
        }
        public override string ToString()
        {
            return $"({_x.ToString(DOUBLE_FORMAT)}; {_y.ToString(DOUBLE_FORMAT)})";
        }
    }
    internal class Garden
    {// Цю константу можете змінити тільки Ви, отже, якщо клас зробите бібліотечним, то цей параметр буде не гнучким.
        private const int RANDOM_STANDART = 10;
        private uint _treesAmount;// можна обійтись...
        private List<Point> _trees;
        private double _fenceLength;
        public double FenceLength
        {
            get { return _fenceLength; }
        }
        public Garden(string fileURL = "")
        {
            _fenceLength = 0;
            if (File.Exists(fileURL))
            {               
                string[] temp = File.ReadAllLines(fileURL);
                uint.TryParse(temp[0], out _treesAmount);
                _trees = new List<Point>((int)_treesAmount);
                if (temp.Length < _treesAmount + 1)
                {
                    _treesAmount = 0;
                    return;
                }
                for (int i = 1; i <= _treesAmount; ++i) 
                {
                    string[] tempPoint = temp[i].Split(' ');
                    _trees.Add(new Point(double.Parse(tempPoint[0]), double.Parse(tempPoint[1])));
                }              
            }
            else
            {
                _trees = new List<Point>();
                MakeRandomTrees(RANDOM_STANDART);
            }
        }
        private void MakeRandomTrees(int diapasone)
        {
            Random random = new Random();
            _treesAmount = (uint)random.Next(3, diapasone + 1);
            for (int i = 0; i < _treesAmount; ++i)
            {
                _trees.Add(new Point(
                    ((double)random.Next(0, diapasone + 1) / random.Next(1, diapasone + 1)),
                    ((double)random.Next(0, diapasone + 1) / random.Next(1, diapasone + 1))));
            }
        }
        private sbyte CheckOrientation(int statedIndex, int checkingIndex, int anyAnotherIndex) //Перевірка кута між доданим деревом у контур, можливим і будь-яким іншим
        {
            double result = (_trees[checkingIndex].Y - _trees[statedIndex].Y) * //Псевдоскалярний добуток, визначення "Проти" чи "За" годинниковою стрілкою
                (_trees[anyAnotherIndex].X - _trees[checkingIndex].X) -
                (_trees[checkingIndex].X - _trees[statedIndex].X) * 
                (_trees[anyAnotherIndex].Y - _trees[checkingIndex].Y);
            if (result == 0)
            {
                return 0;
            }
            return result > 0 ? (sbyte)1 : (sbyte)-1; //Якщо мінус одиниця, то далі беремо в оберт перевірки
        }
        public void FindShortestFence()
        {
            if (_treesAmount >= 3)
            {
                int statedIndex, checkingIndex;
                List<Point> fenceTrees = new List<Point>();
                int minIndex = (int)_treesAmount - 1;
                for(int i = 0; i < _treesAmount - 1; ++i) //Пошук найлівішої точки по Х// може бути будь-яка з кутових
                {
                    if (_trees[i].X < _trees[minIndex].X)
                    {
                        minIndex = i;
                    }
                }
                statedIndex = minIndex;
                do
                {
                    fenceTrees.Add(_trees[statedIndex]);
                    checkingIndex = (statedIndex + 1) % (int)_treesAmount;
                    for (int i = 0; i < _treesAmount; ++i)
                    {
                        if (CheckOrientation(statedIndex, i, checkingIndex) == -1)
                        {
                            checkingIndex = i;
                        }
                    } //На виході матимемо точку контуру
                    _fenceLength += Math.Sqrt((_trees[checkingIndex].X - _trees[statedIndex].X) *
                        (_trees[checkingIndex].X - _trees[statedIndex].X) +
                        (_trees[checkingIndex].Y - _trees[statedIndex].Y) *
                        (_trees[checkingIndex].Y - _trees[statedIndex].Y)); //Додаємо відстань між попередньою й новою точкою контуру
                    statedIndex = checkingIndex;
                } while (statedIndex != minIndex); //Поки не замкнемо контур
            }
        }
        public static bool operator ==(in Garden left, in Garden right)
        {
            return left._fenceLength == right._fenceLength;
        }
        public static bool operator !=(in Garden left, in Garden right)
        {
            return !(left == right);
        }
        public static bool operator >(in Garden left, in Garden right)
        {
            return left._fenceLength > right._fenceLength;
        }
        public static bool operator <(in Garden left, in Garden right)
        {
            return left._fenceLength < right._fenceLength;
        }
        public static bool operator >=(in Garden left, in Garden right)
        {
            return !(left < right);
        }
        public static bool operator <=(in Garden left, in Garden right)
        {
            return !(left > right);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_treesAmount + " " + _fenceLength + "\n");
            foreach(Point p in _trees)
            {
                sb.Append(p.ToString() + "\n");
            }
            return sb.ToString();
        }
    }
}
