using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class MatrixGenerator
    {
        private int[,] _matrix;
        private int _sizeM;
        private int _sizeN;
        public MatrixGenerator(int sizeM = 3, int sizeN = 3)
        {
            _sizeM = sizeM;
            _sizeN = sizeN;
            _matrix = new int[sizeM, sizeN];
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < _matrix.GetLength(1); ++j)
                {
                    sb.Append(_matrix[i, j] + "\t");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
        public void FormSnakeAntiClockWise() //Формування спіралі за годинниковою стрілкою
        {
            int border = 0; //Скільки елементів від краю (вже заповнені)
            int beginElement = 0; //Змінна для заповнення
            int i, j = 0;
            int substitution = _sizeM + _sizeN - 2; //Для віддзеркалення, закономірність: заповнюючи один стовпчик, заповнюємо протилежний, й так з рядками
            // Розкажете на занятті свій підхід. Добре те, що хочемо врахувати  дзеркальність.Погано, що не до кінця ще оптимізовано.
            while (substitution >= 0 && beginElement + substitution < _matrix.Length)
            {
                for (i = border; i < _sizeM - border; ++i)
                {
                    ++beginElement;
                    _matrix[i, j] = beginElement;
                    if (beginElement + substitution > _matrix.Length) //Обмеження, щоб число не перевищувало m * n
                    {
                        continue;
                    }
                    _matrix[_sizeM - i - 1, _sizeN - j - 1] = _matrix[i, j] + substitution; //Віддзеркалення елементу (рухаючись по стовпчику)                  
                }
                --i;
                for (j = border + 1; j < _sizeN - border - 1; ++j)
                {
                    ++beginElement;
                    _matrix[i, j] = beginElement;
                    if (beginElement + substitution > _matrix.Length) //Аналогічне обмеження
                    {
                        continue;
                    }
                    _matrix[_sizeM - i - 1, _sizeN - j - 1] = _matrix[i, j] + substitution; //Віддзеркалення елементу (рухаючись по рядочку)                                  
                }
                --j;
                beginElement = _matrix[i, j] + substitution; //Бегін тепер останній віддзеркаленний елемент
                ++border;
                j = border;

                substitution -= 4; //Різниця між протилежними елементами спадає стало на 4
            }
        }
        public void FormSnakeClockWise()
        {// Dи транспонуванням міняєте розміри матриці, а від Ва просять вміти для незмінних розмірів за іншим принципом закрутити спіраль
            FormSnakeAntiClockWise(); //Формуємо за годинниковою стрілкою
            int[,] matrixTemp = new int[_sizeN, _sizeM]; //Транспонування матриці
            for (int i = 0; i < _sizeM; ++i)
            {
                for (int j = 0; j < _sizeN; ++j)
                {
                    matrixTemp[j, i] = _matrix[i, j];
                }
            }
            _matrix = new int[_sizeN, _sizeM];
            Array.Copy(matrixTemp, _matrix, _matrix.Length); //Вставляємо транспоновану матрицю
            int temp = _sizeN; //Свапаємо розміри після транспонування
            _sizeN = _sizeM;
            _sizeM = temp;
        }
    }
}
