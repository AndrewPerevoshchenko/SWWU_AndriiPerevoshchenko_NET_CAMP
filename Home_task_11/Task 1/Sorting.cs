using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    public static class Sorting<T> where T : IComparable<T> 
    //Сортування можливо, якщо для Т реалізований інтерфейс відповідний
    {
        private delegate int PivotChoosing(in T[] array, int beginIndex, int endIndex); 
        //Делегат, щоб підставляти функції для pivot-елемента
        private static void SwapElements(T[] array, int indexFirst, int indexSecond) 
        //Функція перестановки (глибока копія не потрібна для посилань не потрібна)
        {
            T temp = array[indexFirst];
            array[indexFirst] = array[indexSecond];
            array[indexSecond] = temp;
        }
        private static (int, int) DividePartsDNF(T[] array, int beginIndex, int endIndex, PivotChoosing pivotFunction) 
        //Використано алгоритм "Національного прапору Нідерландів" ( O(N * logN) + допомога у випадку великої кількості повторень)
        {
            int biggestLeft = beginIndex;
            int middleElement = beginIndex;
            int smallestRight = endIndex;
            T pivotElement = array[pivotFunction.Invoke(array, beginIndex, endIndex)];
            while (middleElement <= smallestRight) //Поки не досягнемо правої частини
            {
                if (array[middleElement].CompareTo(pivotElement) == -1) //Усі менші за pivot - наліво, крайній правий елемент першого блоку посуваємо
                {
                    SwapElements(array, biggestLeft, middleElement);
                    biggestLeft++;
                    middleElement++;
                }
                else if (array[middleElement].CompareTo(pivotElement) == 0) //Якщо рівні pivot, то залишаємо у другому блоці
                {
                    middleElement++;
                }
                else //У третій блок потравляють елементи, що більші за pivot (крайня ліва межа посувається аналогічно)
                {
                    SwapElements(array, middleElement, smallestRight);
                    smallestRight--;
                }
            }
            (int, int) pivotIndices = ( biggestLeft, smallestRight ); //повертаємо межі першого та третього шарів
            return pivotIndices;
        }
        private static void DoQuickSort(T[] array, int beginIndex, int endIndex, PivotChoosing pivotFunction)
        {        
            if (beginIndex < endIndex)
            {
                (int, int) indexes = DividePartsDNF(array, beginIndex, endIndex, pivotFunction); //Виконуємо partition
                DoQuickSort(array, beginIndex, indexes.Item1 - 1, pivotFunction); //Далі сортуємо перший прошарок
                DoQuickSort(array, indexes.Item2 + 1, endIndex, pivotFunction); //А тут третій прошарок (другий уже відсортовано)
            }
        }
        public static void StartQuickSorting(T[] array, PivotType pivotType) //Запуск QuickSort з можливістю вибору типу сортування
        {
            PivotChoosing pivotChoosing; //Делегат
            switch (pivotType)
            {
                default:
                case PivotType.FirstElement:
                    pivotChoosing = FindPivotAsFirstElement;
                    break;
                case PivotType.RandomElement:
                    pivotChoosing = FindRandomPivot;
                    break;
                case PivotType.Median:
                    pivotChoosing = FindMedianPivot;
                    break;
            }
            DoQuickSort(array, 0, array.Length - 1, pivotChoosing); //Викликаємо сам метод
        }
        private static int FindPivotAsFirstElement(in T[] array, int beginIndex, int endIndex) //Якщо опорний елемент - перший, переставимо з кінцевим
        {
            SwapElements(array, beginIndex, endIndex);
            return endIndex;
        }
        private static int FindRandomPivot(in T[] array, int beginIndex, int endIndex) //Якщо опорний елемент - рандомний, переставимо з кінцевим
        {
            Random rand = new Random();
            int pivotIndex = rand.Next() % (endIndex - beginIndex) + beginIndex;
            SwapElements(array, pivotIndex, endIndex);
            return endIndex;
        }
        private static int FindMedianPivot(in T[] array, int beginIndex, int endIndex) //Шукаємо елемент по-середині й робимо необхідні перестановки
        {
            int pivotIndex = beginIndex + (endIndex - beginIndex) / 2;
            if (array[pivotIndex].CompareTo(array[beginIndex]) == -1)
                SwapElements(array, beginIndex, pivotIndex);
            if (array[endIndex].CompareTo(array[beginIndex]) == -1)
                SwapElements(array, beginIndex, endIndex);
            if (array[endIndex].CompareTo(array[pivotIndex]) == -1)
                SwapElements(array, pivotIndex, endIndex);
            return pivotIndex;
        }
    }
}
