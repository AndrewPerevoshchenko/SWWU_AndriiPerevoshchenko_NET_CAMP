namespace Home_task_7
{
    internal class Program
    // сумарний бал  94
    {// загалом немає абстракції. Ви все будуєте на конкретних класах. А це не дасть гарно розвиватись.
        static void Main(string[] args) //Тут можна було переробити хард-код на введення користувачем. Задаються світлофори та таймінги переключення кольорів.
        {//світлофори мав би створювати сам симулятор.Навіщо це робити в main?  Крім того назви ідентифікаторів простошикарно інформативні)
            TrafficLight a = new TrafficLight(GeographicalDirection.North, GeographicalDirection.East, TrafficColour.Red);
            TrafficLight a3 = new TrafficLight(GeographicalDirection.South, GeographicalDirection.East, TrafficColour.Red);
            TrafficLight a1 = new TrafficLight(GeographicalDirection.East, GeographicalDirection.East, TrafficColour.Red);
            TrafficLight a2 = new TrafficLight(GeographicalDirection.West, GeographicalDirection.East, TrafficColour.Red);

            // Звичайно, що ці константи користувач мав би обирати. В якому класі? В якій технології? А якщо треба в різній? 
            ColoursTimer b = new ColoursTimer(4, 2, 5, 1);
            TrafficSimulator aa = new TrafficSimulator(b, a, a3, a1, a2);
            Console.WriteLine(aa.StartSchemeTOS(12)); //Перегляд на 12 секунд (останній запис - стан системи в даний час, незалежно від заголовку з числом)
        }
    }
}
