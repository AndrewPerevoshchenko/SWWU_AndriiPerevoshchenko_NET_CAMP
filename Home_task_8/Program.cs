namespace Home_task_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Останній вивід для кожного симулятора - це стан світлофорів у час, який указує користувач!
            SimulatorsGroup simulatorsGroup = FileWorker.CreateCrossroadsGroupSimulator(
                "C:\\Users\\nena\\Desktop\\Sigma курси\\Home_task_8\\Data\\Crossroads.txt", 
                "C:\\Users\\nena\\Desktop\\Sigma курси\\Home_task_8\\Data\\Crossroads1.txt");
            //Достатньо лише передати адреси файлів з перехрестями. Симулятор створить уже все сам (включно з окремими симуляціями)
            Console.WriteLine("Enter the ceils of timers just to start simulators");
            double.TryParse(Console.ReadLine(), out double timeCeil1);
            double.TryParse(Console.ReadLine(), out double timeCeil2);
            simulatorsGroup.StartSimulator(0, timeCeil1); //Запускаємо перший по айді
            simulatorsGroup.StartSimulator(1, timeCeil2); //І другий симулятор по айді
            Console.WriteLine(simulatorsGroup); //Виводимо загальний лог
        }
    }
}