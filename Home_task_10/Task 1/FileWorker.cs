using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal static class FileWorker
    {
        public static List<(string, string)> ReadTypesNumberswFromFile(string path) //Зчитуємо пари "Платіжна система - номер картки"
        {
            List<(string, string)> result = new();
            if (File.Exists(path))
            {
                string[] temp = File.ReadAllLines(path);
                if (temp.Length > 0)
                {
                    foreach (string line in temp)
                    {
                        string[] tempPair = line.Split("#", StringSplitOptions.RemoveEmptyEntries);
                        if (tempPair.Length == 2)
                        {
                            int quoteIndex = tempPair[1].IndexOf('\"');
                            if (quoteIndex != -1)
                            {
                                string cardNumber = tempPair[1].Substring(quoteIndex + 1, tempPair[1].Length - quoteIndex - 2);
                                result.Add((tempPair[0].Trim(), cardNumber.Trim()));
                                continue;
                            }
                            throw new InvalidDataException("Incorrect format of the string");
                        }
                        throw new InvalidDataException("Incorrect amount of elements in the string");
                    }
                    return result;
                }
                throw new InvalidDataException("File is empty");
            }
            throw new FileNotFoundException("File is not found");
        }
        public static HashSet<PaymentSystemCardTemplate> ReadTemplatesFromFile(string path) //Зчитуємо шаблони карток платіжних систем
        {
            HashSet<PaymentSystemCardTemplate> result = new();
            if (File.Exists(path))
            {
                string[] temp = File.ReadAllLines(path);
                if (temp.Length > 0)
                {
                    foreach (string line in temp)
                    {
                        string[] tempThree = line.Split(" | ", StringSplitOptions.RemoveEmptyEntries);
                        if (tempThree.Length == 3)
                        {
                            if (tempThree[0] == string.Empty) throw new InvalidDataException("Card name cannot be empty");
                            string[] begins = tempThree[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            string[] lengths = tempThree[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);       
                            HashSet<string> cardBegins = new HashSet<string>();
                            HashSet<uint> cardLengths = new HashSet<uint>();
                            foreach(string element in begins)
                            {
                                if(uint.TryParse(element, out uint e))
                                {
                                    cardBegins.Add(element);
                                }
                                
                            }
                            foreach(string element in lengths)
                            {
                                if (uint.TryParse(element, out uint e))
                                {
                                    cardLengths.Add(e);
                                }
                            }
                            if (cardLengths.Count == 0 || cardBegins.Count == 0) throw new InvalidDataException("Incorrect data of the accessible card begins or lengths");
                            result.Add(new PaymentSystemCardTemplate(tempThree[0].Trim(), cardBegins, cardLengths));
                            continue;
                        }
                        throw new InvalidDataException("Incorrect amount of elements in the string");
                    }
                    return result;
                }
                throw new InvalidDataException("File is empty");
            }
            throw new FileNotFoundException("File is not found");
        }
    }
}
