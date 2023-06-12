using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

internal static class ExternalMergeSort
{  
    private const uint AMOUNT_LIMIT = 50; //Максимальна кількість елементів, яка може поміститися в один "блок"
    public static void StartSorting(string inputFilePath, string outputFilePath = "")
    //Функція запуску сортування (inputFile - вхідний файл з невідсортованими даними, а після виконаня - з відсортованими (вихідний))
    {
        if (File.Exists(inputFilePath))
        {
            List<string> blocksPaths = CreateSortedBlocks(inputFilePath);
            while (blocksPaths.Count > 1)
            {
                blocksPaths = MergeEachBlockPair(blocksPaths);
            }
            if (outputFilePath == string.Empty)
            {
                outputFilePath = inputFilePath;
            }
            File.Move(blocksPaths[0], outputFilePath, true);
            return;
        }
        throw new FileNotFoundException("ERROR: File is not found");
    }
    private static List<string> CreateSortedBlocks(string inputFilePath)
    //Створюємо відсортовані блоки, отримуємо список шляхів до тимчасових файлів
    {
        List<string> blocksPaths = new List<string>(); //Список шляхів до тимчасових файлів (один тимчасовий файл містить не більше,
                                                      //ніж дозволена максимальна кількість елементів)
        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            List<int> block = new List<int>(); //Один блок елементів (кількість не більша максимальної). Буде відсортований та записаний у тимчасовий файл
            string? tempLine = reader.ReadLine(); //Змінна для зчитування пострічково (одна стрічка - один елемент: за потреби можна змінити зі Split())
            if (tempLine == string.Empty || tempLine == null)
            {
                throw new InvalidDataException("ERROR: File is empty");
            }
            do
            {
                if (int.TryParse(tempLine, out int number))
                {
                    block.Add(number);
                    if (block.Count >= AMOUNT_LIMIT)
                    {
                        block = DoBlockMergeSort(block); //Виконуємо MergeSort для блоку
                        string blockFilePath = Path.GetTempFileName(); //Шлях до тимчасового файлу з одним блоком
                        WriteBlockIntoFile(block, blockFilePath); //Записуємо інформацію блоку до файлу
                        blocksPaths.Add(blockFilePath); //Додаємо в список шляхів
                        block.Clear(); //Підчищаємо змінну
                    }
                    continue;
                }
                if (tempLine != string.Empty)
                {
                    throw new InvalidDataException("ERROR: File has not only integers");
                }
            }
            while ((tempLine = reader.ReadLine()) != null);
            if (block.Count > 0) //У випадку, якщо залишилися ще елементи в блоці (розмір блоку не досяг максимуму)
            {
                block = DoBlockMergeSort(block);
                string blockFilePath = Path.GetTempFileName();
                WriteBlockIntoFile(block, blockFilePath);
                blocksPaths.Add(blockFilePath);
            }
        }
        return blocksPaths;
    }
    private static List<int> DoBlockMergeSort(List<int> block) 
    //Звичайний MergeSort для блоку елементів
    {
        if (block.Count <= 1) return block;
        int middle = block.Count / 2;
        List<int> microBlockFirst = block.GetRange(0, middle);
        List<int> microBlockSecond = block.GetRange(middle, block.Count - middle);
        microBlockFirst = DoBlockMergeSort(microBlockFirst);
        microBlockSecond = DoBlockMergeSort(microBlockSecond);
        return MergeMicroBlocks(microBlockFirst, microBlockSecond);
    }
    private static List<int> MergeMicroBlocks(List<int> microBlockFirst, List<int> microBlockSecond) 
    //Метод для об'єднання двох блоків + сортування
    {
        List<int> sortedBlock = new();
        int i = 0, j = 0; 
        while (i < microBlockFirst.Count && j < microBlockSecond.Count)
        {
            if (microBlockFirst[i] <= microBlockSecond[j])
            {
                sortedBlock.Add(microBlockFirst[i]);
                ++i;
            }
            else
            {
                sortedBlock.Add(microBlockSecond[j]);
                ++j;
            }
        }
        while (i < microBlockFirst.Count)
        {
            sortedBlock.Add(microBlockFirst[i]);
            ++i;
        }
        while (j < microBlockSecond.Count)
        {
            sortedBlock.Add(microBlockSecond[j]);
            ++j;
        }

        return sortedBlock;
    }
    private static List<string> MergeEachBlockPair(List<string> blocksPaths) 
    //Об'єднуємо попарно блоки (викликатиметься рекурсивно вище)
    {
        List<string> mergedChunks = new();
        for (int i = 0; i < blocksPaths.Count; i += 2)
        {
            string blockPathFirst = blocksPaths[i];
            string? blockPathSecond = (i < blocksPaths.Count - 1) ? blocksPaths[i + 1] : null;
            string mergedBlocksPath = Path.GetTempFileName();
            MergeBlocks(blockPathFirst, blockPathSecond, mergedBlocksPath); //Об'єднуємо два блоки
            mergedChunks.Add(mergedBlocksPath); //Додаємо у список об'єднаних
            File.Delete(blockPathFirst); //Підчищаємо першу частинку
            if (blockPathSecond != null) File.Delete(blockPathSecond); //І другу, якщо вона існує, також підчищаємо
        }
        return mergedChunks;
    }
    private static void MergeBlocks(string blockPathFirst, string? blockPathSecond, string mergedBlocksPath) 
    //Функція об'єднання двох блоків в один
    {
        using (StreamReader srFirst = new StreamReader(blockPathFirst))
        using (StreamReader? srSecond = blockPathSecond != null ? new StreamReader(blockPathSecond): null)
        using (StreamWriter sw = new StreamWriter(mergedBlocksPath))
        {
            int? num1 = ReadInteger(srFirst);
            int? num2 = ReadInteger(srSecond);
            while (num1 != null || num2 != null)
            {
                if (num1 != null && (num2 == null || num1 <= num2))
                {
                    sw.WriteLine(num1);
                    num1 = ReadInteger(srFirst);
                }
                else
                {
                    sw.WriteLine(num2);
                    num2 = ReadInteger(srSecond);
                }
            }
        }
    }
    private static int? ReadInteger(StreamReader? reader) 
    //Зчитування стрічки - перетворення в число
    {
        if (reader != null)
        {
            string? line = reader.ReadLine();
            if (line != null)
            {
                if (int.TryParse(line, out int number))
                {
                    return number;
                }
                throw new InvalidDataException("ERROR: File has not only numbers");
            }
        }     
        return null;
    }
    private static void WriteBlockIntoFile(List<int> block, string blockFilePath) 
    //Для записування числа у файл
    {
        using (StreamWriter sw = new StreamWriter(blockFilePath))
        {
            foreach (int number in block)
            {
                sw.WriteLine(number);
            }
        }
    }
}