using System;

namespace Task_2
{//У діаграмі є проблеми. Клас користувач, помпа і вежа входять в симулятор, як мінімум, агрегаційно. Хто створює ці сутності взагалі не зрозуміло! У Вас вони передаються через конструктор. а хто створює? 
	// Користувач не має методу, який активує взяти воду. Це означає, що його поведінкою повністю керуватиме симулятор. А він мав би тільки зав'язати класи. Поставте собі запитання, яка ціль класу симулятор і постарайтесь відповісти 
	// одним реченням. Функціональність класів ще слабо продумана. Це Ваша важлива задача!!!
	public class Program
	{
		static void Main(string[] args)
		{
			StringWork sw = new StringWork(@"C:\Users\Андрей\Desktop\Home_task_3\Task 2\Task 2\Task 2\Data.txt");
			(uint?, uint?) result = sw.FindSecondSubstring("Lorem");
			Console.WriteLine(result.Item1 + " " + result.Item2);
			Console.WriteLine(sw.EnumerateCapital());
			sw.ReplaceDoubles("REPLACED");
			Console.WriteLine(sw);
		}
	}
}
