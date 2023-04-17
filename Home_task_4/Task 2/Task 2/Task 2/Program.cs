namespace Task_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Collection emails = new Collection("C:\\Users\\nena\\Desktop\\Sigma курси\\Home_task_4\\Task 2\\Task 2\\Task 2\\Data.txt");
            Console.WriteLine(emails);
            List<Email> listEmails; //Правильні адреси електронної пошти
            List<Email> listLexems; //Лексеми із символом "@". Без нього - ігноруються взагалі
            var tuple = emails.DivideByBool();
            listEmails = tuple.Item1;
            listLexems = tuple.Item2;
            Console.WriteLine("<><><> Correct emails:");
            foreach(Email email in listEmails)
            {
                Console.WriteLine(email);
            }
            Console.WriteLine("\n<><><> Lexems with @:");
            foreach (Email email in listLexems)
            {
                Console.WriteLine(email);
            }
        }
    }
}