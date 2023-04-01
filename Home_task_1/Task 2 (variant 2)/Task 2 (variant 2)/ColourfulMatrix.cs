using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HT_23._03._23
{
    internal class ColourfulMatrix
    {
        private List<List<int>> _matrix; //Вирішив скористатися листами для заповнення матриці, задля того, щоб вільно використати Span згодом, і не тільки
        private int _sizeM;
        private int _sizeN;
        private int _rowIndex;
        private int _maxSize;
        private int _beginIndex;
        private int _popularElement;
        public int RowIndex
        {
            get { return _rowIndex; }
        }
        public int BeginIndex
        {
            get { return _beginIndex; }
        }
        public int MaxSize
        {
            get { return _maxSize; }
        }
        public int PopularElement
        {
            get { return _popularElement; }
        }
        public ColourfulMatrix(int sizeM = 3, int sizeN = 4)
        {
            _sizeM = sizeM;
            _sizeN = sizeN;
            _matrix = new List<List<int>>(sizeM);
            _popularElement = -1;
            _rowIndex = 0;
            _beginIndex = 0;
            _maxSize = 0;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var i in _matrix)
            {
                foreach (var j in i)
                {
                    sb.Append(j + " ");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
        public void MakeRandomPicture() //Заповнюємо випадковими числами нашу матрицю
        {
            Random random = new Random();
            for (int i = 0; i < _sizeM; ++i)
            {
                List<int> temp = new List<int>();
                for (int j = 0; j < _sizeN; ++j)
                {
                    temp.Add(random.Next(0, 17));
                }
                _matrix.Add(temp);
            }
        }
        private bool FindConsecutiveIdentical(Span<int> span) //Пошук послідовності однакових чисел в одному рядку
        {
            bool isChanged = false; //Допоможе для випадку зміни номера стрічки в матриці
            if (span.Length == 0)
            {
                return isChanged;
            }
            int currentNumber = span[0] + 1; //Робимо перший раз число, яке не буде рівне початковому (для запуску цикла коректно)
            int counter = 0;
            int index = 0;
            foreach (int element in span)
            {
                if (element == currentNumber) //Якщо число буде рівне теперішньому, то збільшуємо показники
                {
                    ++counter;
                    ++index;
                    continue;
                }
                if (_maxSize < counter) //Якщо максимальний розмір менший за каунтер - присвоюємо нові дані, інакше - нічого
                {
                    _maxSize = counter;
                    _beginIndex = index - counter;
                    _popularElement = currentNumber;
                    isChanged = true;
                }
                currentNumber = element; //Якщо не попали в перший if, то міняємо теперішнє число
                ++index;
                counter = 1;
            }
            return isChanged;
        }
        public void FindTheLargestSubsequence()
        {// затратніше по пам'яті.
            int counter = 0; //Індексація рядків матриці
            int buffer = -1; //Потрібен для того, щоб підстрічки, які йдуть до кінця стрічки масиву, запам'ятовувалися алгоритмом
            foreach (var item in _matrix)
            {
                item.Add(buffer);
                if (FindConsecutiveIdentical(item.ToArray()))
                { //Пошук для кожної стрічки. Якщо змінилося щось - запам'ятовуємо індекс рядка
                    _rowIndex = counter;
                    ++counter;
                    continue;
                }
                ++counter;
                item.Remove(buffer); //Видаємо буфер
            }
        }
    }
}
