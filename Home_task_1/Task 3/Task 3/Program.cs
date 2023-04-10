namespace Task_3

{
    internal class Program
    {
        // На скільки я зрозуміла, Ви хочете розв'язати задачу, коли не просто наскрізь бачимо дірку, а коли заливаємо воду в комірку і очікуємо, 
        //що вода на протилежному краю розлиється, при умові, що крім протилежних граней інші є ізольовані. І Використовуєте теорію графів, де вони подані 
        //у вигляді списку суміжності. І використовуєте алгоритм пошуку шляху від вершин до вершин. Це хороша ідея, яка, на жаль, не працюватиме на великих розмірностях.
        // Додаткові бали за цю задачу Ви отримуєте: 18 з 20.
        
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
