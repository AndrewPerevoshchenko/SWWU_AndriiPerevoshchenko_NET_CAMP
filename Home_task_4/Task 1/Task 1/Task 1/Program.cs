namespace Task_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode; //Для виводу української мови
            TextWork test = new TextWork(@"C:\Users\nena\Desktop\Sigma курси\Home_task_4\Task 1\Task 1\Task 1\Data.txt");
            Console.WriteLine("All text: " + test);
            List<string> result = test.FindBracketSentences();
            foreach(string s in result)
            {
                Console.WriteLine("\nSENTENCE >> \n" + s);
            }
        }
    }
}