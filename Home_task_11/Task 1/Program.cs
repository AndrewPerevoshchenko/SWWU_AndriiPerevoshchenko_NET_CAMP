using System.Diagnostics;

namespace Task_1
{
    
    internal class Program
    {
        //Тут просто хардкодом написані функції для візуалізації (на технічну частину не впливають)
        public static int[] ReadArrayFromFile(string path) //Хардкодове зчитування з файлу
        {
            if (File.Exists(path))
            {
                string[] temp = File.ReadAllLines(path);
                int.TryParse(temp[0], out int value);
                int[] result = new int[value];
                string[] elements = temp[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                for(int i = 0; i < elements.Length && i < result.Length; ++i) 
                {
                    result[i] = Convert.ToInt32(elements[i]);
                }
                return result;
            }
            throw new FileNotFoundException();
        }
        public static (long, int[]) DoQuickSortWithTiming<T>(T[] array, PivotType pivotType) //Підрахунок часу + використання копії                                                                           
        {
            int[] copyArray = new int[array.Length];
            Array.Copy(array, copyArray, array.Length);
            Stopwatch sw = Stopwatch.StartNew();
            Sorting<int>.StartQuickSorting(copyArray, pivotType);
            sw.Stop();
            return (sw.ElapsedTicks, copyArray);
        }
        public static void PrintArray<T>(T[] array) //Роздруківка масивів
        {
            foreach (T item in array)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine("\n");
        }
        public static void Main(string[] args)
        {
            int[] array = ReadArrayFromFile("C:\\Users\\nena\\Desktop\\Sigma курси\\Home_task_11\\Task 1\\Data.txt");
            Console.WriteLine("INPUT ARRAY:");
            PrintArray(array);

            var testFirst = DoQuickSortWithTiming(array, PivotType.FirstElement);
            Console.WriteLine($"TYPE: FirstElement  || TIME: {testFirst.Item1}");
            PrintArray(testFirst.Item2);
            //Час у першому використанні показується неправильно (незалежно від типу опорного елемента)

            var testSecond = DoQuickSortWithTiming(array, PivotType.RandomElement);
            Console.WriteLine($"TYPE: RandomElement || TIME: {testSecond.Item1}");
            PrintArray(testSecond.Item2);
            //Тут уже більше схоже на правду

            var testThird = DoQuickSortWithTiming(array, PivotType.Median);
            Console.WriteLine($"TYPE: MedianElement || TIME: {testThird.Item1}");
            PrintArray(testThird.Item2);
            //Аналогічно попередньому (правдиво більш-менш)
        }
    }
}