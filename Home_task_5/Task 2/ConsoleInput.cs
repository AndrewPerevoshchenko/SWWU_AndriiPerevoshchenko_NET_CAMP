using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal static class ConsoleInput
    {
        private const uint REQUIREMENT_FROM_USER = 3;
        private const uint DIMENSIONS_AMOUNT = 3;
        private const ConsoleKey _consoleKey = ConsoleKey.Tab;
        public static Department MakeHypermarket()
        {
            Console.WriteLine("Enter the amount of levels (hypermarket is zero level)");
            uint levelAmount = 0;
            uint.TryParse(Console.ReadLine(), out levelAmount);
            Department head = CreateHierarchy(levelAmount);
            AddGoods(ref head);
            return head;
        }
        private static Department CreateHierarchy(uint levelAmount) //Структура магазину
        {
            Queue<Department> values = new Queue<Department>();
            Console.WriteLine("Enter the name of the hypermarket: ");
            Department head = new Department(Console.ReadLine());
            if (head == null)
            {
                Environment.Exit(-1);
            }
            if (levelAmount == 0)
            {
                return head;
            }
            values.Enqueue(head);
            Console.WriteLine($"We have {levelAmount} levels. Let's write all departments for each level!");
            int queueWas = 1;
            for (int i = 1; i <= levelAmount; ++i)
            {
                Console.WriteLine($">>> LEVEL: {i}. Tap {_consoleKey} if you want to add new subdivision");
                for (int j = 0; j < queueWas; ++j)
                {
                    Department temp = values.Dequeue();
                    Console.WriteLine($">>> DEPARTMENT: {temp.Name}");
                    while (Console.ReadKey().Key == _consoleKey)
                    {
                        Department addedComponent = new Department(Console.ReadLine());
                        temp.Add(addedComponent);
                        values.Enqueue(addedComponent);
                    }
                }
                queueWas = values.Count;
            }
            return head;
        }
        private static Department AddGoods(ref Department head) //Товари
        {
            Console.WriteLine("Enter the amount of Goods >> ");
            uint amount = 0;
            uint.TryParse(Console.ReadLine(), out amount);
            Console.WriteLine("GOODS >> Add in format \"Name | Length Width Height | Pass by >\"");
            for (int i = 0; i < amount; ++i)
            {
                string? tempGoods = Console.ReadLine();
                if (tempGoods != null)
                {
                    string[] tempElements = tempGoods.Split(" | ");
                    if (tempElements.Length < REQUIREMENT_FROM_USER)
                    {
                        continue;
                    }
                    string[] tempDimensions = tempElements[1].Split(' ');
                    if (tempDimensions.Length < DIMENSIONS_AMOUNT)
                    {
                        continue;
                    }
                    Goods goods = new Goods(tempElements[0], double.Parse(tempDimensions[0]), double.Parse(tempDimensions[1]), double.Parse(tempDimensions[2]));
                    string[] pass = tempElements[2].Split('>');
                    Component way = head;
                    for (int j = 1; j < pass.Length; ++j)
                    {
                        if (way is Department) //Пошук адресою
                        {
                            way.Dimensions = new Dimensions(Math.Max(way.Dimensions._length, goods.Dimensions._length),
                                Math.Max(way.Dimensions._width, goods.Dimensions._width),
                                way.Dimensions._height + goods.Dimensions._height);
                            way = (way as Department).Components[(way as Department).Location[pass[j]]];
                        }
                    }
                    way.Add(goods);
                }
            }
            return head;
        }
    }
}
