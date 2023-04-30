using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class QuadroMatrix<T>: IEnumerable //Клас з матрицею
    {
        private uint _size;
        private T[,] _matrix;
        public uint Size
        {
            get { return _size; }
        }
        public T[,] Matrix 
        { 
            get
            {
                T[,] matrixCopy = new T[_size, _size];
                if (_matrix != null)
                {
                    for (int i = 0; i < _size; ++i)
                    {
                        for (int j = 0; j < _size; ++j)
                        {
                            matrixCopy[i, j] = _matrix[i, j];
                        }
                    }
                }
                return matrixCopy;
            } 
        }
        public QuadroMatrix(string fileWay)
        {
            if (File.Exists(fileWay))
            {
                string[] temp = File.ReadAllLines(fileWay);
                if (temp[0].Length > 0)
                {
                    uint.TryParse(temp[0], out _size);
                    _matrix = new T[_size, _size];
                    if (temp.Length >= _size + 1)
                    {
                        for(int i = 1; i <= _size; ++i)
                        {
                            string[] tempRow = temp[i].Split(' ');
                            if (tempRow.Length < _size)
                            {
                                throw new InvalidDataException("Incorrect amount of the elements in the row");
                            }
                            for (int j = 0; j < _size; ++j)
                            {
                                try
                                {
                                    _matrix[i - 1,j] = (T)Convert.ChangeType(tempRow[j], typeof(T));
                                }
                                catch
                                {
                                    throw new InvalidDataException("Incorrect value of the element");
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("Incorrect amount of rows");
                    }
                }
                else
                {
                    _size = 0;
                    _matrix = new T[_size, _size];
                    throw new InvalidDataException("Incorrect size of the array");
                }
            }
            else
            {
                throw new FileNotFoundException($"File with address {fileWay} was not found");
            }
        }
        public IEnumerator GetEnumerator() //Дістаємо ітератор
        {
            return new MatrixEnumerator<T>(_matrix);
        }
        public override string ToString() //Повний друк за принципом спіралі, використовуючи лише ітератор та MoveNext()
        {
            StringBuilder sb = new StringBuilder();
            IEnumerator enumerator = new MatrixEnumerator<T>(_matrix);
            while (enumerator.MoveNext())
            {
                sb.Append(enumerator.Current.ToString() + " ");
            }
            return sb.ToString();
        }
    }
}
