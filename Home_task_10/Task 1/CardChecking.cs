using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class CardChecking
    {      
        private HashSet<PaymentSystemCardTemplate> _templates;
        private List<(string, string)> _cards;
        public CardChecking()
        {
            _templates = new HashSet<PaymentSystemCardTemplate>();
            _cards = new List<(string, string)>();
        }
        public CardChecking(HashSet<PaymentSystemCardTemplate> templates, List<(string, string)> cards)
        {
            _templates = new HashSet<PaymentSystemCardTemplate>(templates);
            _cards = new List<(string, string)>(cards);
        }
        public void AddTemplate(PaymentSystemCardTemplate template)
        {
            if (template != null)
            {
                _templates.Add((PaymentSystemCardTemplate)template.Clone());
            }
        }
        public void AddCard(string str)
        {
            if (str != string.Empty)
            {
                string[] elements = str.Split('#', StringSplitOptions.RemoveEmptyEntries);
                if (elements.Length == 2)
                {
                    int quoteIndex = elements[1].IndexOf('\"');
                    if (quoteIndex != -1)
                    {
                        string cardNumber = elements[1].Substring(quoteIndex + 1, elements[1].Length - quoteIndex - 2);
                        _cards.Add((elements[0].Trim(), cardNumber.Trim()));
                        return;
                    }
                    throw new InvalidDataException("Incorrect format of the string");
                }
            }
        }
        public List<string> MakeCheckingReport()
        {
            List<string> report = new();
            string format = $"# {{{0}}} # card_number = \"{{{1}}}\" # {{{2}}}"; //Форматування для звіту
            List<PaymentSystemCardTemplate> templates = _templates.ToList();
            foreach(var pair in _cards)
            {
                PaymentSystemCardTemplate? cardTemplate = templates.Find(n => n.CardType == pair.Item1); //Шукаємо: чи існує платіжна система
                if (cardTemplate == null)
                {
                    report.Add(String.Format(format, pair.Item1, pair.Item2, "INCORRECT (Payment system doesn't exist)"));
                    continue;
                }
                bool correct = false; //Якщо все добре, то перевіримо припустимий розмір
                foreach(var item in cardTemplate.AccesibleLengths)
                {
                    if (pair.Item2.ToString().Length == item)
                    {
                        correct = true;
                        break;
                    }
                } 
                if (correct) //Якщо розмір підходить, то треба перевірити початок номеру карту (згідно з початками номерів у даній системі платіжній)
                {
                    correct = false;
                    foreach(var item in cardTemplate.AccesibleBegins)
                    {
                        if(pair.Item2.Substring(0, item.Length) == item) //Перевірка, власне
                        {
                            correct = true;
                            break;
                        }
                    }
                    if (correct) //Якщо все добре, то залишається лише алгоритм Луна
                    {
                        if (CheckCorrectnessByLuna(pair.Item2))
                        {
                            report.Add(String.Format(format, pair.Item1, pair.Item2, "CORRECT"));
                        }
                        else
                        {
                            report.Add(String.Format(format, pair.Item1, pair.Item2, "INCORRECT (Unaccessible card number)"));
                        }       
                    }
                    else
                    {
                        report.Add(String.Format(format, pair.Item1, pair.Item2, "INCORRECT (Unaccessible card number beginning for that type)"));
                    }              
                }
                else
                {
                    report.Add(String.Format(format, pair.Item1, pair.Item2, "INCORRECT (Unaccessible length for that type)"));
                }     
            }
            _cards.Clear(); //Очистка списку перевірки (за потреби можна в майбутньому замінити список на чергу)
            return report;
        }
        private bool CheckCorrectnessByLuna(string cardNumber) //Алгоритм Луна
        {
            Dictionary<char, int> checkingDouble = new Dictionary<char, int>()
            {
                {'0', 0 }, 
                {'1', 2 },
                {'2', 4 },
                {'3', 6 },
                {'4', 8 },
                {'5', 1 }, //10 (після подвоєння) - сума 1
                {'6', 3 }, //12 (після подвоєння) - сума 3
                {'7', 5 }, //...
                {'8', 7 },
                {'9', 9 }
            };
            if (cardNumber.Length % 2 != 0)
            {
                cardNumber = "0" + cardNumber; //Якщо непарна кількість цифр у номері, то додамо ще цифру попереду (для оптимізації циклу нижче)
            }
            int sum = 0;
            for (int i = cardNumber.Length - 1; i > 0; i -= 2)
            {
                sum += (cardNumber[i] - '0') + checkingDouble[cardNumber[i - 1]]; //У суму додаємо парами: дана цифра + цифра, яка заміниться при подвоєнні
            }
            return sum % 10 == 0;
        }
    }
}
