namespace Task_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Garden garden = new Garden(@"C:\Users\nena\Desktop\Sigma курси\Home_task_5\Task 1\Data.txt");
            Console.WriteLine("~~~ The list of the trees (first string - amount of the trees and fence's length) ~~~");
            Console.WriteLine(garden);
            Console.WriteLine("~~~ The length of the shortest fence ~~~");
            garden.FindShortestFence();
            Console.WriteLine(garden.FenceLength.ToString("F3"));

            Garden anotherGarden = new Garden();
            Console.WriteLine("~~~ Second garden ~~~");
            Console.WriteLine(anotherGarden);
            Console.WriteLine("~~~ The shortest fence in the second garden ~~~");
            anotherGarden.FindShortestFence();
            Console.WriteLine(anotherGarden.FenceLength.ToString("F3"));
            Console.WriteLine("~~~ Check compare operators (garden 1 with garden 2) ~~~");
            Console.WriteLine(">  " + (garden > anotherGarden));
            Console.WriteLine("<  " + (garden < anotherGarden));
            Console.WriteLine(">= " + (garden >= anotherGarden));
            Console.WriteLine("<= " + (garden <= anotherGarden));
            Console.WriteLine("== " + (garden == anotherGarden));
            Console.WriteLine("!= " + (garden != anotherGarden));
        }
    }
}