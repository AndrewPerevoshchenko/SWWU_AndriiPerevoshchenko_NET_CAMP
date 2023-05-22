using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_task_9
{
    internal static class FileWorker
    {
        public static List<Cook> ReadCooksFromFileEmployingRandomly(string filePath)
        {
            if (File.Exists(filePath))
            {          
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length != 0)
                {
                    List<Cook> cooks = new List<Cook>();
                    foreach (string line in lines)
                    {
                        cooks.Add(EmployCookRandomly(line));
                    }
                    cooks.Sort();
                    return cooks;   
                }
                throw new InvalidDataException("File is empty");
            }
            throw new FileNotFoundException("File is not found");
        }
        public static OrdersGroup MakeOrdersGroupFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length != 0)
                {
                    OrdersGroup ordersGroup = new OrdersGroup();
                    foreach (string line in lines)
                    {
                        string[] tempLine = line.Split(" | ");
                        if (tempLine.Length >= 4)
                        {
                            uint.TryParse(tempLine[2], out uint amount);
                            double.TryParse(tempLine[3], out double time);
                            Enum.TryParse(typeof(DishType), tempLine[1], out var dishType);
                            Order order = new Order(tempLine[0], (DishType)dishType, amount, time);
                            ordersGroup.AddDishOrder(order);
                            continue;
                        }
                        throw new InvalidDataException("Data from the file is uncorrect ");
                    }
                    return ordersGroup;
                }
                throw new InvalidDataException("File is empty");
            }
            throw new FileNotFoundException("File is not found");
        }
        private static Cook EmployCookRandomly(string surname)
        {
            Random rnd = new Random();
            CookType type = (CookType)rnd.Next(0, Enum.GetNames(typeof(CookType)).Length + 1);
            switch (type)
            {
                default:
                case CookType.Pizzaman:
                    return new Pizzaman(surname);
                case CookType.Confectioner:
                    return new Confectioner(surname);
                case CookType.Bartender:
                    return new Bartender(surname);
            }
        }
    }
}
