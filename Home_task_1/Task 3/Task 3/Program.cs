namespace Task_3

{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cube test = new Cube(3);
            test.FillCubeRandomly();
            List<(NodeCoordinates, NodeCoordinates)> result = test.FindThroughHoles();
            foreach (var item in result)
            {
                Console.WriteLine($"Begin: {item.Item1}, End: {item.Item2}");
            }
            Console.WriteLine("\nTrue -> is empty\n");
            Console.WriteLine(test.ToString());
        }
    }
}