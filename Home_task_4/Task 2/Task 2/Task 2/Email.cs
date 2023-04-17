using Microsoft.Win32.SafeHandles;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal class Email
    {
        private const uint LOCAL_LENGTH = 64;
        private const uint DOMAIN_LENGTH = 255;
        private string _localPart; 
        private string _domain; 
        private bool _isEmail = true;
        public bool IsEmail
        {
            get { return _isEmail; }
        }
        public Email(string localPart, string domain)
        {
            _localPart = localPart;
            _domain = domain;
        }
        public void CheckEmail()
        {
            int function = 0; 
            string commonLocal = "";
            string commonDomain = "";
            while (_isEmail == true)
            {
                switch (function)
                {
                    case 0:
                        _isEmail = CheckSize(); //Перевірка на правильність розміру
                        break;
                    case 1:
                        _isEmail = CheckLocalBackSlash(); //Перевірка на наявність слешів у локальній частині
                        break;
                    case 2:
                        _isEmail = CheckDomainMinus(); //Перевірка на дефіси в домені
                        break;
                    case 3:
                        (bool, string) commentTemp = CheckComments(_localPart); //Локальна частина без коментарів (далі цю частину перевірятимемо)
                        commonLocal = commentTemp.Item2;
                        bool first = commentTemp.Item1;
                        commentTemp = CheckComments(_domain); //Доменна частина без коментарів (далі цю частину перевірятимемо)
                        commonDomain = commentTemp.Item2;
                        if (!first || !commentTemp.Item1 || commonLocal.Length == 0 || commonDomain.Length == 0)
                        {
                            _isEmail = false;
                        }
                        break;
                    case 4:
                        (bool, string) quoteTemp = RemoveQuotes(commonLocal); //Локальна частина без кометарів і лапок
                        _isEmail = quoteTemp.Item1;
                        commonLocal = quoteTemp.Item2;
                        break;
                    case 5:
                        _isEmail = CheckPoint(commonDomain); //Перевірка крапок у домені
                        break;
                    case 6:
                        _isEmail = CheckDomenSymbols(commonDomain); //Перевірка символів у домені
                        break;
                    case 7:
                        if (commonLocal.Length == 0)
                        {
                            return;
                        }
                        _isEmail = CheckAt(commonLocal); //Перевірка локальної частини (якщо вона поза лапками була) на зайві собачки
                        break;
                    case 8:
                        _isEmail = CheckPoint(commonLocal); //Перевірка локальної частини на крапки
                        break;
                    case 9:
                        _isEmail = !CheckDomenSymbols(commonLocal) ? CheckSpecialSymbol(commonLocal) : true; 
                        //Перевірка локальної частини на символи. Якщо знайшлися окрім звичайних, то перевіряємо спеціальні
                        break;
                    case 10:
                        return;
                }
                ++function;
            }
        }
        private (bool, string) CheckComments(string part)
        {
            List<int> openIndexes = new List<int>();
            int temp = part.IndexOf('(');
            while(temp != -1)
            {
                openIndexes.Add(temp);
                temp = part.IndexOf('(', temp + 1);
            }
            List<int> closedIndexes = new List<int>();
            temp = part.IndexOf(')');
            while (temp != -1)
            {
                closedIndexes.Add(temp);
                temp = part.IndexOf(')', temp + 1);
            }
            if (openIndexes.Count != closedIndexes.Count)
            {
                return (false, "");
            }
            if (openIndexes.Count == 0)
            {
                return (true, part);
            }
            StringBuilder sb = new StringBuilder(part);
            Stack<int> opened = new Stack<int>();
            int itOpened = 0;
            int itClosed = 0;
            openIndexes.Add(-1);
            while(itClosed != closedIndexes.Count)
            { 
                while (openIndexes[itOpened] < closedIndexes[itClosed] && (itOpened != openIndexes.Count - 1))
                {
                    opened.Push(openIndexes[itOpened]);
                    ++itOpened;
                }
                if (opened.Count == 0)
                {
                    return (false, "");
                }
                int begin = opened.Pop();
                sb.Remove(begin, closedIndexes[itClosed] - begin + 1);
                sb.Insert(begin, "\\", closedIndexes[itClosed] - begin + 1);
                ++itClosed;
            }
            return (true, sb.Replace("\\", String.Empty).ToString());
        }
        private bool CheckDomainMinus()
        {
            if (_domain[0] == '-' || _domain[^1] == '-')
            {
                return false;
            }
            return true;
        }
        private bool CheckDomenSymbols(string commonDomain)
        {
            string temp = commonDomain.ToLower();
            foreach(char ch in temp)
            {
                if ((ch < 'a' || ch > 'z') && (ch < '0' || ch > '9') && ch != '-' && ch != '.')
                {
                    return false;
                }
            }
            return true;
        }
        private bool CheckSpecialSymbol(string commonLocal)
        {
            foreach (char ch in commonLocal)
            {
                if ((ch < '#' || ch > '+') &&
                    (ch < '-' || ch > '/') &&
                    (ch < '^' || ch > '`') &&
                    (ch < '{' || ch > '~') &&
                    ch != '!' &&
                    ch != '=' &&
                    ch != '?' &&
                    (ch < 'a' || ch > 'z') &&
                    (ch < '0' || ch > '9')
                    )
                {
                    return false;
                }
            }
            return true;
        }
        private (bool, string) RemoveQuotes(string commonLocal)
        {
            (bool, int, int) quotes = CheckQuotes();
            if (!quotes.Item1)
            {
                return (false, "");
            }
            string withoutQuotes = commonLocal;
            if (quotes.Item2 != -1)
            {
                if (quotes.Item2 != 0 && quotes.Item3 != commonLocal.Length - 1)
                {
                    withoutQuotes = commonLocal.Remove(quotes.Item2 - 1, quotes.Item3 - quotes.Item2 + 3);
                }
                else if (quotes.Item2 == 0 && quotes.Item3 != commonLocal.Length - 1)
                {
                    withoutQuotes = commonLocal.Remove(quotes.Item2, quotes.Item3 - quotes.Item2 + 2);
                }
                else if (quotes.Item2 != 0 && quotes.Item3 == commonLocal.Length - 1)
                {
                    withoutQuotes = commonLocal.Remove(quotes.Item2 - 1, quotes.Item3 - quotes.Item2 + 2);
                }
                else
                {
                    withoutQuotes = "";
                }
            }
            return (true, withoutQuotes);
        }
        private bool CheckSize()
        {
            if (_localPart.Length > LOCAL_LENGTH || _localPart.Length == 0)
            {
                return false;
            } 
            if (_domain.Length > DOMAIN_LENGTH || _domain.Length == 0) 
            {
                return false;
            }
            return true;
        }
        private bool CheckLocalBackSlash()
        {
            return _localPart.IndexOf('\\') == -1 ? true : false;
        }
        private bool CheckPoint(string commonLocal)
        {
            if (commonLocal[0] == '.' || commonLocal[^1] == '.')
            {
                return false;
            }
            int pointIndex = commonLocal.IndexOf('.');
            while (pointIndex != -1)
            {
                if (commonLocal[pointIndex + 1] == '.')
                {
                    return false;
                }
                pointIndex = commonLocal.IndexOf('.', pointIndex + 1);
            }
            return true;
        }
        private (bool, int, int) CheckQuotes()
        {
            int beginIndex = _localPart.IndexOf('\"');
            if (beginIndex != -1)
            {
                if (beginIndex != 0)
                {
                    if (_localPart[beginIndex - 1] != '.') 
                    {
                        return (false, -1, _localPart.Length);
                    }
                }
            }
            else
            {
                return (true, -1, _localPart.Length);
            }
            int endIndex = _localPart.IndexOf('\"', beginIndex + 1);
            if (endIndex != -1)
            {
                if (endIndex != _localPart.Length - 1)
                {
                    if (_localPart[endIndex + 1] != '.')
                    {
                        return (false, -1, _localPart.Length);
                    }
                }
                if(_localPart.IndexOf('\"', endIndex + 1) != -1)
                {
                    return (false, -1, _localPart.Length);
                }
                if (beginIndex + 1 == endIndex)
                {
                    return (false, -1, _localPart.Length);
                }
                return (true, beginIndex, endIndex);
            }
            return (false, -1, _localPart.Length);
        }
        private bool CheckAt(string commonLocal)
        {
            return commonLocal.IndexOf('@') == -1 ? true : false;
        }
        public override string ToString()
        {
            return _localPart + "@" + _domain;
        }
    }
}