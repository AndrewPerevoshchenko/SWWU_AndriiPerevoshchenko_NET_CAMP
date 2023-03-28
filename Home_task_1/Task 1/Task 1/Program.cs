using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class Program
    {//kjkhjkgkgkhg
        static void Main(string[] args)
        {
            MatrixGenerator test = new MatrixGenerator(7, 4);
            test.FormSnakeClockWise();
            Console.WriteLine(test.ToString());

            MatrixGenerator test2 = new MatrixGenerator(3, 5);
            test2.FormSnakeAntiClockWise();
            Console.WriteLine(test2.ToString());
        }
    }
}
