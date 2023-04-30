using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    internal static class Functions
    { 
        public static IEnumerable<int> MakeWholeArray(params int[][] arrays) //Створюємо масив з усіх масивів, не викидаючи жоден елемент з вихідних даних
        {
            int maximum = int.MinValue; //Максимумом обмежимо цикл while нижче
            foreach (int[] array in arrays)
            {
                if (array.Max() > maximum)
                {
                    maximum = array.Max();
                }
            }             
            int lastElementIncremented = int.MinValue; //Останнє додане мінімальне
            while (lastElementIncremented <= maximum) //Поки останнє додане плюс одиниця не перейде максимум
            {
                int minimum = int.MaxValue; //Змінна, що буде відповідати за мінімальне число
                foreach (int[] array in arrays) //Шукаємо мінімальне значення
                {
                    foreach (int element in array)
                    {
                        if (element < minimum && element >= lastElementIncremented) //Якщо число менше за можливий мінімум і точно не менше за останнє додане плюс одиниця
                        {
                            minimum = element;
                        }
                    }
                    
                }
                int amount = 0;
                foreach (int[] array in arrays) //Рахуємо кількість мінімумів у кожному масиві
                {
                    amount += array.Count(n => n == minimum);
                }
                for (int i = 0; i < amount; ++i) //Робимо yield return необхідну кількість разів
                {
                    yield return minimum;
                }
                lastElementIncremented = minimum + 1; //Останнє додане інкрементед
            }   
        }


    }
}
