using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    internal static class Functions
    {
        public static IEnumerable<string> FindUniqueWords(string text)
        {
            string clearString = new string(text.Where(c => char.IsLetter(c) || c == ' ').ToArray()); //Почистимо стрічку від сміття
            string[] words = clearString.Split(' ', StringSplitOptions.RemoveEmptyEntries); 
            for (int i = 0; i < words.Length; ++i)
            {
                bool unique = true;
                int j = 0, k = 0;
                for (j = 0, k = words.Length - 1; j < i && k > i; ++j, --k) //Дзеркальний цикл, шукаємо, доки не дійдемо хоча б з одного боку до i елемента
                {
                    if (Comparing(words[i], words[j]) || Comparing(words[i], words[k]))
                    {
                        unique = false;
                        break;
                    }
                }
                if (unique) //Якщо залишилося унікальним, тоді перевіримо ще слова між j та i або i та k (залежно, з якого боку ще залишилися). Один з наступних двох циклів точно омине
                {
                    for (int l = k; l > i; --l) 
                    {
                        if (Comparing(words[i], words[l]))
                        {
                            unique = false;
                            break;
                        }
                    }
                    for (int l = j; j < i; ++j)
                    {
                        if (Comparing(words[i], words[l]))
                        {
                            unique = false;
                            break;
                        }
                    }
                    if (unique) //Якщо досі унікальне - точно унікальне
                    {
                        yield return words[i];
                    }  
                }
            }
        }
        public static bool Comparing(string left, string right) //Вважаємо, що слова рівні в будь-якому регістрі
        {
            return left.ToLower() == right.ToLower();
        }
    }
}
