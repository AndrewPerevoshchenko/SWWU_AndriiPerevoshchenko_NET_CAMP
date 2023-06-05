namespace Task_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Зчитуємо шаблони карт
            HashSet<PaymentSystemCardTemplate> cardTemplates = FileWorker.ReadTemplatesFromFile("C:\\Users\\nena\\Desktop\\Home_task_10\\Task 1\\PaymentSystem.txt");
            //Зчитуємо пари "Можлива платіжна система - можливий номер картки"
            List <(string, string)> typesNumbersPairs = FileWorker.ReadTypesNumberswFromFile("C:\\Users\\nena\\Desktop\\Home_task_10\\Task 1\\Data.txt");
            //Передаємо дані на валідацію
            CardChecking cardChecking = new CardChecking(cardTemplates, typesNumbersPairs);
            //Маємо звіт по картках
            var report = cardChecking.MakeCheckingReport();
            Console.WriteLine("RERORT #1");
            foreach (string str in report)
            {
                Console.WriteLine(str);
            }
            //Додаємо нову картку згідно з форматом (старі дані мали вже почиститися, адже вже перевірили)
            cardChecking.AddCard("# MasterCard # card_number = \"5255555555554444\"");
            //Новіий звіт
            report = cardChecking.MakeCheckingReport();
            Console.WriteLine("RERORT #2");
            foreach (string str in report)
            {
                Console.WriteLine(str);
            }
        }
    }
}