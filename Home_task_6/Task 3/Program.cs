namespace Task_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = Functions.FindUniqueWords("There is,^$#! this_ no there   %@$22 is unique uniqu words word as as  this these hm");
            foreach(string word in a)
            {
                Console.WriteLine(word);
            }
        }
    }
}