using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal static class FileWorker
    {
        public static Store ReadStoreFromFile(string filePath) //Зчитуємо товари в магазин з файлу
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length > 0)
                {
                    Store result = new Store();
                    foreach (string line in lines)
                    {
                        string[] elements = line.Split(" | ", StringSplitOptions.RemoveEmptyEntries);
                        if (elements.Length >= 4) 
                        {
                            double.TryParse(elements[2], out double weight);
                            string[] dimensions = elements[3].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                            if (dimensions.Length != 3)
                            {
                                throw new InvalidDataException("Incorrect data (dimensions)");
                            }
                            double.TryParse(dimensions[0], out double length);
                            double.TryParse(dimensions[1], out double width);
                            double.TryParse(dimensions[2], out double height);
                            switch (elements[0])
                            {
                                case "C":
                                    result.AddMerchandise(new Clothes(elements[1].Trim(), weight, new Dimensions(length, width, height)));
                                    break;
                                case "E":
                                    if (elements.Length < 6)
                                    {
                                        throw new InvalidDataException("Not enough data for Electronics");
                                    }
                                    double.TryParse(elements[4], out double volumeStandart);
                                    double.TryParse(elements[5], out double exceedingSizeCoeff);
                                    result.AddMerchandise(new Electronics(elements[1].Trim(), weight, new Dimensions(length, width, height), volumeStandart, exceedingSizeCoeff));
                                    break;
                                case "F":
                                    if (elements.Length < 5)
                                    {
                                        throw new InvalidDataException("Not enough data for Foods");
                                    }
                                    double.TryParse(elements[4], out double urgencyCoeff);
                                    result.AddMerchandise(new Foods(elements[1].Trim(), weight, new Dimensions(length, width, height), urgencyCoeff));
                                    break;
                                default:
                                    throw new InvalidDataException("The type of the merchandise is not found");
                            }
                            continue;
                        }
                        throw new InvalidDataException("Incorrect data in a line");
                    }
                    return result;
                }
                throw new InvalidDataException("File is empty");
            }
            throw new FileNotFoundException("File is not found");
        }
        public static SortedDictionary<uint, double> ReadCoeffsFromFile(string filePath) //Для зчитування таблиці коефіцієнтів (вага або габарити)
            //Наприклад, на Новій пошті є розрахована вартість посилки відносно розмірів коробки та ваги (до 0.5кг, до 1кг, до 30кг тощо)
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length > 0)
                {
                    SortedDictionary<uint, double> result = new();
                    foreach (string line in lines)
                    {
                        string[] pair = line.Split(" | ");
                        if (pair.Length == 2)
                        {
                            if (uint.TryParse(pair[0], out uint volume) && double.TryParse(pair[1], out double coeff))
                            {
                                if (coeff > 0)
                                {
                                    result.Add(volume, coeff);
                                    continue;
                                }
                            }
                            throw new InvalidDataException("Incorrect data");
                        }
                        throw new InvalidDataException("Incorrect data in a line");
                    }
                    return result;
                }
                throw new InvalidDataException("File is empty");
            }
            throw new FileNotFoundException("File is not found");
        }
    }
}
