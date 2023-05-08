namespace Home_task_7
{
    internal class Program
    {
        static void Main(string[] args) //Тут можна було переробити хард-код на введення користувачем. Задаються світлофори та таймінги переключення кольорів.
        {
            TrafficLight a = new TrafficLight(GeographicalDirection.North, GeographicalDirection.East, TrafficColour.Red);
            TrafficLight a3 = new TrafficLight(GeographicalDirection.South, GeographicalDirection.East, TrafficColour.Red);
            TrafficLight a1 = new TrafficLight(GeographicalDirection.East, GeographicalDirection.East, TrafficColour.Red);
            TrafficLight a2 = new TrafficLight(GeographicalDirection.West, GeographicalDirection.East, TrafficColour.Red);

            ColoursTimer b = new ColoursTimer(4, 2, 5, 1);
            TrafficSimulator aa = new TrafficSimulator(b, a, a3, a1, a2);
            Console.WriteLine(aa.StartSchemeTOS(12)); //Перегляд на 12 секунд (останній запис - стан системи в даний час, незалежно від заголовку з числом)
        }
    }
}
