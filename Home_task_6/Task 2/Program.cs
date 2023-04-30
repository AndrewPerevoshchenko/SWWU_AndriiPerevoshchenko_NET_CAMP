namespace Task_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arrayFirst = { 1, 2, 3, 4, 4, 1 };
            int[] arraySecond = { 1, 0, -3, 4, 0, 0 };
            int[] arrayThird = { 0, 0, -5, -5, -7, -3 };
            IEnumerable<int> a = Functions.MakeWholeArray(arrayFirst, arraySecond, arrayThird);
            foreach (int num in a)
            {
                Console.WriteLine(num);
            }
        }
    }
}