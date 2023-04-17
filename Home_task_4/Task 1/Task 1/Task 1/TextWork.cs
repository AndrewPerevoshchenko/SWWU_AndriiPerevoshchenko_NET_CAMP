using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    public struct Coordinates
    {
        private int _row;
        private int _symbol;
        public int Row { get { return _row; } }
        public int Symbol { get { return _symbol; } }
        public Coordinates(int row = 0, int symbol = 0)
        {
            _row = row;
            _symbol = symbol;
        }
        public static bool operator >(Coordinates a, Coordinates b)
        {
            if (a.Row == b.Row) 
            {
                return a.Symbol > b.Symbol;
            }
            return a.Row > b.Row;
        }
        public static bool operator <(Coordinates a, Coordinates b)
        {
            if (a.Row == b.Row)
            {
                return a.Symbol < b.Symbol;
            }
            return a.Row < b.Row;
        }
    }
    internal class TextWork
    {
        private readonly List<string> _text;
        
        public TextWork(string fileURL)
        {
            _text = new List<string>();
            if (fileURL != null)
            {
                _text.AddRange(System.IO.File.ReadAllLines(fileURL));
            }
        }
        private static List<int> IndexOfSymbols(string str, params char[] symbols) //IndexOf() для декількох символів
        {
            List<int> indexes = new List<int>();
            foreach(char c in symbols)
            {
                int temp = str.IndexOf(c);
                while(temp != -1)
                {
                    indexes.Add(temp);
                    temp = str.IndexOf(c, temp + 1);
                }
            }
            indexes.Sort(); //Посортовані індекси оптимізаційніші
            return indexes;
        }
        private void ChangeBegin(ref Coordinates begin, in Coordinates end) //Функція для пересування початку речення
        {
            if (end.Symbol == _text[end.Row].Length - 1) //Якщо розділювач речень - не останній символ стрічки
            {
                begin = new Coordinates(end.Row + 1, 0);
            }
            else //Інакше початок нового речення в новій стрічці
            {
                begin = new Coordinates(end.Row, end.Symbol + 1);
            }
        }
        private string SubRow(ref Coordinates begin, in Coordinates end) //Substring() для шматків речення в декількох стрічках
        {            
            StringBuilder sb = new StringBuilder();
            if (begin.Row == end.Row) //Якщо в одній стрічці, достатньо звичайного Substring()
            {
                sb.Append(_text[begin.Row].Substring(begin.Symbol, end.Symbol - begin.Symbol + 1));
                ChangeBegin(ref begin, end);
                return sb.ToString();
            }
            sb.Append(_text[begin.Row].Substring(begin.Symbol) + "\n"); //Шматок у першій стрічці (до кінця)
            for(int i = begin.Row + 1; i < end.Row; ++i)
            {
                sb.Append(_text[i] + "\n"); //Проміжні стрічки
            }
            sb.Append(_text[end.Row].Substring(0, end.Symbol + 1)); //Шматок в останній стрічці до розділювача
            ChangeBegin(ref begin, end);
            return sb.ToString();
        }
        public List<string> FindBracketSentences() //Основний метод пошуку дужок
        {
            _text.Add("buff"); //Додамо буфер задля того, щоб не виникло проблем з ChangeBegin()
            List<string> result = new List<string>();
            Coordinates sentenceBegin = new Coordinates();
            bool hasPrevious = false; //Коли дужки були в стрічці, але розділювач десь у наступних стрічках
            for(int i = 0; i < _text.Count - 1; ++i)
            {
                List<int> endIndexes = IndexOfSymbols(_text[i], '.', '!', '?'); //Індекси розділювачів
                List<int> brackets = IndexOfSymbols(_text[i], '(', '{', '['); //Індекси відкритих дужок (тільки їх достатньо, адже закриті за умовою розставлені правильно)
                if (endIndexes.Count == 0) //Якщо не знайшли розділювача
                {
                    if (!hasPrevious && brackets.Count > 0) //Є дужка, але не знаходили раніше - записали
                    {
                        hasPrevious = true;
                    }
                }
                else
                {
                    if (hasPrevious) //Якщо знайшли раніше, то до першого розділювача виокремили речення
                    {
                        hasPrevious = false;
                        result.Add(SubRow(ref sentenceBegin, new Coordinates(i, endIndexes[0])));
                    }
                    if (brackets.Count > 0) //Якщо дужки були знайдені, то
                    {                        
                        if (brackets[0] < endIndexes[0]) //Перевіряємо чи вони є до першого розділювача
                        {
                            result.Add(SubRow(ref sentenceBegin, new Coordinates(i, endIndexes[0])));
                        }
                        else //Інакше, пересуваємо початок
                        {
                            ChangeBegin(ref sentenceBegin, new Coordinates(i, endIndexes[0]));
                        }
                        for (int j = 1; j < endIndexes.Count; ++j) //Перевіряємо всі розділювачі
                        {
                            int br = Array.Find(brackets.ToArray(), n => n > sentenceBegin.Symbol && n < endIndexes[j]); //Пошук дужок до розділювача
                            if (br > 0) //Якщо є - записали
                            {
                                result.Add(SubRow(ref sentenceBegin, new Coordinates(i, endIndexes[j])));
                            }
                            else //Ні - сунемо початок
                            {
                                ChangeBegin(ref sentenceBegin, new Coordinates(i, endIndexes[j]));
                            }
                        }
                        if (sentenceBegin.Row == i) //Якщо початок речення є в цій стрічці, а розділювача вже не буде, то перевіримо на наявність дужок у хвостику
                        {
                            hasPrevious = Array.Find(brackets.ToArray(), n => n > sentenceBegin.Symbol) > 0;
                        }
                    }
                    else //Якщо дужок не було, то початок буде після останнього розділювача
                    {
                        ChangeBegin(ref sentenceBegin, new Coordinates(i, endIndexes.Last()));
                    }
                }
            }
            _text.RemoveAt(_text.Count - 1); //Забираємо буфер
            return result;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in _text)
            {
                sb.AppendLine(s);
            }
            return sb.ToString();
        }
    }
}
