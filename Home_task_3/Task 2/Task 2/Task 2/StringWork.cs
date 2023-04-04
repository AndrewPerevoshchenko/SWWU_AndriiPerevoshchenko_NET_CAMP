using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
	//Спробував скористатися різними підходами: звичайними методами, регулярними функціями та LINQ
	public class StringWork
	{
		private string[] _text;
		public StringWork(string fileURL)
		{
			_text = System.IO.File.ReadAllLines(fileURL);
		}
		public (uint?, uint?) FindSecondSubstring(string subString)
		{
			uint i;
			for(i = 0; i < _text.Length; ++i) //Пошук першої підстрічки
			{
				if (_text[i].Contains(subString))//Якщо є підстрічка, то...
				{
					uint indexTemp = (uint)_text[i].IndexOf(subString);
					string strTemp = _text[i].Substring((int)indexTemp + subString.Length);
					if (strTemp.Contains(subString)) //...перевіримо: чи є друга в цій же стрічці
					{
						return (i, (uint)strTemp.IndexOf(subString) + indexTemp + (uint)subString.Length);
					}
					break;
				}
			}
			for(uint j = i + 1; j < _text.Length; ++j) //Якщо не знайшли дві пістрічки в одній стрічці, то перевіряємо далі
			{
				if (_text[j].Contains(subString))
				{
					return (j, (uint)_text[j].IndexOf(subString));
				}
			}
			return (null, null);
		}
		public uint EnumerateCapital()
		{
			uint counter = 0;
			foreach (string str in _text)
			{
				counter += (uint)str.Where(c => char.IsUpper(c)).Count(); //Рахуємо всі слова в стрічці, де літера збігається з літерою у верхньому регістрі
			}
			return counter;
		}
		public void ReplaceDoubles(string replacement) 
		{
			for(int i = 0; i < _text.Length; ++i)
			{
				string[] temp = _text[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
				foreach(string item in temp)
				{
					int count = 0;
					count += System.Text.RegularExpressions.Regex.Matches(item, @"(.)\1").Count; //Рахуємо скільки повторювальних літер підряд
					if(count > 0)
					{
						_text[i] = _text[i].Replace(item, replacement); //Заміняємо слово, якщо маємо подвоєння
					}
				}
			}			
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach(string str in _text)
			{
				sb.Append(str + "\n");
			}
			return sb.ToString();
		}
	}
}
