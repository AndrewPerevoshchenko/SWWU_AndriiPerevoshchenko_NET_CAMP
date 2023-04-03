using HT_23._03._23;

namespace Task_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1 - сам елемент, 4 - вимір, 3 - кількість елементів у векторі, 2 - кількість векторів у матриці, 3 - кількість матриць
            Tensor<int> a = new Tensor<int>(1, 4, 3, 2, 3);
            a.createUnitTensor();
            Console.WriteLine(a);
        }
    }
}