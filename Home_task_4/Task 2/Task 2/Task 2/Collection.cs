using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal class Collection
    {
        private List<Email> _emails;
        public List<Email> Emails
        {
            get { return _emails; }
        }
        public Collection(string fileURL) //Створюємо колекцію й одразу перевіряємо на правильність кожну пошту
        { 
            _emails = new List<Email>();
            List<string> _text = new List<string>();
            _text.AddRange(System.IO.File.ReadAllLines(fileURL));
            char[] separator = { '\n', '\t' }; //Увага! Пробіл не врахований як розділювач, адже він може існувати в середині адреси пошти (в коментарях або лапках)
            foreach (string item in _text)
            {                
                string[] temp = item.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string element in temp)
                {
                    int index = element.LastIndexOf('@');
                    if (index != 0 && index != element.Length - 1 && index != -1)
                    {
                        Email mail = new Email(element.Substring(0, index), element.Substring(index + 1));
                        mail.CheckEmail();
                        _emails.Add(mail);
                        
                    }
                }
            }
        }

        public (List<Email>, List<Email>) DivideByBool() //Розподіл на два табори: правильні й лексеми
        {
            List<Email> correct = new List<Email>();
            List<Email> uncorrect = new List<Email>();
            foreach (Email email in _emails)
            {
                if (email.IsEmail)
                {
                    correct.Add(email);
                }
                else
                {
                    uncorrect.Add(email);
                }
            }
            return (correct, uncorrect);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Email email in _emails)
            {
                sb.Append(email.ToString() + "\n");
            }
            return sb.ToString();
        }
    }
}
