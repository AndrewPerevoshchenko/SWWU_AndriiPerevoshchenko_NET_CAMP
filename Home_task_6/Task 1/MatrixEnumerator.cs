using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal struct Position //Щось на кшталт індексатора для матриці
    {
        private int _row;
        private int _column;
        public int Row { get { return _row; } set { _row = value > 0 ? value : 0; } }
        public int Column { get { return _column; } set { _column = value > 0 ? value : 0; } }
        public Position(int row = 0, int column = 0)
        {
            _row = row;
            _column = column;
        }
    }
    internal class MatrixEnumerator<T>: IEnumerator //Розробили власний ітератор
    {
        private uint _size;
        private T[,] _matrix;
        Position position = new Position(-1, -1);
        public MatrixEnumerator(in T[,] matrix)
        {
            _matrix = matrix; //Не копіюємо, а саме передаємо посилання
            _size = (uint)_matrix.GetLength(0);
        }
        public bool MoveNext() //Рухаємося до наступного елемента ітеративно
        {
            if ((position.Row + position.Column) % 2 == 0) //Якщо сума номерів рядка та стовпця - парна, то це вітка руху вниз по діагоналі
            {
                if (position.Row == _size - 1) //Якщо ми на останньому рядочку, то рух вправо, а не вниз
                {
                    ++position.Column;
                }
                else if (position.Column == 0) //Або якщо дійшли до першого стовпчика, то рух просто вниз
                {
                    ++position.Row;
                }
                else //Інакше, вниз діагонально
                {
                    ++position.Row;
                    --position.Column;
                }
            }
            else //Аналогія для непарності, тільки тут вітка руху вгору
            {
                if (position.Column == _size - 1) //Якщо остання колонка, то вниз
                {
                    ++position.Row;
                }
                else if (position.Row == 0) //Якщо дійшли до верху, то праворуч
                {
                    ++position.Column;
                }
                else //А так рухаємося діагонально вгору
                {
                    ++position.Column;
                    --position.Row;
                }
            }
            return (position.Row < _size) && (position.Column < _size); //Якщо вийдемо за межі колекції, то false
        }

        public void Reset() //Ресет значення
        {
            position = new Position(-1, -1);
        }

        object IEnumerator.Current //Карент елемент або дефолт для Т
        {
            get
            {
                return Current?? default(T);
            }
        }

        public T Current //Власне, дістаємо карент, якщо ітератор в межах колекції
        {
            get
            {
                return (position.Row < _size) && (position.Column < _size) ? _matrix[position.Row, position.Column] : throw new IndexOutOfRangeException("Index out of range");
            }
        }
    }
}
