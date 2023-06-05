namespace Task_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Store store = FileWorker.ReadStoreFromFile("C:\\Users\\nena\\Desktop\\Home_task_10\\Task 2\\Data\\DataStore.txt");
            Console.WriteLine(store);
            var dimensionsCoeffs = FileWorker.ReadCoeffsFromFile("C:\\Users\\nena\\Desktop\\Home_task_10\\Task 2\\Data\\DimensionsCoeffs.txt");
            var weightCoeffs = FileWorker.ReadCoeffsFromFile("C:\\Users\\nena\\Desktop\\Home_task_10\\Task 2\\Data\\WeightCoeffs.txt");
            Console.Write("Write down the base cost: ");
            double.TryParse(Console.ReadLine(), out double baseCost);
            IGoodsVisitor visitor = new GetDeliveryCostVisitor(baseCost, weightCoeffs, dimensionsCoeffs);
            var result = store.Accept(visitor);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }
    }
}