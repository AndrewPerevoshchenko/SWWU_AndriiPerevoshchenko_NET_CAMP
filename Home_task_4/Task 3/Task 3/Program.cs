namespace Task_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EnergySystem energySystem = new EnergySystem(@"C:\Users\nena\Desktop\Sigma курси\Home_task_4\Task 3\Task 3\Data.txt");
            Console.WriteLine(energySystem);
            int flatNumber;
            Console.Write("~~~ Enter flat number just to see information about the flat >> ");
            int.TryParse(Console.ReadLine(), out flatNumber);
            if (flatNumber > 0 && flatNumber <= energySystem.FlatAmount)
            {
                Console.WriteLine(energySystem.Users[flatNumber - 1]);
            }
            Console.WriteLine("~~~ There is the owner with the biggest debt ~~~\n");
            Console.WriteLine(energySystem.FindMaxDebtor() + "\n");
            List<uint> lifelessList = energySystem.FindLifelessFlat();
            Console.WriteLine("~~~ The flats with no electricity usage ~~~\n");
            foreach (uint item in lifelessList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine("~~~ Count the electricity expences of each flat ~~~\n");
            Dictionary<(uint, string), string> expences = energySystem.CountExpenses();
            foreach(var item in expences)
            {
                Console.WriteLine(item.Key.Item1 + " | " + item.Key.Item2 + ": " + item.Value);
            }
            Console.WriteLine("~~~ The difference between dates of each flat ~~~ \n");
            List<(uint, DateTime, uint)> diffDays = energySystem.CountDifferenceBetweenDates(new DateTime(2023, 05, 01));
            Console.WriteLine("Flat | Last removal date | Difference");
            foreach(var item in diffDays)
            {
                Console.WriteLine(item.Item1 + " | " + item.Item2.ToString("dd.MM.yy") + " | " + item.Item3);
            }
        }
    }
}