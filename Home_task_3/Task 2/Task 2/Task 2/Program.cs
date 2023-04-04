using System;

namespace Task_2
{
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
