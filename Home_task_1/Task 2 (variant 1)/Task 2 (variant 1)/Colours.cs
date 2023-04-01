using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2_variant_1
{
    internal class Colours
    {
        private byte[,] _pixels;
        private int _sizeM;
        private int _sizeN;
        // Краще всі ці величини повертати як параметри методів.
        private int _rowIndex;
        private int _beginIndex;
        private int _maxSize;
        private int _element;
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
        public int Element
        {
            get { return _element; }
        }
        public Colours(int sizeM = 3, int sizeN = 4)
        {
            _sizeM = sizeM;
            _sizeN = sizeN;
            _pixels = new byte[sizeM, sizeN];
            _rowIndex = 0;
            _beginIndex = 0;
            _maxSize = 1;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _pixels.GetLength(0); ++i)
            {
                for (int j = 0; j < _pixels.GetLength(1); ++j)
                {
                    sb.Append(_pixels[i, j] + "\t");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
        public void MakeRandomPicture() //Заповнюємо випадковими елементами від 0 до 16
        {
            Random random = new Random();
            for (int i = 0; i < _sizeM; ++i)
            {
                for (int j = 0; j < _sizeN; ++j)
                {// Цієї чарівної константи нам тут не треба...
                    _pixels[i, j] = Convert.ToByte(random.Next(0, 17));
                }
            }
        }
        public void FindPopulars() //Пошук найдовшої послідовності однакових чисел
            //Алгоритм цікавий. Розкажіть про нього на занятті.Він не завди кращий від стандартного...
        {
            int middle = _sizeN / 2 + 1; //Шукаємо середину для окремих випадків
            for (int i = 0; i < _sizeM; ++i)
            {
                if (_maxSize >= middle) //Якщо розмір найбільшої послідовності більший за половину (для парних - виходить за середину, непарні - на середині)
                {
                    if (_pixels[i, middle] != _pixels[i, middle - 1] || _pixels[i, middle] != _pixels[i, middle + 1]) //Достатньо перевірити: чи від центрального сусідні є такими самими (інакше - більшої підпослідовності немає точно)
                    {
                        continue;
                    }
                }
                else if (_maxSize == _sizeN) //Якщо довжина послідовності - вся стрічка, то далі можна не робити
                {
                    break;
                }
                for (int j = 0; j < _sizeN - _maxSize; ++j)
                {
                    if (_pixels[i, j] == _pixels[i, j + _maxSize]) //Якщо елемент попереду на maxSize від даного такий самий, то треба перевірити елементи між ними
                    {
                        int element = _pixels[i, j];
                        bool isSequence = true;
                        for (int k = j + _maxSize - 1; k > j; --k) //Сама перевірка між ними
                        {
                            if (_pixels[i, k] != element) //Між ними знайдено інший елемент - значить з нього й починатимемо перевірку далі, виходимо
                            {
                                isSequence = false;
                                j = k;
                                break;
                            }
                        }
                        if (isSequence) //Якщо цикл пройшов успішно й між ними елементи такі самі - це вже нова максимальна підстрічка
                        {
                            _element = element;
                            _rowIndex = i;
                            _beginIndex = j;
                            ++_maxSize;
                            j += _maxSize;
                            while (j < _sizeN) //Щоб j не вийшов за межі стрічки
                            {
                                if (_pixels[i, j] == element) //Можливо наша підстрічка продовжується й після елемента j + MaxSize
                                {
                                    ++_maxSize;
                                    ++j;
                                    continue;
                                }
                                break; //Робимо доки не зустрінемо інший елемент    
                            }
                            --j;
                        }
                    }
                }
            }
        }
    }
}
